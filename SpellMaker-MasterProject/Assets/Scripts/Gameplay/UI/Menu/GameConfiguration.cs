using Mono.Cecil;
using SMUBE.AI;
using SMUBE.Units;
using SMUBE.Units.CharacterTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameConfiguration : MonoBehaviour
{
    [SerializeField] private List<CharacterSlotEntry> team1CharacterSlots;
    [SerializeField] private List<CharacterSlotEntry> team2CharacterSlots;

    private Dictionary<int, List<BaseCharacterType>> _configuration = new();

    bool initialized = false;

    public void BeginSetup()
    {
        if (initialized)
        {
            return;
        }

        initialized = true;

        team1CharacterSlots[0].Setup(0, OnTeam1UnitRemoved, OnTeam1UnitAdded, CharacterSlotState.Active);
        team1CharacterSlots[1].Setup(1, OnTeam1UnitRemoved, OnTeam1UnitAdded);
        team1CharacterSlots[2].Setup(2, OnTeam1UnitRemoved, OnTeam1UnitAdded);

        team2CharacterSlots[0].Setup(0, OnTeam2UnitRemoved, OnTeam2UnitAdded, CharacterSlotState.Active, altColorScheme: true);
        team2CharacterSlots[1].Setup(1, OnTeam2UnitRemoved, OnTeam2UnitAdded, altColorScheme: true);
        team2CharacterSlots[2].Setup(2, OnTeam2UnitRemoved, OnTeam2UnitAdded, altColorScheme: true);

        _configuration.Add(0, new List<BaseCharacterType>());
        _configuration.Add(1, new List<BaseCharacterType>());

        RefeshControlButtonState(team1CharacterSlots);
        RefeshControlButtonState(team2CharacterSlots);
    }

    private void OnTeam1UnitAdded(int slotId)
    {
        OnTeamUnitAdded(team1CharacterSlots, slotId);
        RefeshControlButtonState(team1CharacterSlots);
    }

    private void OnTeam2UnitAdded(int slotId)
    {
        OnTeamUnitAdded(team2CharacterSlots, slotId);
        RefeshControlButtonState(team2CharacterSlots);
    }

    private void OnTeamUnitAdded(List<CharacterSlotEntry> teamList, int slotId)
    {
        var activeUnitsCount = teamList.Count(unit => unit.CharacterSlotState == CharacterSlotState.Active);

        if (activeUnitsCount < 3)
        {
            teamList[slotId].SetState(CharacterSlotState.Active);
        }
    }

    private void OnTeam1UnitRemoved(int slotId)
    {
        RemoveTeamUnit(team1CharacterSlots, slotId);
        RefeshControlButtonState(team1CharacterSlots);
    }

    private void OnTeam2UnitRemoved(int slotId)
    {
        RemoveTeamUnit(team2CharacterSlots, slotId);
        RefeshControlButtonState(team2CharacterSlots);
    }

    private void RemoveTeamUnit(List<CharacterSlotEntry> teamList, int slotId)
    {
        teamList[slotId].SetState(CharacterSlotState.Empty);
    }

    private void RefeshControlButtonState(List<CharacterSlotEntry> teamList)
    {
        var activeUnitsCount = teamList.Count(unit => unit.CharacterSlotState == CharacterSlotState.Active);

        for (int i = 0; i < teamList.Count; i++)
        {
            CharacterSlotEntry unit = teamList[i];

            if (activeUnitsCount == 1)
            {
                if (unit.CharacterSlotState == CharacterSlotState.Active)
                {
                    unit.SetControlButtonState(ControlButtonState.Empty);
                }
                else
                {
                    unit.SetControlButtonState(ControlButtonState.Add);
                }
            }
            else if (activeUnitsCount >= 2)
            {
                if (unit.CharacterSlotState == CharacterSlotState.Active)
                {
                    unit.SetControlButtonState(ControlButtonState.Remove);
                }
                else
                {
                    unit.SetControlButtonState(ControlButtonState.Add);
                }
            }
        }
    }

    public List<Unit> GetGameConfiguration()
    {
        Func<AIModel> aiModelProvider = () => new RandomAIModel(false);
        var units = new List<Unit>();

        foreach (var team1Unit in team1CharacterSlots)
        {
            if(team1Unit.CharacterSlotState != CharacterSlotState.Active)
            {
                continue;
            }

            switch (team1Unit.SelectedType)
            {
                case BaseCharacterType.None:
                    break;
                case BaseCharacterType.Scholar:
                    var scholarUnit = UnitHelper.CreateUnit<Scholar>(0, aiModelProvider?.Invoke());
                    units.Add(scholarUnit);
                    break;
                case BaseCharacterType.Squire:
                    var squireUnit = UnitHelper.CreateUnit<Squire>(0, aiModelProvider?.Invoke());
                    units.Add(squireUnit);
                    break;
                case BaseCharacterType.Hunter:
                    var hunterUnit = UnitHelper.CreateUnit<Hunter>(0, aiModelProvider?.Invoke());
                    units.Add(hunterUnit);
                    break;
            }
        }

        foreach (var team2Unit in team2CharacterSlots)
        {
            if (team2Unit.CharacterSlotState != CharacterSlotState.Active)
            {
                continue;
            }

            switch (team2Unit.SelectedType)
            {
                case BaseCharacterType.None:
                    break;
                case BaseCharacterType.Scholar:
                    var scholarUnit = UnitHelper.CreateUnit<Scholar>(1, aiModelProvider?.Invoke());
                    units.Add(scholarUnit);
                    break;
                case BaseCharacterType.Squire:
                    var squireUnit = UnitHelper.CreateUnit<Squire>(1, aiModelProvider?.Invoke());
                    units.Add(squireUnit);
                    break;
                case BaseCharacterType.Hunter:
                    var hunterUnit = UnitHelper.CreateUnit<Hunter>(1, aiModelProvider?.Invoke());
                    units.Add(hunterUnit);
                    break;
            }
        }

        return units;
    }
}