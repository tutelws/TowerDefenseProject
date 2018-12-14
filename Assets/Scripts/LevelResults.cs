using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Level Results", menuName = "Scriptable Object/New Level Results")]
public class LevelResults : ScriptableObject{
    [System.Serializable]
    public class LevelResult
    {
        public string SceneName;
        public int StarsCount;
    }
    public List<LevelResult> LevelResultsList;

}
