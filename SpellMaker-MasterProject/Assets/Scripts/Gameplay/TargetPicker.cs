using Commands;
using Commands.SpecificCommands._Common;
using SMUBE.BattleState;
using SMUBE.Commands.SpecificCommands._Common;
using SMUBE.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TargetPicker
{
    public BattleStateModel BattleStateModel { get; }
    public List<Unit> Units { get; }
 
    public TargetPicker(BattleStateModel battleStateModel, List<Unit> units)
    {
        BattleStateModel = battleStateModel;
        Units = units;
    }

    public async void GetCommandArgs(Unit activeUnit, CommandArgsValidator validator, Action<CommandArgs> onTargetPicked)
    {
        switch (validator)
        {
            case OneToZeroArgsValidator:
                onTargetPicked?.Invoke(new CommonArgs(activeUnit.UnitData, null, BattleStateModel));
                return;
            case OneToOneArgsValidator:

                break;
            default:
                break;
        }
    }

    public async Task<Unit> PickTarget()
    {


        return null;
    }

}
