using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text id;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TMP_Text healthText;

    [SerializeField] private Slider staminaSlider;
    [SerializeField] private TMP_Text staminaText;
    
    [SerializeField] private Slider manaSlider;
    [SerializeField] private TMP_Text manaText;

    public void UpdateInfoPanel(UnitController unitController)
    {
        id.text = unitController.Unit.UnitData.Name;

        var stats = unitController.Unit.UnitData.UnitStats;

        if(stats.CurrentHealth <= 0)
        {
            gameObject.SetActive(false);
        }

        healthSlider.normalizedValue = (float)stats.CurrentHealth / stats.MaxHealth;
        healthText.text = $"{stats.CurrentHealth} / {stats.MaxHealth}";

        if(stats.MaxStamina == 0)
        {
            staminaSlider.gameObject.SetActive(false);
        }
        else
        {
            staminaSlider.normalizedValue = (float)stats.CurrentStamina / stats.MaxStamina;
            staminaText.text = $"{stats.CurrentStamina} / {stats.MaxStamina}";
        }

        if(stats.MaxMana == 0)
        {
            manaSlider.gameObject.SetActive(false);
        }
        else
        {
            manaSlider.normalizedValue = (float)stats.CurrentMana / stats.MaxMana;
            manaText.text = $"{stats.CurrentMana} / {stats.MaxMana}";
        }
    }
}
