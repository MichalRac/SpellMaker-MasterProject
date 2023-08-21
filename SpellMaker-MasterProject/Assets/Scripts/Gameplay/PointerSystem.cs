using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerSystem : MonoBehaviour
{
    private InputActions _inputActions;


    public static Vector2 CurrentPos;
    public static event Action<Vector2> PointerUpdate;

    private void Awake()
    {
        _inputActions = new InputActions();
        _inputActions.Enable();
    }

    private void Update()
    {

        var newPos = _inputActions.MainMap.PointerPos.ReadValue<Vector2>();

        if(newPos != CurrentPos)
        {
            CurrentPos = newPos;
            PointerUpdate?.Invoke(CurrentPos);
        }
    }
}
