using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelMenu : MonoBehaviour {
    [SerializeField]
    private GameObject _buttonPrefab;
    [SerializeField]
    private LevelResults _levelResults;

    private string[] _sceneNames;

    private void Awake()
    {
        for (int i = _sceneNames.Length - 1; i >= 0; i--)
        {
            LevelResults.LevelResult lr = _levelResults.LevelResultsList.Find(r => r.SceneName == _sceneNames[i]);
            int starsCount = 0;
            if (lr != null)
                starsCount = lr.StarsCount;
            Instantiate(_buttonPrefab, transform);
            LevelButton lb = _buttonPrefab.GetComponent<LevelButton>();
            LevelResults.LevelResult lrPred = null;
            if (i > 0)
                lrPred = _levelResults.LevelResultsList.Find(r => r.SceneName == _sceneNames[i - 1]);
            bool unLocked = i == 0 || lrPred != null && lrPred.StarsCount > 0;

            lb.Init(_sceneNames[i], unLocked, starsCount);
        }
    }
    public void Init(string[] sceneNames)
    {
        _sceneNames = sceneNames;
    }
}
