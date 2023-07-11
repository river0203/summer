using UnityEngine;

public class PlayerStates : MonoBehaviour
{
    public enum State
    {
        Idle, Walk, Sprint,
        Jump, Fall, Land, 
        Dodge,
        WeakAttack_1, WeakAttack_2, WeakAttack_3, WeakAttack_4, WeakAttack_5, WeakAttack_6, StrongAttack, JumpAttack,
        Heal, Heal_Walk,
        Damaged, Dead
    }

}
