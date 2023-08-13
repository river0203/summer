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

    public void PlayWeaponTrailFX()
    {
        _enemyEffactManager.PlayWeaponFX();
    }
        
    public void PlaySpawnScenematic()
    {
        _cameraAnim.Play("BossScenematic");
    }

    public override void CanRotate()
    {
        base.CanRotate();
    }

    public override void StopRotate()
    {
        base.StopRotate();  
    }
}