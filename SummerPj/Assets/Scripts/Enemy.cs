using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float enemy_speed;
    private float[] enemy_attack = new float[6];
    private NavMeshAgent agent;
    private State.CharState state;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        state = GetComponent<State.CharState>();
    }


}