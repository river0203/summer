using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static LivingEntity;

public class EnemySys : MonoBehaviour
{
    Transform _player;
    NavMeshAgent _sysAgent;
    Rigidbody _sysRigid;

    [SerializeField]
    float _startingHP;
    [SerializeField]
    float _damage;

    float _hp;
    
    void Start()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        _sysAgent = gameObject.GetComponent<NavMeshAgent>();
        _sysRigid  = gameObject.GetComponent<Rigidbody>();
        _startingHP = _hp;
    }

    private void BossMove()
    {
        float distance = Vector3.Distance(transform.position, _player.transform.position);
        _sysAgent.destination = _player.position;
    }
    // Update is called once per frame
    void Update()
    {
        BossMove();
    }

    void freeze_velocity()
    {
        _sysRigid.velocity = Vector3.zero;
        _sysRigid.angularVelocity = Vector3.zero;

    }

    private void FixedUpdate()
    {
        freeze_velocity();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Weapon"))
        {
            _hp -= _damage;
            //_sysState = LivingEntity.State.Stage2;

            if(_hp <= 0)
            {
                //_sysState = LivingEntity.State.Dead;
                Destroy(this.gameObject);
            }
        }

    }
}
