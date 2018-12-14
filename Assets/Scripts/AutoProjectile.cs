using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoProjectile : MonoBehaviour, IProjectile {
    [SerializeField]
    private float _projectileRotationSpeed;
    [SerializeField]
    private float _lifeTime;
    [SerializeField]
    private AudioClip _flySound;

    private AudioSource _audioSource;
    private float _currLifeTime;
    private Transform _targetTransform;
    private Rigidbody _rigidbody;
    private float _speed = 0f;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _rigidbody = GetComponent<Rigidbody>();
        _currLifeTime = _lifeTime;
        if (_audioSource == null)
            return;
        _audioSource.loop = true;
        _audioSource.clip = _flySound;
        _audioSource.Play();
    }
    private void OnDestroy()
    {
        if (_audioSource != null)
            _audioSource.Stop();
    }
    private void Update()
    {
        _currLifeTime -= Time.deltaTime;
        if (_currLifeTime <= 0 || _targetTransform == null)
        {
            Destroy(gameObject);
            return;
        }
        var targetRotation = Quaternion.LookRotation(_targetTransform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _projectileRotationSpeed * Time.deltaTime);
        _rigidbody.velocity = transform.forward * _speed;
    }
    public void Init(int layer, float speed, Transform targetTransform)
    {
        var colliders = GetComponentsInChildren<Collider>();
        for (int i = 0; i < colliders.Length; ++i)
            colliders[i].gameObject.layer = layer;
        _targetTransform = targetTransform;
        transform.LookAt(_targetTransform);
        _speed = speed;
    }
}
