using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Unit
{
    public static class NavigationHelper
    {
        public static Vector3 GetUnitAdjacentPosition(UnitController unitController)
        {
            var isPlayer = unitController.Unit.UnitData.UnitIdentifier.TeamId == 0;

            var pos = unitController.InitPosition;
            pos.x = isPlayer ? pos.x - 1 : pos.x + 1;
            return pos;
        }

    }
}
