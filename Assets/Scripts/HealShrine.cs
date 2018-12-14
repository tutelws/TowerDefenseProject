using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealShrine : MonoBehaviour {

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
        DamagableTower DT = other.GetComponent<DamagableTower>();
        if (DT != null)
        {
            _targets.Add(other.gameObject);
        }
    }
    private void SetRandomCurrTarget()
    {
        if (_targets != null && _targets.Count != 0)
            RandomNotDestroyed(_targets.FindAll((t) => !t.GetComponent<DamagableTower>().Destroyed && t.GetComponent<DamagableTower>().IsDamaged));
    }
    private void RandomNotDestroyed(List<GameObject> notDestroyedTargets)
    {
        if (notDestroyedTargets != null && notDestroyedTargets.Count != 0)
            _currTarget = notDestroyedTargets[UnityEngine.Random.Range(0, notDestroyedTargets.Count)];
        else
            _currTarget = null;
    }
    private void Shoot()
    {
        SetRandomCurrTarget();
        if (_currTarget == null)
            return;
        GameObject go = Instantiate(_autoProjectilePrefab);
        go.transform.position = _shootingPoint.position;
        AutoProjectile projectile = go.GetComponent<AutoProjectile>();
        projectile.Init(_projectileLayer, _projectileSpeed, _currTarget.transform);

    }
}
