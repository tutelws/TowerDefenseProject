using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {
    [SerializeField]
    private GameObject _shootingPoint;
    [SerializeField]
    private GameObject _projectilePrefab;
    [SerializeField]
    private float _fireRateTimer;
    [SerializeField]
    private float _projectileSpeed;
    [SerializeField]
    private int _projectileLayer;
    [SerializeField]
    private AudioClip _attackSound;

    private AudioSource _audioSource;
    private Vector3 _targetPos;
    private float _fireTimer;

    public Vector3 TargetPos { set { _targetPos = value; } }

    private void OnEnable()
    {
        _fireTimer = _fireRateTimer;
    }
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        _fireTimer -= Time.deltaTime;
        if (_fireTimer <= 0)
        {
            _fireTimer += _fireRateTimer;
            Shoot();
        }
    }

    private void Shoot()
    {
        if (_audioSource != null && _attackSound != null)
        {
            _audioSource.PlayOneShot(_attackSound);
        }
        Vector3 shootPos;
        Quaternion shootRot;
        if (_shootingPoint == null)
        {
            shootPos = gameObject.transform.position;
            shootRot = gameObject.transform.rotation;
        }
        else
        {
            shootPos = _shootingPoint.transform.position;
            shootRot = _shootingPoint.transform.rotation;
        }
        transform.LookAt(_targetPos);
        var go = Instantiate(_projectilePrefab);
        go.transform.position = shootPos;
        go.transform.rotation = shootRot;
        var projectile = go.GetComponent<IProjectile>();
        projectile.Init(_projectileLayer, _projectileSpeed);
    }
}
