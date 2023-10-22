using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameSummary : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _message;

    public void ShowSummary(bool victory)
    {
        _message.text = victory
            ? "You won!"
            : "You lost!";

        gameObject.SetActive(true);
    }
}
