using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionPickEntry : MonoBehaviour
{
    [SerializeField] private Button _btn;
    [SerializeField] private TMP_Text _text;

    public void Setup(Action onClicked, string label)
    {
        _btn.onClick.AddListener(() => onClicked.Invoke());
        _text.text = label;
    }
}
