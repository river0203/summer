using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity
{
    public enum State
    {
        Idle,
        Run,
        Attack,
        Dead,
        IsHitting,

        //½ºÄÌ·¹Åæ ½ºÅ³
        Skill1 = 10,
        Skill2 = 15,
        Skill3 = 17,
        Skill4 = 25,
        Skill5 = 100

    }
}
