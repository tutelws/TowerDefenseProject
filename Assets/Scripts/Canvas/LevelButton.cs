using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour {
    public Image[] stars;
    public string SceneName;
    public bool UnLocked;

    public void Init(string sceneName, bool unLocked, int starsCount)
    {
        for (int i = 0; i < starsCount; i++)
            stars[i].color = new Color(255, 255, 255, 255);
        UnLocked = unLocked;
        GetComponentInChildren<Text>().text = sceneName;
        SceneName = sceneName;
        //GetComponent<Button>().onClick.AddListener(OnButtonClick);
    }
    public void OnButtonClick()
    {
        //if (!UnLocked)
        //    return;
        UnityEngine.SceneManagement.SceneManager.LoadScene(SceneName);
    }
}
