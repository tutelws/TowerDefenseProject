using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrineBlock : MonoBehaviour {
    [SerializeField]
    private GameObject _activeModel;
    [SerializeField]
    private GameObject _unActiveModel;
    [SerializeField]
    private GameObject _protectShrinesSelectionPrefab;
    [SerializeField]
    private Transform _spawnPosTransform;
    [SerializeField]
    private Vector3 _selectionPosOffset;

    public GameObject SelectedShrine;
    private GameObject _shrineSelection;
    
    private void Awake()
    {
        Deactivate();
    }
    private void OnMouseEnter()
    {
        Activate();
    }
    private void OnMouseExit()
    {
        Deactivate();
    }
    private void OnMouseDown()
    {
        if (_shrineSelection != null)
            return;
        Activate();
        _shrineSelection = Instantiate(_protectShrinesSelectionPrefab, CanvasManager.Instance.MainCanvas.transform);
        _shrineSelection.GetComponent<ProtectShrinesSelection>().Init(this, _spawnPosTransform.position, transform.position, _selectionPosOffset);
    }
    private void Activate()
    {
        _activeModel.SetActive(true);
        _unActiveModel.SetActive(false);
        if (SelectedShrine != null)
            GameManager.Instance.SpawnShrineShootingAreaByIndex(SelectedShrine.GetComponent<Shrine>().ShrineIndex, SelectedShrine.transform.position);
    }
    private void Deactivate()
    {
        _activeModel.SetActive(false);
        _unActiveModel.SetActive(true);
        if (SelectedShrine != null)
            GameManager.Instance.DestroyShrineShootingArea();
    }
}

