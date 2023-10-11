using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MenuMode
{
    None = 0,

    Title = 1,
    GameConfiguration = 2,
    Ingame = 3,
}

public class MenuRoot : MonoBehaviour
{
    [SerializeField] private TitlePage _titlePage;
    [SerializeField] private GameConfiguration _gameConfiguration;
    [SerializeField] private BattleService _battleService;
    [SerializeField] private ActionPicker _actionPicker;

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
                break;
            case MenuMode.Title:
                _titlePage.gameObject.SetActive(true);
                _gameConfiguration.gameObject.SetActive(false);
                _actionPicker.gameObject.SetActive(false);
                break;
            case MenuMode.GameConfiguration:
                _titlePage.gameObject.SetActive(false);
                _gameConfiguration.gameObject.SetActive(true);
                _gameConfiguration.BeginSetup();
                _actionPicker.gameObject.SetActive(false);
                break;
            case MenuMode.Ingame:
                gameObject.SetActive(false);
                _titlePage.gameObject.SetActive(false);
                _gameConfiguration.gameObject.SetActive(false);
                _actionPicker.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void SwitchToTitle()
    {
        SwitchToMenuMode(MenuMode.Title);
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
}
