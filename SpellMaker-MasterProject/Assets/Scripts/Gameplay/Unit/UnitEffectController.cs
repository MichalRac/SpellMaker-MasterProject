using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitEffectController : MonoBehaviour
{
    [SerializeField] private GameObject _pointer;
    [SerializeField] private MeshRenderer _pointerMeshRenderer;
    [SerializeField] private Material _poinerAllyMaterial;
    [SerializeField] private Material _pointerEnemyMaterial;

    [SerializeField] private GameObject _selector;
    [SerializeField] private MeshRenderer _selectorMeshRenderer;
    [SerializeField] private Material _allyMaterial;
    [SerializeField] private Material _enemyMaterial;

    private Material _defaultMaterial;
    private bool _isAlly;

    public void Setup(bool isAlly)
    {
        _selectorMeshRenderer.material = isAlly ? _allyMaterial : _enemyMaterial;

        _isAlly = isAlly;
        _defaultMaterial = _pointerMeshRenderer.material;
    }

    public void SetPointed(bool value)
    {
        _pointer.SetActive(value);
    }

    public void SetPointedHighlight(bool value)
    {
        _pointerMeshRenderer.material = value
            ? _isAlly
                ? _poinerAllyMaterial
                : _pointerEnemyMaterial
            : _defaultMaterial;
    }

    public void SetSelected(bool value)
    {
        _selector.SetActive(value);
    }
}
