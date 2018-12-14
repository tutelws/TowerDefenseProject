using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {
    [SerializeField]
    private Animation _moveAnimation;
    [SerializeField]
    private float _moveSpeed = 20;
    [SerializeField]
    private GameObject _gunGO;
    //[SerializeField]
    //private float _shootingDistance;
    [SerializeField]
    private float _stoppingDistance;
    [SerializeField]
    private float _shootingRotationSpeed = 14f;
    [SerializeField]
    private GameObject _currTower;
    [SerializeField]
    private AudioClip _walkSound;


    private AudioSource _audioSource;
    private NavMeshAgent _navAgent;
    private bool _targetSetted = false;
    
    private void Awake()
    {
        _navAgent = GetComponent<NavMeshAgent>();
        _navAgent.speed = _moveSpeed;
        _gunGO.SetActive(false);
        _audioSource = GetComponent<AudioSource>();
        //if (_currTower != null) SetNewTarget(_currTower);
    }
    private void Start()
    {
    
        if (_moveAnimation != null)
            _moveAnimation.Play();

    }
    private void Update()
    {  
        if (_walkSound != null && _navAgent.speed != 0 && !_audioSource.isPlaying)
        {
            _audioSource.clip = _walkSound;
            _audioSource.loop = true;
            _audioSource.Play();   
        }
        float dist = Vector3.Distance(_currTower.transform.position, transform.position);
        if (dist <= _stoppingDistance)
            NavStop();
        else if (!_targetSetted)
        {
            SetNewTarget(GameManager.Instance.GetNearestTower(gameObject.transform.position));
            
        }
            
    }
    private void NavStop()
    {
        _targetSetted = false;
        _audioSource.Stop();
        var targetRotation = Quaternion.LookRotation(_currTower.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _shootingRotationSpeed * Time.deltaTime);
        _navAgent.speed = 0;
        if (_moveAnimation != null)
            _moveAnimation.Stop();
        _gunGO.SetActive(true);
    }
    private void OnDestroyTarget(DamagableTower damagableTower)
    {
        SetNewTarget(GameManager.Instance.GetNearestTower(gameObject.transform.position));
    }
    public void SetNewTarget(GameObject tower = null)
    {
        _targetSetted = true;
        if (_currTower != null)
            _currTower.GetComponent<DamagableTower>().DieEvent -= OnDestroyTarget;
        if (tower == null)
            _currTower = GameManager.Instance.MainBase;
        else
        {
            _currTower = tower;
            _currTower.GetComponent<DamagableTower>().DieEvent += OnDestroyTarget;
        }
        _navAgent.SetDestination(_currTower.transform.position);

        Bounds b = _currTower.GetComponent<DamagableTower>().CastleModel.GetComponent<MeshRenderer>().bounds;
        _stoppingDistance += b.extents.x;
        _navAgent.stoppingDistance = _stoppingDistance;
        _navAgent.speed = _moveSpeed;
        _gunGO.GetComponent<Gun>().TargetPos = _currTower.transform.position;
        _gunGO.SetActive(false);
        if (_moveAnimation != null)
            _moveAnimation.Play();

    }
    private void OnDestroy()
    {
        if (_currTower != null)
            _currTower.GetComponent<DamagableTower>().DieEvent -= OnDestroyTarget;
    }
}
