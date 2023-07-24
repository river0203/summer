using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG
{
    public class EnemyAnimatorManager : AnimatorManager
    {
        EnemyManager _enemyManager;
        EnemyStats _enemyStats;
        private void Awake()
        {
            _anim = GetComponent<Animator>();
            _enemyManager = GetComponentInParent<EnemyManager>();
        }

        public override void TakeCriticalDamageAnimationEvent()
        {
            _enemyStats.TakeDamageNoAnimation(_enemyManager.pendingCriticalDamage);
            _enemyManager.pendingCriticalDamage = 0;
        }
        private void OnAnimatorMove()
        {
            float delta = Time.deltaTime;
            _enemyManager.enemyRigidBody.drag = 0;
            Vector3 deltaPosition = _anim.deltaPosition;
            deltaPosition.y = 0;

            Vector3 velocity = deltaPosition / delta;
            _enemyManager.enemyRigidBody.velocity = velocity;
        }
    }
}

