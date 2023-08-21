using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimationEventHandler : MonoBehaviour
{
    public event Action Hit;

    public void OnHit()
    {
        Hit?.Invoke();
    }
}
