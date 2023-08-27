using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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

    [SerializeField] private GameObject _defendVFX;
    [SerializeField] private GameObject _healVFX;
    [SerializeField] private GameObject _heavyDmgVFX;

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

    public async void PlayDefendVFX()
    {
        _defendVFX.SetActive(true);
        await Task.Delay(1000);
        _defendVFX.SetActive(false);
    }

    public async void PlayHealVFX()
    {
        _healVFX.SetActive(true);
        await Task.Delay(1000);
        _healVFX.SetActive(false);
    }
    public async void PlayHeavyDmgVFX()
    {
        _heavyDmgVFX.SetActive(true);
        await Task.Delay(250);
        _heavyDmgVFX.SetActive(false);
    }
}
