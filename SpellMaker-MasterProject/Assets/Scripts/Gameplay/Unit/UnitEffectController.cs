using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitEffectController : MonoBehaviour
{
    [SerializeField] private GameObject _pointer;

    [SerializeField] private GameObject _selector;
    [SerializeField] private MeshRenderer _selectorMeshRenderer;
    [SerializeField] private Material _allyMaterial;
    [SerializeField] private Material _enemyMaterial;

    public void Setup(bool isAlly)
    {
        _selectorMeshRenderer.material = isAlly ? _allyMaterial : _enemyMaterial;
    }

    public void SetPointed(bool value)
    {
        _pointer.SetActive(value);
    }
    
    public void SetSelected(bool value)
    {
        _selector.SetActive(value);
    }
}
