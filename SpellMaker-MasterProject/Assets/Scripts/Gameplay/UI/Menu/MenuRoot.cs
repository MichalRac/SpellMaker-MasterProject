using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MenuMode
{
    None = 0,

    Title = 1,
    GameConfiguration = 2,
}

public class MenuRoot : MonoBehaviour
{
    [SerializeField] private TitlePage _titlePage;
    [SerializeField] private GameConfiguration _gameConfiguration;
    [SerializeField] private BattleService _battleService;

    public void SwitchToMenuMode(MenuMode menuMode)
    {
        switch (menuMode)
        {
            case MenuMode.None:
                gameObject.SetActive(false);
                _titlePage.gameObject.SetActive(false);
                _gameConfiguration.gameObject.SetActive(false);
                break;
            case MenuMode.Title:
                _titlePage.gameObject.SetActive(true);
                _gameConfiguration.gameObject.SetActive(false);
                break;
            case MenuMode.GameConfiguration:
                _titlePage.gameObject.SetActive(false);
                _gameConfiguration.gameObject.SetActive(true);
                _gameConfiguration.BeginSetup();
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
        SwitchToMenuMode(MenuMode.None);
        var units = _gameConfiguration.GetGameConfiguration();
        _battleService.BeginGame(units);
    }
}
