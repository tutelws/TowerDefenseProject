using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectile {

    void Init(int layer, float speed, Transform targetTransform = null);
}
