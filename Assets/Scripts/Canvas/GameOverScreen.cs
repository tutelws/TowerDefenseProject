using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScreen : MonoBehaviour {
    [SerializeField]
    private string _menuSceneName;
    private float _oldTimeScale;

    private void Awake()
    {
        _oldTimeScale = Time.timeScale;
        Time.timeScale = 0.1f;
    }
    public void OnMainMenuClick()
    {
        Time.timeScale = _oldTimeScale;
        UnityEngine.SceneManagement.SceneManager.LoadScene(_menuSceneName);
    }

}
