using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IProjectile {
    [SerializeField]
    private float _lifeTime;
    
    private Rigidbody _rigidbody;
    private float _currLifeTime;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _currLifeTime = _lifeTime; 
    }
    private void Update()
    {
        _currLifeTime -= Time.deltaTime;
        if (_currLifeTime <= 0)
            Destroy(gameObject);
    }
    public void Init(int layer, float speed, Transform targetTransform = null)
    {
        var colliders = GetComponentsInChildren<Collider>();
        for (int i = 0; i < colliders.Length; ++i)
        {
            colliders[i].gameObject.layer = layer;
        }
        var dir = transform.forward;
        _rigidbody.velocity = dir * speed;
    }
 



}
