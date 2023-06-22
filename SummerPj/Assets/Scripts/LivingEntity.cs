using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour
{
    public float startingHealth = 100f;
    public float health {get; protected set;}
    public float damage; //상대에게 받는 데미지

    public enum State
    {
        Idle,
        Run,
        Attack,
        Dead,
        Stay
    }
    private State state;

    private void OnDamage()
    {
        health -= damage;
        if(health <= 0)
        {
            state = State.Dead;
        }
    }
}
