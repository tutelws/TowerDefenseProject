using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterMenu : MonoBehaviour {
    
    private MenuController _menuController;
    public int ChapterIndex;
    public GameObject LevelsMenu;
    private void Start()
    {
        _menuController = GetComponentInParent<MenuController>();
    }
    public void OnSpiderChapterClick()
    {
        LevelsMenu.GetComponent<LevelMenu>().Init(_menuController.GetComponent<MenuController>().Chapters[ChapterIndex].sceneNames);
        LevelsMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}
