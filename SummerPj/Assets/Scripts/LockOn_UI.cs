using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOn_UI : MonoBehaviour
{
    [SerializeField]GameObject _player;
    EnemyLockOn _enemyLockOn;
    float _currentYOffset = 0.5f;
    void Start()
    {
        gameObject.SetActive(false);
        _enemyLockOn = _player.GetComponent<EnemyLockOn>();
    }

    void Update()
    {
        transform.position = new Vector3(_enemyLockOn._currentTarget.transform.position.x, _enemyLockOn._currentTarget.transform.position.y + _currentYOffset, _enemyLockOn._currentTarget.transform.position.z);
        Vector3 _dir = _enemyLockOn._currentTarget.position - transform.position;
        transform.rotation = Quaternion.LookRotation(_dir);
    }
}
