using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionButton : MonoBehaviour {

    public Image Shortcut;
    public Text ButtonText;
    public int ShrinePrefabIndex;
    public Vector3 SpawnPos;
    public GameObject ParentGO;
    public void OnClickButton()
    {
        ProtectShrinesSelection pss = ParentGO.GetComponent<ProtectShrinesSelection>();
        if (ShrinePrefabIndex == -1)
        {
            pss.DeleteSelectedShrine();
            Destroy(ParentGO);
            return;
        }
        GameObject spawnedShrine = GameManager.Instance.SpawnShrineByIndex(ShrinePrefabIndex, SpawnPos);
        pss.DeleteSelectedShrine();
        pss.SetSelectedShrine(spawnedShrine);
        GameManager.Instance.DestroyShrineShootingArea();
        Destroy(ParentGO);
    }
    public void OnPointerEnter()
    {
        if (ShrinePrefabIndex == -1)
            return;
        GameManager.Instance.SpawnShrineShootingAreaByIndex(ShrinePrefabIndex, SpawnPos);
    }
    public void OnPointerExit()
    {
        if (ShrinePrefabIndex == -1)
            return;
        GameManager.Instance.DestroyShrineShootingArea();
    }
    public void Init(GameObject parentGO)
    {
        ShrinePrefabIndex = -1;
        ParentGO = parentGO;
        ButtonText.text = "Nothing";
    }
    public void Init(int protectShrinePrefabIndex, Vector3 spawnPos, GameObject parentGO)
    {
        ParentGO = parentGO;
        ShrinePrefabIndex = protectShrinePrefabIndex;
        SpawnPos = spawnPos;
        GameObject shrine = GameManager.Instance.CurrPlayerProtectShrines[protectShrinePrefabIndex].ProtectShrinePrefab;
        Shortcut.sprite = shrine.GetComponent<Shrine>().ShrineShortcut;
        ButtonText.text = shrine.name + " [" + GameManager.Instance.CurrPlayerProtectShrines[protectShrinePrefabIndex].Count + "]";
    }

}
