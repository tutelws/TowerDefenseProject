using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Shrine : MonoBehaviour {
    
    private int _shrineIndex;

    public GameObject ShootingAreaPrefab;
    public Sprite ShrineShortcut;
    public int ShrineIndex { get { return _shrineIndex; } }
    public float ShootRadius { get { return GetComponent<SphereCollider>().radius; } }
    public Action<Shrine> OnShrineDestroy;
    
    private void OnDestroy()
    {
        if (OnShrineDestroy != null)
            OnShrineDestroy(this);
    }
    public void Init(int shrineIndex, Vector3 spawnPos)
    {
        _shrineIndex = shrineIndex;
        transform.position = spawnPos;
    }

}
