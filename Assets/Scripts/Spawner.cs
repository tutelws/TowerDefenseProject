using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    [SerializeField]
    private GameObject _targetTower;
    [SerializeField]
    private float _spawnTimer;

    private int _currWave;
    private int _currSpawnCount;
    private float _currTimer;

    private void Awake()
    {
        _currWave = 0;
        _currSpawnCount = -1;
        _currTimer = 1f;
        _spawnTimer = 1f;
        SpawnUnits();

    }
    private void Update()
    {
        if (_currSpawnCount == 0)
            _currTimer /= 2;
        _currTimer -= Time.deltaTime;
        if (_currTimer <= 0)
        {
            SpawnUnits();
            _currTimer += _spawnTimer;
        }
    }
    private void SpawnUnits()
    {
        if (GameManager.Instance == null)
            return;
        LevelConfig lc = GameManager.Instance.GetLevelConfig;
        if (_currWave == lc.waves.Length)
        {
            GameManager.Instance.IsAllWavesSpawned = true;
            return;
        }  
        if (_currSpawnCount == -1)
        {
            _currSpawnCount = 0;
            _spawnTimer = lc.waves[_currWave].SpawnDelay;
        }
        else
        if (_currSpawnCount == 0)
        {
            CanvasManager.Instance.SetCounterWave(_currWave + 1);
        }
        else
        if (_currSpawnCount == lc.waves[_currWave].SpawnCount)
        {
            if (lc.WaitForEnemiesKill && GameManager.Instance.EnemyCount > 0)
                return;
            _currWave++;
            if (_currWave < lc.waves.Length)
            {
                _currSpawnCount = 0;
                _spawnTimer = lc.waves[_currWave].SpawnDelay;
            }
            return;
        }
        for (int i = 0; i < lc.waves[_currWave].EnemyTypeCount.Length; i++)
            for (int j = 0; j < lc.waves[_currWave].EnemyTypeCount[i]; j++)
            {
                var go = Instantiate(lc.waves[_currWave].EnemyPrefabs[i], gameObject.transform.position, Quaternion.identity);
                Enemy e = go.GetComponent<Enemy>();
                e.transform.localRotation = transform.localRotation;
                e.SetNewTarget(_targetTower.GetComponent<DamagableTower>().Destroyed ?
                    GameManager.Instance.GetNearestTower(transform.position) : _targetTower);
                GameManager.Instance.EnemyCount++;
            }
        _currSpawnCount++;
        _spawnTimer -= lc.waves[_currWave].SpawnDelayDowner;
    }

}
