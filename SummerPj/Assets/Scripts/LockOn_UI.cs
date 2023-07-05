using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOn_UI : MonoBehaviour
{
    [SerializeField]GameObject _player;
    EnemyLockOn _enemyLockOn;
    [SerializeField] Transform LockOn_Image;
    [SerializeField] float _currentYOffset = 0.5f;
    Transform _camera;
    void Start()
    {
        _camera = Camera.main.transform;
        _enemyLockOn = _player.GetComponent<EnemyLockOn>();
    }

    void Update()
    {
        LockOn_Image.transform.position = new Vector3(_enemyLockOn._currentTarget.transform.position.x, _enemyLockOn._currentTarget.transform.position.y + _currentYOffset, _enemyLockOn._currentTarget.transform.position.z);
        Vector3 _dir = _enemyLockOn._currentTarget.position - _camera.transform.position;
        LockOn_Image.transform.rotation = Quaternion.LookRotation(_dir);
    }
}
