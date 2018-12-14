using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour 
{
    [System.Serializable]
    public class Chapter
    {
        public string[] sceneNames;
    }
    public Chapter[] Chapters;
    public GameObject MainMenu;
    public GameObject ChaptersMenu;

	public void OnChaptersClick()
	{
        ChaptersMenu.SetActive(true);
        MainMenu.SetActive(false);
	}
    public void OnExitClick()
    {
        Application.Quit();
    }
    
}
