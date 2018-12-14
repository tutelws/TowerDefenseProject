using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectShrinesSelection : MonoBehaviour {

    [SerializeField]
    private GameObject SelectionButtonPrefab;
    [SerializeField]
    private float _lerpValue;

    private ShrineBlock _shrineBlock;
    private Vector3 _targetPos;

    private void Update()
    {
        transform.localPosition = Vector3.Lerp(
            transform.localPosition, 
            CanvasManager.Instance.WorldToCanvas(_targetPos),
            _lerpValue * Time.deltaTime
            );
    }
    public void Init(ShrineBlock shrineBlock, Vector3 spawnPos, Vector3 targetPos, Vector3 offset)
    {
        _shrineBlock = shrineBlock;
        LevelConfig.PlayerProtectShrine[] shrines = GameManager.Instance.CurrPlayerProtectShrines;
        for (int i = 0; i < shrines.Length; i++)
        {
            if (shrines[i].Count <= 0)
                continue;
            GameObject button = Instantiate(SelectionButtonPrefab, transform);
            button.GetComponent<SelectionButton>().Init(i, spawnPos, gameObject);
        }
        GameObject nullButton = Instantiate(SelectionButtonPrefab, transform);
        nullButton.GetComponent<SelectionButton>().Init(gameObject);
        //call 1 variant of .Init method if we want create a null shrine button;
        transform.localPosition = CanvasManager.Instance.WorldToCanvas(targetPos);
        _targetPos = targetPos + offset;
    }
    public void SetSelectedShrine(GameObject selectedShrine)
    {
        _shrineBlock.SelectedShrine = selectedShrine;
    }
    public void DeleteSelectedShrine()
    {
        Destroy(_shrineBlock.SelectedShrine);
    }
}
