using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour
{

    public enum State
    {
        Idle,
        Run,
        Attack,
        Dead,
        Stay,
        IsHitting,

        //½ºÄÌ·¹Åæ ½ºÅ³
        skell_skill_1 = 10,
        skell_skill_2 = 15,
        skell_skill_3 = 17,
        skell_skill_4 = 25,
        skell_skill_5 = 100
    }
    private State state;
}
