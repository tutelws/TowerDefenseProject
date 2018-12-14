using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RtsCamera : MonoBehaviour {
    [SerializeField]
    private float _camSpeed;
    [SerializeField]
    private float _topBorder;
    [SerializeField]
    private float _botBorder;
    [SerializeField]
    private float _leftBorder;
    [SerializeField]
    private float _rightBorder;

    [SerializeField]
    private Vector3 _maxPos;
    [SerializeField]
    private Vector3 _minPos;

    private bool _isMoving;
    public bool IsMoving { get { return _isMoving; } }
    void Update () {
        Vector3 _direction = Vector3.zero;
        _isMoving = false;
        if (Input.mousePosition.y >= Screen.height * _topBorder && transform.position.z < _maxPos.z)
        {
            _direction += Vector3.forward;
        }
        if (Input.mousePosition.y <= Screen.height * _botBorder && transform.position.z > _minPos.z)
        {
            _direction += Vector3.back;
        }
        if (Input.mousePosition.x >= Screen.width * _rightBorder && transform.position.x < _maxPos.x)
        {
            _direction += Vector3.right;
        }
        if (Input.mousePosition.x <= Screen.width * _leftBorder && transform.position.x > _minPos.x)
        {
            _direction += Vector3.left;
        }
        if (_direction != Vector3.zero)
            _isMoving = true;
        transform.position = Vector3.Lerp(transform.position, transform.position + _direction, Time.deltaTime * _camSpeed);
	}
}
