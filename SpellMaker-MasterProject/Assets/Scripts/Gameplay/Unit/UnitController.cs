using SMUBE.Units;
using SMUBE.Units.CharacterTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    private Unit _unit;

    [SerializeField] private GameObject HunterModel;
    [SerializeField] private GameObject ScholarModel;
    [SerializeField] private GameObject SquireModel;

    [SerializeField] private Transform modelHandle;

    [SerializeField] private List<SkinnedMeshRenderer> meshRenderers;
    [SerializeField] private Material altMaterial;

    public void Setup(Unit unit)
    {
        _unit = unit;

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

        if(unit.UnitData.UnitIdentifier.TeamId != 0)
        {
            foreach (var meshRenderer in meshRenderers)
            {
                meshRenderer.material = altMaterial;
            }

            modelHandle.transform.Rotate(new Vector3(0, 180, 0));
        }
    }
}
