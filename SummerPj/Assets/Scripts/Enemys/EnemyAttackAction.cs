using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    [CreateAssetMenu(menuName ="A.I/Enemy Action/Attack Action")]
    public class EnemyAttackAction : EnemyAction
    {
        public int attackScore = 3;
        public int recoveryTime = 2;

        public float maximumAttackAngle = 35;
        public float minimumAttackAngle = -35;

        public float minimumDistanceNeededToAttack = 0;
        public float maximumDistanceNeededToAttack = 3;

    }
}

