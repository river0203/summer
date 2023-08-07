using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonResurrection : MonoBehaviour
{
    public Transform _origin;
    public Transform _target;

    public void SetPos(Vector3 origin, Vector3 target)
    {
        _origin.position = origin;
        _target.position = target;
    }
}
