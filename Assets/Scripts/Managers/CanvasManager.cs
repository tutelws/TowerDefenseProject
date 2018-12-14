using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour {
    [SerializeField]
    private GameObject _gameOverScreen;
    [SerializeField]
    private GameObject _winScreen;
    [SerializeField]
    private GameObject _pauseScreen;
    [SerializeField]
    private WaveCounter _waveCounter;
    public Canvas MainCanvas;
    protected CanvasManager() { }
    public static CanvasManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        _gameOverScreen.SetActive(false);
        _winScreen.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !_pauseScreen.activeInHierarchy)
            _pauseScreen.SetActive(true);

    }
    public void SetCounterWave(int currWaveNumber)
    {
        _waveCounter.SetWaveNumber(currWaveNumber);
    }
    public void GameOverScreen()
    {
        _gameOverScreen.SetActive(true);
    }
    public void WinScreen()
    {
        _winScreen.SetActive(true);
    }
    public Vector2 WorldToCanvas(Vector3 worldPosition, Canvas canvas = null, Camera camera = null)
    {
        if (camera == null)
        {
            if (GameManager.Instance.Camera == null)
                GameManager.Instance.Camera = Camera.main;
            camera = GameManager.Instance.Camera;
        }
        if (canvas == null)
        {
            canvas = MainCanvas;
        }

        var viewport_position = camera.WorldToViewportPoint(worldPosition);
        var canvas_rect = canvas.GetComponent<RectTransform>();

        return new Vector2((viewport_position.x * canvas_rect.sizeDelta.x) - (canvas_rect.sizeDelta.x * 0.5f),
            (viewport_position.y * canvas_rect.sizeDelta.y) - (canvas_rect.sizeDelta.y * 0.5f));
    }
}
