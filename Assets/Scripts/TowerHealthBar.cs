using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TowerHealthBar : MonoBehaviour {
    [SerializeField]
    private float _sliderValueEpsilon;
    [SerializeField]
    private float _lerp;
    [SerializeField]
    private Slider _slider;
    [SerializeField]
    private float lerpTime;

    private bool _isMovingTarget;
    private Transform _targetTransform;
    private Vector3 _offset;
    private float _maxHP;

    public bool IsMovingTarget { get { return _isMovingTarget; } }

	private void Start ()
    {
        transform.localPosition = CanvasManager.Instance.WorldToCanvas(_targetTransform.position + _offset);
        _slider.value = 1f;
    }
    private void Update()
    {
        if (_targetTransform != null && (IsMovingTarget || GameManager.Instance.GetRtsCamera.IsMoving))
            transform.localPosition = Vector3.Lerp(
                transform.localPosition, 
                CanvasManager.Instance.WorldToCanvas(_targetTransform.position + _offset), 
                Time.deltaTime * lerpTime);
        if (_slider.value < 0.01f)
            Destroy(gameObject);
    }
    IEnumerator DamageCor(float desiredValue)
    {
        if (desiredValue < 0) desiredValue = 0;
        while (Mathf.Abs(_slider.value - desiredValue) > _sliderValueEpsilon)
        {
            _slider.value = Mathf.Lerp(_slider.value, desiredValue, _lerp * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        _slider.value = desiredValue;
    }
    public void Damaged(float currHealth)
    {
        float desiredValue = currHealth / _maxHP;
        StopAllCoroutines();
        StartCoroutine(DamageCor(desiredValue));
    }
    public void Init(float maxHP, Transform targetTransform, bool isMovingTarget, Vector3 offset, float scale)
    {
        _maxHP = maxHP;
        _isMovingTarget = isMovingTarget;
        _targetTransform = targetTransform;
        _offset = offset;
        _slider.gameObject.transform.localScale = new Vector3(scale, scale, scale);

    }


}
