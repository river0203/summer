using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorManager : AnimatorManager
{
    EnemyManager _enemyManager;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _enemyManager = GetComponentInParent<EnemyManager>();
    }

    public override void TakeCriticalDamageAnimationEvent()
    {
        //_enemyStats.TakeDamageNoAnimation(_enemyManager.pendingCriticalDamage);
        _enemyManager.pendingCriticalDamage = 0;
    }
    public void EnableParrying()
    {
        _enemyManager.isParrying = true;
    }
    public void DisableParrying()
    {
        _enemyManager.isParrying = false;
    }
    public void EnableCanBeRiposted()
    {
        _enemyManager.canBeRiposted = true;
    }
    public void DisableCanBeRiposted()
    {
        _enemyManager.canBeRiposted = false;
    }
}