using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static LivingEntity;

public class EnemySys : MonoBehaviour
{
    State _sysState;
    Transform _player;
    NavMeshAgent _sysAgent;
    
    void Start()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        _sysAgent = gameObject.GetComponent<NavMeshAgent>();
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
}
