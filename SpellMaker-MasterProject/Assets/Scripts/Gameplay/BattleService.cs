using Commands;
using SMUBE.AI;
using SMUBE.Commands.SpecificCommands._Common;
using SMUBE.Core;
using SMUBE.Units;
using SMUBE.Units.CharacterTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleService : MonoBehaviour
{
    private const int PLAYER_TEAM_ID = 0;
    private const int CPU_TEAM_ID = 1;

    private BattleCore _battleCore;

    [SerializeField] private ActionPicker _actionPicker;
    [SerializeField] private UnitController unitControllerPrefab;
    [SerializeField] private SpawnPointProvider spawnPointProvider;
    [SerializeField] private Transform _unitContainer;

    // Start is called before the first frame update
    void Start()
    {
        InitializeGame();
        RunTurn();
    }

    private void InitializeGame()
    {
        var aiModel = new RandomAIModel();

        var unit1team1 = UnitHelper.CreateUnit<Squire>(PLAYER_TEAM_ID, aiModel);
        var unit2team1 = UnitHelper.CreateUnit<Hunter>(PLAYER_TEAM_ID, aiModel);
        var unit3team1 = UnitHelper.CreateUnit<Scholar>(PLAYER_TEAM_ID, aiModel);

        var unit1team2 = UnitHelper.CreateUnit<Squire>(CPU_TEAM_ID, aiModel);
        var unit2team2 = UnitHelper.CreateUnit<Hunter>(CPU_TEAM_ID, aiModel);
        var unit3team2 = UnitHelper.CreateUnit<Scholar>(CPU_TEAM_ID, aiModel);


        _battleCore = new BattleCore(new List<Unit>()
        {
            unit1team1,
            unit2team1,
            unit3team1,

            unit1team2,
            unit2team2,
            unit3team2,
        });

        SpawnUnits(unit1team1, unit2team1, unit3team1, unit1team2, unit2team2, unit3team2);
    }

    private void SpawnUnits(params Unit[] newUnits)
    {
        int team1SpawnCount = 0;
        int team2SpawnCount = 0;

        var team1SpawnPoints = spawnPointProvider.GetTeamSpawnPoints(0);
        var team2SpawnPoints = spawnPointProvider.GetTeamSpawnPoints(1);

        foreach (var newUnit in newUnits)
        {
            var teamId = newUnit.UnitData.UnitIdentifier.TeamId;
            var newUnitController = Instantiate(unitControllerPrefab, _unitContainer);
            newUnitController.transform.position = teamId == 0 
                ? team1SpawnPoints[team1SpawnCount++].transform.position 
                : team2SpawnPoints[team2SpawnCount++].transform.position;

            newUnitController.Setup(newUnit);
        }
    }

    private void RunTurn()
    {
        if(_battleCore.currentStateModel.IsFinished(out var winnerTeamId))
        {
            Debug.Log($"Game finished, winner team: {winnerTeamId}");
            return;
        }

        _battleCore.currentStateModel.GetNextActiveUnit(out var nextActiveUnit);

        var isPlayerControlled = nextActiveUnit.UnitData.UnitIdentifier.TeamId == PLAYER_TEAM_ID;

        if(isPlayerControlled)
        {
            ProcessPlayerTurn(nextActiveUnit);
        }
        else
        {
            ProcessPlayerTurn(nextActiveUnit);
        }
    }

    private void ProcessPlayerTurn(Unit nextActiveUnit)
    {
        var options = nextActiveUnit.ViableCommands;

        _actionPicker.SetupActionPicker(options, OnActionPicked);
    }

    private void ProcessCPUTurn(Unit nextActiveUnit)
    {
        nextActiveUnit.AiModel.ResolveNextCommand(_battleCore.currentStateModel, nextActiveUnit.UnitData.UnitIdentifier);
    }

    private void OnActionPicked(ICommand command)
    {
    }

    private CommandArgs GetCommandArgs(CommandArgsValidator command)
    {
        switch (command)
        {
            case OneToZeroArgsValidator:
                break;
            case OneToOneArgsValidator:
                break;
            default:
                break;
        }

        return null;
    }
}
