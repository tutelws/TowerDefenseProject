using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level Config", menuName = "Scriptable Object/New Level Config")]
public class LevelConfig : ScriptableObject {
    [System.Serializable]
    public struct PlayerProtectShrine
    {
        public int Count;
        public GameObject ProtectShrinePrefab;
    }
	[System.Serializable]
    public struct EnemyWave
    {
        public GameObject[] EnemyPrefabs;
        public int[] EnemyTypeCount;
        public int SpawnCount;
        public float SpawnDelay;
        public float SpawnDelayDowner;
    }
    
    public bool WaitForEnemiesKill;
    public PlayerProtectShrine[] playerProtectShrines;
    public EnemyWave[] waves;
}
