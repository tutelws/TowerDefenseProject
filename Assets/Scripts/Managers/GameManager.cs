using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private LevelResults _levelResults;
    [SerializeField]
    private LevelConfig _levelConfig;
    [SerializeField]
    private GameObject[] _towers;
    [SerializeField]
    private GameObject _mainBase;

    private RtsCamera _rtsCamera;
    private int _currLevelResultIndex;
    private GameObject _shrineShootingArea;
    public bool IsAllWavesSpawned;
    public int EnemyCount;
    public Canvas MainCanvas;
    public Camera Camera;

    protected GameManager() { }
    public static GameManager Instance { get; private set; }

    public RtsCamera GetRtsCamera { get { return _rtsCamera; } }
    public LevelConfig GetLevelConfig { get { return _levelConfig; } }
    public GameObject[] Towers { get { return _towers; } }
    public GameObject MainBase { get { return _mainBase; } }
    public LevelConfig.PlayerProtectShrine[] CurrPlayerProtectShrines;

    private void Awake()
    {
        Instance = this;
        string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        _currLevelResultIndex = _levelResults.LevelResultsList.FindIndex(r => r.SceneName == sceneName);
        if (_currLevelResultIndex == -1)
            _levelResults.LevelResultsList.Add(new LevelResults.LevelResult
            {
                SceneName = sceneName,
                StarsCount = 0
            });

        _rtsCamera = Camera.GetComponent<RtsCamera>();
        EnemyCount = 0;
        IsAllWavesSpawned = false;
        if (Camera == null) Camera = Camera.main;
        if (_mainBase != null)
            _mainBase.GetComponent<DamagableTower>().DieEvent += OnBaseDie;
        CurrPlayerProtectShrines = (LevelConfig.PlayerProtectShrine[])_levelConfig.playerProtectShrines.Clone();
    }
    private void Update()
    {
        if (IsAllWavesSpawned && EnemyCount <= 0)
            GameWin();
    }
    private void CalculateLevelResults()
    {
        float hpSum = 0;
        float hpMax = 0;
        for (int i = 0; i < _towers.Length; i++)
        {
            DamagableTower da = _towers[i].GetComponent<DamagableTower>();
            hpSum += da.CurrHealth;
            hpMax += da.MaxHealth;
        }
        DamagableTower dab = _mainBase.GetComponent<DamagableTower>();
        hpSum += dab.CurrHealth;
        hpMax += dab.MaxHealth;

        float part = hpSum / hpMax;
        int starsCount = part < 0.33f ? 1 : part > 0.66f ? 3 : 2;
        if (_levelResults.LevelResultsList[_currLevelResultIndex].StarsCount < starsCount)
            _levelResults.LevelResultsList[_currLevelResultIndex].StarsCount = starsCount;
    }
    private void GameWin()
    {
        CalculateLevelResults();
        CanvasManager.Instance.WinScreen();
    }
    private void OnBaseDie(DamagableTower damagableTower)
    {
        CanvasManager.Instance.GameOverScreen();
    }

    private void OnProtectShrineDestroy(Shrine shrine)
    {
        CurrPlayerProtectShrines[shrine.ShrineIndex].Count++;
    }
    public GameObject SpawnShrineByIndex(int shrineIndex, Vector3 spawnPos)
    {
        if (CurrPlayerProtectShrines[shrineIndex].Count <= 0)
            return null;
        CurrPlayerProtectShrines[shrineIndex].Count--;
        GameObject shrine = Instantiate(CurrPlayerProtectShrines[shrineIndex].ProtectShrinePrefab);
        Shrine ps = shrine.GetComponent<Shrine>();
        ps.Init(shrineIndex, spawnPos);
        ps.OnShrineDestroy += OnProtectShrineDestroy;
        return shrine;
    }
    public void SpawnShrineShootingAreaByIndex(int shrineIndex, Vector3 spawnPos)
    {
        Destroy(_shrineShootingArea);
        GameObject area = Instantiate(CurrPlayerProtectShrines[shrineIndex].ProtectShrinePrefab.GetComponent<Shrine>().ShootingAreaPrefab);
        area.transform.position = spawnPos;
        float rad = CurrPlayerProtectShrines[shrineIndex].ProtectShrinePrefab.GetComponent<SphereCollider>().radius;
        area.transform.localScale = new Vector3(rad * 2, rad * 2, rad * 2);
        _shrineShootingArea = area;
    }
    public void DestroyShrineShootingArea()
    {
        if (_shrineShootingArea != null)
            Destroy(_shrineShootingArea);
    }
    public GameObject GetNearestTower(Vector3 pos)
    {
        GameObject tower = null;
        float minDist = float.MaxValue;
        for (int i = 0; i < _towers.Length; i++)
        {
            GameObject t = _towers[i];
            float currDist = Vector3.Distance(t.transform.position, pos);
            if (!t.GetComponent<DamagableTower>().Destroyed && currDist < minDist)
            {
                minDist = currDist;
                tower = t;
            }
        }
        return tower;
    }
    
}
