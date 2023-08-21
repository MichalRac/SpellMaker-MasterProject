using Assets.Scripts.Gameplay.Unit;
using Commands;
using SMUBE.AI;
using SMUBE.Commands.SpecificCommands._Common;
using SMUBE.Core;
using SMUBE.Units;
using SMUBE.Units.CharacterTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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

    private List<UnitController> _unitControllers = new();

    private TargetPicker _targetPicker;
    private StatelessCommandAnimationRunner _commandAnimationRunner;

    private Unit _activeUnit;

    private void Awake()
    {
        _commandAnimationRunner = new StatelessCommandAnimationRunner();
    }

    // Start is called before the first frame update
    async void Start()
    {
        InitializeGame();
        
        while(!_battleCore.currentStateModel.IsFinished(out var _))
        {
            await RunTurn();
        }

        _battleCore.currentStateModel.IsFinished(out var winnerTeam);
        Debug.Log($"Team {winnerTeam} won!");
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


        var units = new List<Unit>()
        {
            unit1team1,
            unit2team1,
            unit3team1,

            unit1team2,
            unit2team2,
            unit3team2,
        };

        _battleCore = new BattleCore(units);

        SpawnUnits(unit1team1, unit2team1, unit3team1, unit1team2, unit2team2, unit3team2);
        _targetPicker = new TargetPicker(_battleCore.currentStateModel, _unitControllers);
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

            newUnitController.Setup(newUnit, newUnitController.transform.position);

            newUnitController.UnitAnimationHandler.RotateTowards(NavigationHelper.GetUnitAdjacentPosition(newUnitController));

            _unitControllers.Add(newUnitController);
        }
    }

    private async Task RunTurn()
    {
        if(_battleCore.currentStateModel.IsFinished(out var winnerTeamId))
        {
            Debug.Log($"Game finished, winner team: {winnerTeamId}");
            return;
        }

        _battleCore.currentStateModel.GetNextActiveUnit(out var nextActiveUnit);

        _activeUnit = nextActiveUnit;

        var isPlayerControlled = nextActiveUnit.UnitData.UnitIdentifier.TeamId == PLAYER_TEAM_ID;

        var activeUnitController = _unitControllers.Find((uc) => uc.Unit.UnitData.UnitIdentifier == _activeUnit.UnitData.UnitIdentifier);

        activeUnitController.UnitEffectController.SetSelected(true);

        if (isPlayerControlled)
        {
            await ProcessPlayerTurn(nextActiveUnit);
        }
        else
        {
            await ProcessCPUTurn(nextActiveUnit);
        }

        activeUnitController.UnitEffectController.SetSelected(false);

        activeUnitController.UpdatePosition();

        await HandlePersistentEffects();

        foreach (var uc in _unitControllers)
        {
            uc.UpdateInfoPanel();
        }
    }

    private async Task ProcessPlayerTurn(Unit nextActiveUnit)
    {
        var options = nextActiveUnit.ViableCommands;

        var command = await _actionPicker.SetupActionPicker(options);
        await OnActionPicked(command);
    }

    private async Task ProcessCPUTurn(Unit nextActiveUnit)
    {
        var command = nextActiveUnit.AiModel.ResolveNextCommand(_battleCore.currentStateModel, nextActiveUnit.UnitData.UnitIdentifier);
        var commandArg = nextActiveUnit.AiModel.GetCommandArgs(command, _battleCore.currentStateModel, nextActiveUnit.UnitData.UnitIdentifier);

        await _commandAnimationRunner.PerformActionVisual(_unitControllers, command, commandArg);
        _battleCore.currentStateModel.ExecuteCommand(command, commandArg);
    }

    private async Task OnActionPicked(ICommand command)
    {
        var commandArgs = await GetCommandArgs(_activeUnit, command.CommandArgsValidator);
        await _commandAnimationRunner.PerformActionVisual(_unitControllers, command, commandArgs);

        _battleCore.currentStateModel.ExecuteCommand(command, commandArgs);
    }

    private async Task<CommandArgs> GetCommandArgs(Unit activeUnit, CommandArgsValidator commandArgsValidator)
    {
        return await _targetPicker.GetCommandArgs(activeUnit, commandArgsValidator);
    }

    private async Task HandlePersistentEffects()
    {
        foreach (var uc in _unitControllers)
        {
            if(uc.Unit.UnitData.UnitStats.CurrentHealth <= 0)
            {
                _commandAnimationRunner.ApplyDeath(uc);
                return;
            }

            await _commandAnimationRunner.PerformEffectVisual(uc);
        }
    }
}
