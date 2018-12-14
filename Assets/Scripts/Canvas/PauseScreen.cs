using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScreen : MonoBehaviour {
    [SerializeField]
    private string _menuSceneName;
    private float _oldTimeScale;
    
    private void OnEnable()
    {
        _oldTimeScale = Time.timeScale;
        Time.timeScale = 0.1f;
    }
    private void OnDisable()
    {
        Time.timeScale = _oldTimeScale;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.SetActive(false);
        }
    }
    public void OnButtonPlayClick()
    {
        Time.timeScale = _oldTimeScale;
        gameObject.SetActive(false);
    }
    public void OnMenuButtonClick()
    {
        Time.timeScale = _oldTimeScale;
        UnityEngine.SceneManagement.SceneManager.LoadScene(_menuSceneName);
    }
}
