using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraControllerState
{
    Overview = 0,
    Slot1 = 1,
    Slot2 = 2,
    Slot3 = 3,
}

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _cameraHandle;
    [SerializeField] private Transform _camera;

    private Tween _currentMoveTween;
    private Tween _currentRotateTween;

    public void MoveCameraHandle(Vector3 targetPos)
    {
        if(_currentMoveTween != null)
        {
            _currentMoveTween.Kill();
        }

        targetPos.x = targetPos.x + 45;
        targetPos.z = targetPos.z - 115;

        _currentMoveTween = _cameraHandle.DOLocalMove(targetPos, 0.5f);
    }

    public void RotateCamera(Vector3 to)
    {
        if (_currentRotateTween != null)
        {
            _currentRotateTween.Kill();
        }

        _currentRotateTween = _cameraHandle.DOLocalRotate(to, 0.5f);
    }

    public void SetCameraState(CameraControllerState state)
    {
        switch (state)
        {
            case CameraControllerState.Overview:
                MoveCameraHandle(new Vector3(7.73f, 5.27f, 0f));
                RotateCamera(new Vector3(30f, -90f, 0f));
                break;
            case CameraControllerState.Slot1:
                MoveCameraHandle(new Vector3(6.87f, 3.84f, 0f));
                RotateCamera(new Vector3(24.87f, -90f, 0f));
                break;
            case CameraControllerState.Slot2:
                MoveCameraHandle(new Vector3(6.87f, 3.84f +2.21f, 3.25f - 0.31f));
                RotateCamera(new Vector3(24.87f + 14f, -120.5f, 0f));
                break;
            case CameraControllerState.Slot3:
                MoveCameraHandle(new Vector3(6.87f, 3.84f + 2.21f, -3.5f + 0.31f));
                RotateCamera(new Vector3(24.87f + 14f, -67.7f, 0f));
                break;
        }
    }
}
