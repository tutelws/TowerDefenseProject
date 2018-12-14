using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageProvider : MonoBehaviour {

    [SerializeField]
    private float _damage;

    public float Damage { get { return _damage; } }
}
