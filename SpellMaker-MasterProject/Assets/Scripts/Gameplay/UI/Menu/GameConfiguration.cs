using Mono.Cecil;
using SMUBE.AI;
using SMUBE.AI.BehaviorTree;
using SMUBE.AI.DecisionTree;
using SMUBE.AI.GoalOrientedBehavior;
using SMUBE.AI.StateMachine;
using SMUBE.Units;
using SMUBE.Units.CharacterTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum AISetup
{
    Random = 0,
    DT = 1,
    FSA = 2,
    BT = 3,
    GOB = 4,
}

public class GameConfiguration : MonoBehaviour
{
    [SerializeField] private List<CharacterSlotEntry> team1CharacterSlots;
    [SerializeField] private List<CharacterSlotEntry> team2CharacterSlots;
    [SerializeField] private TextMeshProUGUI _selectedAILabel;

    private AISetup _selectedAISetup = AISetup.DT;

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

        UpdateSelectedAILabel();
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

    public void SelectNextAI()
    {
        if (_selectedAISetup == AISetup.GOB)
        {
            _selectedAISetup = AISetup.Random;
        }
        else
        {
            _selectedAISetup++;
        }
        UpdateSelectedAILabel();
    }

    public void SelectPreviousAI()
    {
        if (_selectedAISetup == AISetup.Random)
        {
            _selectedAISetup = AISetup.GOB;
        }
        else
        {
            _selectedAISetup--;
        }
        UpdateSelectedAILabel();
    }

    private void UpdateSelectedAILabel()
    {
        switch (_selectedAISetup)
        {
            case AISetup.Random:
                _selectedAILabel.text = "Random AI";
                break;
            case AISetup.DT:
                _selectedAILabel.text = "Decision Tree AI";
                break;
            case AISetup.FSA:
                _selectedAILabel.text = "Finite State Automata AI";
                break;
            case AISetup.BT:
                _selectedAILabel.text = "Behavior Tree AI";
                break;
            case AISetup.GOB:
                _selectedAILabel.text = "Goal Oriented Behavior AI";
                break;
        }
    }

    public List<Unit> GetGameConfiguration()
    {
        Func<AIModel> aiModelProvider = null;

        switch (_selectedAISetup)
        {
            case AISetup.Random:
                aiModelProvider = () => new RandomAIModel(false);
                break;
            case AISetup.DT:
                aiModelProvider = () => new DecisionTreeAIModel(false);
                break;
            case AISetup.FSA:
                aiModelProvider = () => new StateMachineAIModel(null, false);
                break;
            case AISetup.BT:
                aiModelProvider = () => new BehaviorTreeAIModel(false);
                break;
            case AISetup.GOB:
                aiModelProvider = () => new GoalOrientedBehaviorAIModel(false);
                break;
        }

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