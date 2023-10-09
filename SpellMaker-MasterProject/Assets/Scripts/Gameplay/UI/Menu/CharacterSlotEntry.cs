using SMUBE.Units.CharacterTypes;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum CharacterSlotState
{
    Empty = 0,
    Active = 1,
}
public enum ControlButtonState
{
    Empty = 0,
    Add = 1,
    Remove = 2,
}


public class CharacterSlotEntry : MonoBehaviour
{
    private readonly Color inactiveColor = new Color(0.83f, 0.83f, 0.83f);
    private readonly Color activeAllyColor = new Color(0.42f, 0.905f, 0.365f);
    private readonly Color activeEnemyColor = new Color(0.97f, 0.46f, 0.35f);

    [SerializeField] private GameObject _emptyContent;
    [SerializeField] private GameObject _activeContent;

    [SerializeField] private Image _squireUnitTypeImage;
    [SerializeField] private Image _hunterUnitTypeImage;
    [SerializeField] private Image _scholarUnitTypeImage;

    [SerializeField] private GameObject _addButtonGroup;
    [SerializeField] private Button _addUnitButton;
    [SerializeField] private Button _altAddUnitButton;

    [SerializeField] private GameObject _removeButtonGroup;
    [SerializeField] private Button _removeUnitButton;
    [SerializeField] private Button _altRemoveUnitButton;

    [SerializeField] private TextMeshProUGUI _selectedLabel;
    [SerializeField] private TextMeshProUGUI _selectedSubLabel;

    public int SlotId { get; private set; }
    public BaseCharacterType SelectedType { get; private set; }
    private Action<int> onUnitRemoved;
    private Action<int> onUnitAdded;
    private bool _altColorScheme;

    public CharacterSlotState CharacterSlotState { get; private set; }

    public void Setup(int slotId, Action<int> argOnTypeRemoved, Action<int> argOnUnitAdded, 
        CharacterSlotState initSlotState = CharacterSlotState.Empty, bool altColorScheme = false)
    {
        SlotId = slotId;
        CharacterSlotState = initSlotState;
        _altColorScheme = altColorScheme;

        if(_altColorScheme)
        {
            _selectedLabel.color = activeEnemyColor;
            _selectedSubLabel.color = activeEnemyColor;
        }

        _addUnitButton.gameObject.SetActive(!_altColorScheme);
        _altAddUnitButton.gameObject.SetActive(_altColorScheme);

        _removeUnitButton.gameObject.SetActive(!_altColorScheme);
        _altRemoveUnitButton.gameObject.SetActive(_altColorScheme);


        SetAllImagesInactive();
        if (initSlotState == CharacterSlotState.Active)
        {
            OnSquireSelected();
        }

        onUnitRemoved = argOnTypeRemoved;
        onUnitAdded = argOnUnitAdded;

        SetState(initSlotState);
        PreselectUnitType();
    }

    public void SetState(CharacterSlotState targetState)
    {
        CharacterSlotState = targetState;

        switch (targetState)
        {
            case CharacterSlotState.Empty:
                _emptyContent.gameObject.SetActive(true);
                _activeContent.gameObject.SetActive(false);
                break;
            case CharacterSlotState.Active:
                _emptyContent.gameObject.SetActive(false);
                _activeContent.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void SetControlButtonState(ControlButtonState controlButtonState)
    {
        switch (controlButtonState)
        {
            case ControlButtonState.Empty:
                _addButtonGroup.gameObject.SetActive(false);
                _removeButtonGroup.gameObject.SetActive(false);
                break;
            case ControlButtonState.Add:
                _addButtonGroup.gameObject.SetActive(true);
                _removeButtonGroup.gameObject.SetActive(false);
                break;
            case ControlButtonState.Remove:
                _addButtonGroup.gameObject.SetActive(false);
                _removeButtonGroup.gameObject.SetActive(true);
                break;
        }

    }

    public void OnAddButton()
    {
        onUnitAdded?.Invoke(SlotId);
    }

    public void Remove()
    {
        onUnitRemoved?.Invoke(SlotId);
    }

    public void PreselectUnitType()
    {
        switch (SlotId)
        {
            default:
            case 0:
                OnSquireSelected();
                break;
            case 1:
                OnHunterSelected();
                break;
            case 2:
                OnScholarSelected();
                break;
        }
    }

    public void OnSquireSelected()
    {
        SetAllImagesInactive();
        _squireUnitTypeImage.color = _altColorScheme ? activeEnemyColor : activeAllyColor;
        _selectedLabel.text = "Squire";
        _selectedSubLabel.text = "Defensive Unit";
        SelectedType = BaseCharacterType.Squire;
    }    
    
    public void OnHunterSelected()
    {
        SetAllImagesInactive();
        _hunterUnitTypeImage.color = _altColorScheme ? activeEnemyColor : activeAllyColor;
        _selectedLabel.text = "Hunter";
        _selectedSubLabel.text = "Offensive Unit";
        SelectedType = BaseCharacterType.Hunter;
    }

    public void OnScholarSelected()
    {
        SetAllImagesInactive();
        _scholarUnitTypeImage.color = _altColorScheme ? activeEnemyColor : activeAllyColor;
        _selectedLabel.text = "Scholar";
        _selectedSubLabel.text = "Support Unit";
        SelectedType = BaseCharacterType.Scholar;
    }

    private void SetAllImagesInactive()
    {
        _squireUnitTypeImage.color = inactiveColor;
        _hunterUnitTypeImage.color = inactiveColor;
        _scholarUnitTypeImage.color = inactiveColor;
    }
}
