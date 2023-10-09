using Commands;
using Commands.SpecificCommands._Common;
using Commands.SpecificCommands.BaseAttack;
using JetBrains.Annotations;
using SMUBE.Commands.Effects;
using SMUBE.Commands.SpecificCommands.BaseBlock;
using SMUBE.Commands.SpecificCommands.DefendAll;
using SMUBE.Commands.SpecificCommands.HealAll;
using SMUBE.Commands.SpecificCommands.HeavyAttack;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Unit
{
    public class StatelessCommandAnimationRunner
    {
        public void ApplyDeath(UnitController unitController)
        {
            unitController.UnitAnimationHandler.SetAnimBool(GlobalAnimationParameters.DEAD, true);
        }

        public async Task PerformEffectVisual(UnitController unitController)
        {
            if (unitController == null)
            {
                Debug.LogError("Incorrect input for command animation runner - PerformEffectVisual");
                return;
            }

            var blockActive = false;

            foreach (var effect in unitController.Unit.UnitData.UnitStats.PersistentEffects)
            {
                switch(effect)
                {
                    case BlockEffect:
                        blockActive = true;
                        BaseBlockAnimation(unitController, true);
                        break;
                    default:
                        break;
                }
            }

            if (!blockActive)
            {
                BaseBlockAnimation(unitController, false);
            }
        }

        public async Task PerformActionVisual(List<UnitController> unitControllers, ICommand command, CommandArgs commandArgs)
        {
            if (unitControllers == null || unitControllers.Count == 0 || command == null || commandArgs == null)
            {
                Debug.LogError("Incorrect input for command animation runner - PerformActionVisual");
                return;
            }

            switch(command)
            {
                case HeavyAttack:
                    await HeavyAttackSequence(unitControllers, command, commandArgs);
                    break;
                case BaseAttack:
                    await BaseAttackSequence(unitControllers, command, commandArgs);
                    break;
                case DefendAll:
                    await DefendAllSequence(unitControllers, command, commandArgs);
                    break;
                case HealAll:
                    HealAllSequence(unitControllers, command, commandArgs);
                    break;
                case BaseBlock:
                    break;
                default:
                    Debug.LogError($"Not implemented command type {command.GetType().Name}");
                    break;
            }
        }

        public async Task BaseAttackSequence(List<UnitController> unitControllers, ICommand command, CommandArgs commandArgs)
        {
            var unitController = unitControllers.Find(uc => uc.Unit.UnitData.UnitIdentifier == commandArgs.ActiveUnit.UnitIdentifier);
            var targetUnitController = unitControllers.Find(uc => uc.Unit.UnitData.UnitIdentifier == commandArgs.TargetUnits[0].UnitIdentifier);

            var initPos = unitController.transform.position;

            await unitController.UnitAnimationHandler.WalkTo(NavigationHelper.GetUnitAdjacentPosition(targetUnitController));
            await unitController.UnitAnimationHandler.RotateTowards(targetUnitController.transform.position);


            async void on_hit_callback()
            {
                unitController.UnitAnimationEvemtHandler.Hit -= on_hit_callback;
                targetUnitController.UnitAnimationHandler.PlaySingleAnim(GlobalAnimationParameters.HIT);
            }
            unitController.UnitAnimationEvemtHandler.Hit += on_hit_callback;

            await unitController.UnitAnimationHandler.PlaySingleAnim(GlobalAnimationParameters.BASE_ATTACK, GlobalAnimationParameters.BASE_ATTACK_DURATION);

            await unitController.UnitAnimationHandler.WalkTo(unitController.InitPosition);
            await unitController.UnitAnimationHandler.RotateTowards(NavigationHelper.GetUnitAdjacentPosition(unitController));
        }

        private async Task DefendAllSequence(List<UnitController> unitControllers, ICommand command, CommandArgs commandArgs)
        {
            var unitController = unitControllers.Find(uc => uc.Unit.UnitData.UnitIdentifier == commandArgs.ActiveUnit.UnitIdentifier);
            List<UnitController> targetUnits = new();

            foreach (var targetUnit in commandArgs.TargetUnits)
            {
                var targetUnitController = unitControllers.Find(uc => uc.Unit.UnitData.UnitIdentifier == targetUnit.UnitIdentifier);
                targetUnits.Add(targetUnitController);

                targetUnitController.UnitEffectController.PlayDefendVFX();
            }
        }

        private async Task HealAllSequence(List<UnitController> unitControllers, ICommand command, CommandArgs commandArgs)
        {
            var unitController = unitControllers.Find(uc => uc.Unit.UnitData.UnitIdentifier == commandArgs.ActiveUnit.UnitIdentifier);
            List<UnitController> targetUnits = new();

            foreach (var targetUnit in commandArgs.TargetUnits)
            {
                var targetUnitController = unitControllers.Find(uc => uc.Unit.UnitData.UnitIdentifier == targetUnit.UnitIdentifier);
                targetUnits.Add(targetUnitController);

                targetUnitController.UnitEffectController.PlayHealVFX();
            }
        }

        public async Task HeavyAttackSequence(List<UnitController> unitControllers, ICommand command, CommandArgs commandArgs)
        {
            var unitController = unitControllers.Find(uc => uc.Unit.UnitData.UnitIdentifier == commandArgs.ActiveUnit.UnitIdentifier);
            var targetUnitController = unitControllers.Find(uc => uc.Unit.UnitData.UnitIdentifier == commandArgs.TargetUnits[0].UnitIdentifier);

            var initPos = unitController.transform.position;

            await unitController.UnitAnimationHandler.WalkTo(NavigationHelper.GetUnitAdjacentPosition(targetUnitController));
            await unitController.UnitAnimationHandler.RotateTowards(targetUnitController.transform.position);


            async void on_hit_callback()
            {
                unitController.UnitAnimationEvemtHandler.Hit -= on_hit_callback;
                targetUnitController.UnitAnimationHandler.PlaySingleAnim(GlobalAnimationParameters.HIT);
                targetUnitController.UnitEffectController.PlayHeavyDmgVFX();
            }
            unitController.UnitAnimationEvemtHandler.Hit += on_hit_callback;

            await unitController.UnitAnimationHandler.PlaySingleAnim(GlobalAnimationParameters.BASE_ATTACK, GlobalAnimationParameters.BASE_ATTACK_DURATION);

            await unitController.UnitAnimationHandler.WalkTo(unitController.InitPosition);
            await unitController.UnitAnimationHandler.RotateTowards(NavigationHelper.GetUnitAdjacentPosition(unitController));
        }



        public void BaseBlockAnimation(UnitController unitController, bool value)
        {
            unitController.UnitAnimationHandler.SetAnimBool(GlobalAnimationParameters.BASE_BLOCK_IDLE, value);
        }

    }
}
