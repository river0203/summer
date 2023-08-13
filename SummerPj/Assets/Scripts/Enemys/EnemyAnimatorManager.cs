using UnityEngine;

public class EnemyAnimatorManager : AnimatorManager
{
    EnemyManager _enemyManager;
    EnemyEffactManager _enemyEffactManager;

    public Animator _cameraAnim;
    public GameObject _player;

    new private void Awake()
    {
        _anim = GetComponent<Animator>();
        _enemyManager = GetComponentInParent<EnemyManager>();
        _enemyEffactManager = GetComponent<EnemyEffactManager>();
    }

    public override void TakeCriticalDamageAnimationEvent()
    {
        //_enemyStats.TakeDamageNoAnimation(_enemyManager.pendingCriticalDamage);
        // _enemyManager.pendingCriticalDamage = 0;
    }
/*    public override void EnableParrying()
    {
        _enemyManager.isParrying = true;
    }
    public override void DisableParrying()
    {
        _enemyManager.isParrying = false;
    }
    public override void EnableCanBeRiposted()
    {
        _enemyManager.canBeRiposted = true;
    }
    public override void DisableCanBeRiposted()
    {
        _enemyManager.canBeRiposted = false;
    }*/

    public void PlayWeaponTrailFX()
    {
        _enemyEffactManager.PlayWeaponFX();
    }

    public void PlaySpawnScenematic()
    {
        _cameraAnim.Play("BossScenematic");
    }
}