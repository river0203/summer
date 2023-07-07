using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public enum State
    {
        Idle, Walk, Sprint,
        Jump, Fall,
        WeakAttack_1, WeakAttack_2, WeakAttack_3 ,StrongAttack, JumpAttack, 
        Heal, Heal_Walk,
        Dead
    }

}
