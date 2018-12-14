using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ProtectShrine : MonoBehaviour {
    [SerializeField]
    private GameObject _autoProjectilePrefab;
    [SerializeField]
    private int _projectileLayer;
    [SerializeField]
    private float _fireRate;
    [SerializeField]
    private float _projectileSpeed;
    [SerializeField]
    private Transform _shootingPoint;

    private bool isAttack = false;
    private GameObject _currTarget;
    private List<GameObject> _targets;
    private float _fireTimer;

    private void Awake()
    {
        _targets = new List<GameObject>();
        _fireTimer = _fireRate;
    }
    private void Update()
    {
        _fireTimer -= Time.deltaTime;
        if (_fireTimer <= 0)
        {
            _fireTimer += _fireRate;

            Shoot();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        DamagableEnemy DE = other.GetComponent<DamagableEnemy>();
        if (DE != null)
        {
            _targets.Add(other.gameObject);
            DE.DieEvent += OnEnemyDie;
            if (!isAttack)
                _currTarget = GetNearestTarget();
        }
    }
    private void OnTriggerExit(Collider other)
    {
            
        DamagableEnemy DE = other.GetComponent<DamagableEnemy>();
        if (DE!= null)
        {
            _targets.Remove(other.gameObject);
            if (DE.IsAttacked)
            {
                _currTarget = GetNearestTarget();
                DE.IsAttacked = false;
            }
            
        }
    }
    private GameObject GetNearestTarget()
    {
        if (gameObject == null)
            return null;
        float minDist = float.MaxValue;
        GameObject nearest = null;
        foreach (GameObject t in _targets)
        {
            float dist = Vector3.Distance(transform.position, t.transform.position);
            if (dist < minDist)
            {
                nearest = t;
                minDist = dist;
            }
        }
        return nearest;
    }
    private void OnEnemyDie(DamagableEnemy de)
    {
        if (this == null)
            return;
        _targets.Remove(de.gameObject);
        _currTarget = GetNearestTarget();
        isAttack = false;
        de.IsAttacked = false;
    }
    private void Shoot()
    {
        if (_currTarget == null)
            return;
        isAttack = true;
        _currTarget.GetComponent<DamagableEnemy>().IsAttacked = true;
        GameObject go = Instantiate(_autoProjectilePrefab);
        go.transform.position = _shootingPoint.position;
        AutoProjectile projectile = go.GetComponent<AutoProjectile>();
        projectile.Init(_projectileLayer, _projectileSpeed, _currTarget.transform);
        
    }
    private void OnDestroy()
    {
        if (_targets != null)
            foreach (GameObject t in _targets)
                if (t != null)
                    t.GetComponent<DamagableEnemy>().DieEvent -= OnEnemyDie;

        
    }
   
}
