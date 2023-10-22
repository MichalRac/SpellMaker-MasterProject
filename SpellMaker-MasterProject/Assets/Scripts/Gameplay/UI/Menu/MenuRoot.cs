using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MenuMode
{
    None = 0,

    Title = 1,
    GameConfiguration = 2,
    Ingame = 3,
    Info = 4,
}

public class MenuRoot : MonoBehaviour
{
    [SerializeField] private TitlePage _titlePage;
    [SerializeField] private GameConfiguration _gameConfiguration;
    [SerializeField] private BattleService _battleService;
    [SerializeField] private ActionPicker _actionPicker;
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private GameObject _infoRoot;

    private void Awake()
    {
        SwitchToTitle();
    }

    public void SwitchToMenuMode(MenuMode menuMode)
    {
        switch (menuMode)
        {
            case MenuMode.None:
                gameObject.SetActive(false);
                _titlePage.gameObject.SetActive(false);
                _gameConfiguration.gameObject.SetActive(false);
                _actionPicker.gameObject.SetActive(false);
                _infoRoot.SetActive(false);
                break;
            case MenuMode.Title:
                gameObject.SetActive(true);
                _titlePage.gameObject.SetActive(true);
                _gameConfiguration.gameObject.SetActive(false);
                _actionPicker.gameObject.SetActive(false);
                _infoRoot.SetActive(false);
                break;
            case MenuMode.GameConfiguration:
                gameObject.SetActive(true);
                _titlePage.gameObject.SetActive(false);
                _gameConfiguration.gameObject.SetActive(true);
                _gameConfiguration.BeginSetup();
                _actionPicker.gameObject.SetActive(false);
                _infoRoot.SetActive(false);
                break;
            case MenuMode.Ingame:
                gameObject.SetActive(false);
                _titlePage.gameObject.SetActive(false);
                _gameConfiguration.gameObject.SetActive(false);
                _actionPicker.gameObject.SetActive(true);
                _infoRoot.SetActive(false);
                break;
            case MenuMode.Info:
                gameObject.SetActive(true);
                _titlePage.gameObject.SetActive(false);
                _gameConfiguration.gameObject.SetActive(false);
                _actionPicker.gameObject.SetActive(false);
                _infoRoot.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void SwitchToTitle()
    {
        SwitchToMenuMode(MenuMode.Title);
        _cameraController.SetCameraState(CameraControllerState.Title);
    }

    public void SwitchToInfo()
    {
        SwitchToMenuMode(MenuMode.Info);
    }

    public void SwitchToGameConfiguration()
    {
        SwitchToMenuMode(MenuMode.GameConfiguration);
    }

    public void SwitchToBattle()
    {
        SwitchToMenuMode(MenuMode.Ingame);
        var units = _gameConfiguration.GetGameConfiguration();
        _battleService.BeginGame(units);
    }
    public void Quit()
    {
        Application.Quit();
    }

}
