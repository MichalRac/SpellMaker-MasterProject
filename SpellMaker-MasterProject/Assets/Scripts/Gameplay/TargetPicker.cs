using Commands;
using Commands.SpecificCommands._Common;
using SMUBE.BattleState;
using SMUBE.Commands.SpecificCommands._Common;
using SMUBE.DataStructures.Units;
using SMUBE.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class TargetPicker
{
    public BattleStateModel BattleStateModel { get; }
    public List<UnitController> AllUnits { get; }
    private List<UnitController> ViableUnits { get; set; }

    private Action<Unit> OnUnitPicked;

    private InputActions _inputActions;

    private AutoResetEvent targetPickedAutoResetEvent;
    private int currentIndex = 0;
    private bool pointerSelection = false;
 
    public TargetPicker(BattleStateModel battleStateModel, List<UnitController> units)
    {
        BattleStateModel = battleStateModel;
        AllUnits = units;

        _inputActions = new InputActions();
        _inputActions.MainMap.PreviousAndNext.performed += Input_PreviousAndNext_performed;
        _inputActions.MainMap.Confirm.performed += Input_Confirm_performed;
        _inputActions.MainMap.Mouse.performed += Input_Mouse_performed;
    }

    public async Task<CommandArgs> GetCommandArgs(Unit activeUnit, CommandArgsValidator validator)
    {
        switch (validator)
        {
            case OneToZeroArgsValidator:
                return new CommonArgs(activeUnit.UnitData, null, BattleStateModel);
            case OneToOneArgsValidator:
                var unit = await PickEnemyTarget(activeUnit.UnitData.UnitIdentifier.TeamId, AllUnits);
                return new CommonArgs(activeUnit.UnitData, new List<UnitData>() { unit.UnitData }, BattleStateModel) ;
            default:
                return null;
        }
    }

    public async Task<Unit> PickEnemyTarget(int activeUnitTeamId, List<UnitController> allUnits)
    {
        ViableUnits = allUnits.FindAll(u => u.Unit.UnitData.UnitStats.CurrentHealth > 0 && u.Unit.UnitData.UnitIdentifier.TeamId != activeUnitTeamId);

        _inputActions.Enable();
        PointerSystem.PointerUpdate += PointerSystem_PointerUpdate;


        targetPickedAutoResetEvent = new AutoResetEvent(false);

        if(currentIndex >= ViableUnits.Count) 
        { 
            currentIndex = ViableUnits.Count - 1; 
        }

        ViableUnits[currentIndex].UnitEffectController.SetPointed(true);
        ViableUnits[currentIndex].UnitEffectController.SetPointedHighlight(true);

        await Task.Run(() => { targetPickedAutoResetEvent.WaitOne(); });

        _inputActions.Disable();
        PointerSystem.PointerUpdate -= PointerSystem_PointerUpdate;

        ViableUnits[currentIndex].UnitEffectController.SetPointed(false);

        return ViableUnits[currentIndex].Unit;
    }


    private void Input_PreviousAndNext_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        var isNext = obj.ReadValue<float>() > 0;

        ViableUnits[currentIndex].UnitEffectController.SetPointed(false);

        if(isNext) 
        {
            currentIndex++;

            if (currentIndex > ViableUnits.Count - 1)
            {
                currentIndex = 0;
            }
        }
        else
        {
            currentIndex--;

            if (currentIndex < 0)
            {
                currentIndex = ViableUnits.Count - 1;
            }
        }

        pointerSelection = false;
        ViableUnits[currentIndex].UnitEffectController.SetPointed(true);
        ViableUnits[currentIndex].UnitEffectController.SetPointedHighlight(true);
    }

    private void Input_Confirm_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        targetPickedAutoResetEvent.Set();
    }

    private void PointerSystem_PointerUpdate(Vector2 obj)
    {
        Ray ray = Camera.main.ScreenPointToRay(obj);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit))
        {
            var uc = hit.transform.GetComponent<UnitController>();

            if (uc != null)
            {

                int newIndex = -1;
                for (int i = 0; i < ViableUnits.Count; i++)
                {
                    if (ViableUnits[i].Unit.UnitData.UnitIdentifier == uc.Unit.UnitData.UnitIdentifier)
                    {
                        newIndex = i;
                        break;
                    }
                }

                if(newIndex != -1 && currentIndex != newIndex)
                {
                    pointerSelection = true;
                    ViableUnits[currentIndex].UnitEffectController.SetPointed(false);
                    currentIndex = newIndex;
                    ViableUnits[currentIndex].UnitEffectController.SetPointed(true);
                }

                ViableUnits[currentIndex].UnitEffectController.SetPointedHighlight(true);
                return;
            }
        }

        if(pointerSelection == true)
        {
            ViableUnits[currentIndex].UnitEffectController.SetPointedHighlight(false);
        }
    }


    private void Input_Mouse_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        var currentMousePos = PointerSystem.CurrentPos;

        Ray ray = Camera.main.ScreenPointToRay(currentMousePos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            var uc = hit.transform.GetComponent<UnitController>();

            if (uc != null)
            {
                int newIndex = -1;
                for (int i = 0; i < ViableUnits.Count; i++)
                {
                    if (ViableUnits[i].Unit.UnitData.UnitIdentifier == uc.Unit.UnitData.UnitIdentifier)
                    {
                        newIndex = i;
                        break;
                    }
                }

                if(newIndex != -1)
                {
                    ViableUnits[currentIndex].UnitEffectController.SetPointed(false);

                    currentIndex = newIndex;

                    targetPickedAutoResetEvent.Set();

                }
            }
        }
    }

}
