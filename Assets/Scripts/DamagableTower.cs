using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class DamagableTower : MonoBehaviour {
    [SerializeField]
    private GameObject _castleModel;
    [SerializeField]
    private GameObject _flags;
    [SerializeField]
    private GameObject _flagsCrushed;
    [SerializeField]
    private float _maxHealth;
    [SerializeField]
    private GameObject _fadePointsPrefab;
    [SerializeField]
    private float _healthBarScale;
    [SerializeField]
    private Vector3 _healthBarOffset;
    [SerializeField]
    private GameObject _healthBarPrefab;
    [SerializeField]
    private Color _pointsColor;
    [SerializeField]
    private Vector3 _pointsOffset;
    [SerializeField]
    private float _maxRandomPointsPos;
    [SerializeField]
    private float _maxDamagedTransformScale;
    [SerializeField]
    private float _scaleSequenceDuration;
    [SerializeField]
    private AudioClip[] _damageSounds;

    private float _currHealth;
    private AudioSource _audioSource;
    private bool _isDamaged;
    private TowerHealthBar _towerHealthBar;
    private Vector3 _oldScale;

    public float MaxHealth { get { return _maxHealth; } }
    public float CurrHealth { get { return _currHealth; } }
    public bool IsDamaged { get { return _isDamaged; } }
    public GameObject CastleModel { get { return _castleModel; } }
    public bool Destroyed;
    public Action<DamagableTower> DamageEvent;
    public Action<DamagableTower> DieEvent;
    private void Awake()
    {
        _isDamaged = false;
        _currHealth = _maxHealth;
        Destroyed = false;
        _oldScale = _castleModel.transform.localScale;
        _audioSource = GetComponent<AudioSource>();
        //SpawnHealthBar();

    }
    private void OnTriggerEnter(Collider other)
    {
        DamageProvider dp = other.GetComponentInParent<DamageProvider>();
       
        if (dp != null)
        {
            _currHealth -= dp.Damage;
            if (_currHealth > _maxHealth)
                _currHealth = _maxHealth;
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
        _isDamaged = _currHealth < _maxHealth;

    }
    private void Damaged(float damage)
    {
        if (_audioSource != null)
            _audioSource.PlayOneShot(_damageSounds[UnityEngine.Random.Range(0, _damageSounds.Length)]);
        _castleModel.transform.DOKill(true);
        _castleModel.transform.localScale = _oldScale;
        
        Sequence seq = DOTween.Sequence();
        seq.Append(_castleModel.transform.DOScale(_maxDamagedTransformScale, _scaleSequenceDuration / 2f))
            .Append(_castleModel.transform.DOScale(_oldScale, _scaleSequenceDuration / 2f))
            .InsertCallback(0, () =>
            {
                ShowDamage(damage);
            });
    }
    private void Died(float damage)
    {
        _currHealth = 0;
        Destroyed = true;
        GetComponent<Collider>().enabled = false;
        _flags.SetActive(false);
        ShowDamage(damage);
        _flagsCrushed.SetActive(true);
    }
    public void ShowDamage(float damage)
    {
        GameObject p = Instantiate(_fadePointsPrefab, CanvasManager.Instance.MainCanvas.transform);
        FadePoints fp = p.GetComponent<FadePoints>();
        fp.TextColor = _pointsColor;
        fp.Points = damage;
        fp.MaxRandom = _maxRandomPointsPos;
        fp.TargetTransform = gameObject.transform;
        fp.Offset = _pointsOffset;

        if (_towerHealthBar == null)
            SpawnHealthBar();
        _towerHealthBar.Damaged(_currHealth);
    }
    private void SpawnHealthBar()
    {
        GameObject bar = Instantiate(_healthBarPrefab, CanvasManager.Instance.MainCanvas.transform);
        TowerHealthBar tbar = bar.GetComponent<TowerHealthBar>();
        tbar.Init(_maxHealth, gameObject.transform, false, _healthBarOffset, _healthBarScale);
        _towerHealthBar = tbar;
    }
    
   

}
