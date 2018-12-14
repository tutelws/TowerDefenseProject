using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using DG.Tweening;

public class DamagableEnemy : MonoBehaviour {
    [SerializeField]
    private GameObject _enemyModel;
    [SerializeField]
    private float _maxHealth;
    [SerializeField]
    private float _healthBarScale;
    [SerializeField]
    private Vector3 _healthBarOffset;
    [SerializeField]
    private GameObject _healthBarPrefab;
    [SerializeField]
    private float _maxDamagedTransformScale;
    [SerializeField]
    private float _scaleSequenceDuration;
    [SerializeField]
    private AudioClip _damageSound;

    private AudioSource _audioSource;
    private float _currHealth;
    private TowerHealthBar _healthBar;
    private Vector3 _oldScale;

    public bool IsAttacked;
    public bool Destroyed;
    public Action<DamagableEnemy> DamageEvent;
    public Action<DamagableEnemy> DieEvent;

    private void Awake()
    {
        _currHealth = _maxHealth;
        _oldScale = _enemyModel.transform.localScale;
        _audioSource = GetComponents<AudioSource>()[1];
        IsAttacked = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        DamageProvider dp = other.GetComponentInParent<DamageProvider>();
        if (dp != null)
        {
            _currHealth -= dp.Damage;
            if (_currHealth <= 0)
            {
                Died(dp.Damage);
                if (DieEvent != null)
                    DieEvent(this);
            }
            else
            {
                Damaged(dp.Damage);
                if (DamageEvent != null)
                    DamageEvent(this);
            }
            Destroy(dp.gameObject);
        }
    }
    private void Damaged(float damage)
    {
        _audioSource.PlayOneShot(_damageSound);
        _enemyModel.transform.DOKill(true);
        _enemyModel.transform.localScale = _oldScale;

        Sequence seq = DOTween.Sequence();
        seq.Append(_enemyModel.transform.DOScale(_maxDamagedTransformScale, _scaleSequenceDuration / 2f))
            .Append(_enemyModel.transform.DOScale(_oldScale, _scaleSequenceDuration / 2f))
            .InsertCallback(0, () =>
            {
                ShowDamage(damage);
            });
    }
    private void Died(float damage)
    {
        Destroyed = true;
        GetComponent<Collider>().enabled = false;
        ShowDamage(damage);
        GameManager.Instance.EnemyCount--;
        Destroy(gameObject);
    }
    public void ShowDamage(float damage)
    { 
        if (_healthBar == null)
            SpawnHealthBar();
        _healthBar.Damaged(_currHealth);
    }
    private void SpawnHealthBar()
    {
        GameObject bar = Instantiate(_healthBarPrefab, CanvasManager.Instance.MainCanvas.transform);
        TowerHealthBar hbar = bar.GetComponent<TowerHealthBar>();
        hbar.Init(_maxHealth, gameObject.transform, true, _healthBarOffset, _healthBarScale);
        _healthBar = hbar;
    }
}
