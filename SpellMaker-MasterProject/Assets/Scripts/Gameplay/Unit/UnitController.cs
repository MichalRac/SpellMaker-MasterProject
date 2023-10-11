using SMUBE.Units;
using SMUBE.Units.CharacterTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    private Unit _unit;
    public Unit Unit => _unit;

    [SerializeField] private UnitAnimationHandler _unitAnimationHandler;
    public UnitAnimationHandler UnitAnimationHandler => _unitAnimationHandler;

    [SerializeField] private UnitAnimationEventHandler _unitAnimationEventHandler;
    public UnitAnimationEventHandler UnitAnimationEvemtHandler => _unitAnimationEventHandler;

    private Vector3 _initPosition;
    public Vector3 InitPosition => _initPosition;

    [SerializeField] private GameObject HunterModel;
    [SerializeField] private GameObject ScholarModel;
    [SerializeField] private GameObject SquireModel;

    [SerializeField] private Transform modelHandle;

    [SerializeField] private List<SkinnedMeshRenderer> meshRenderers;
    [SerializeField] private Material altMaterial;

    [SerializeField] private UnitEffectController _unitEffectController;
    public UnitEffectController UnitEffectController => _unitEffectController;

    [SerializeField] private InfoPanel _infoPanel;

    public int SlotId { get; private set; }

    public void Setup(Unit unit, Vector3 initPosition, int slotId)
    {
        _unit = unit;
        _initPosition = initPosition;
        SlotId = slotId;

        switch (unit.UnitData.UnitStats.BaseCharacter)
        {
            case Hunter _:
                HunterModel.SetActive(true);
                break;
            case Scholar _:
                ScholarModel.SetActive(true);
                break;
            case Squire _:
                SquireModel.SetActive(true);
                break;
            default:
                break;
        }

        var isAlly = unit.UnitData.UnitIdentifier.TeamId == 0;
        _unitEffectController.Setup(isAlly);

        if (!isAlly)
        {
            foreach (var meshRenderer in meshRenderers)
            {
                meshRenderer.material = altMaterial;
            }
        }

        UpdateInfoPanel();
    }

    public void UpdateInfoPanel()
    {
        _infoPanel.UpdateInfoPanel(this);
    }

    public void UpdatePosition()
    {
        modelHandle.position = InitPosition;
    }
}
