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
        Stage2, // isHitting
        Waiting,

        //½ºÄÌ·¹Åæ ½ºÅ³
        In,
        Skill1 = 10,
        Skill2 = 15,
        Skill3 = 17,
        Skill4 = 25,
        Skill5 = 100

    }
}
