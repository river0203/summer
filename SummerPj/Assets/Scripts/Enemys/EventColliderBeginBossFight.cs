using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventColliderBeginBossFight : MonoBehaviour
{
    WorldEventManager _worldEventManager;
    EnemyAnimatorManager _enemyAnimatorManger;
    EnemyManager _enemyManager;

    public string _inAnim;

    private void Awake()
    {
        _worldEventManager = FindObjectOfType<WorldEventManager>();
        _enemyAnimatorManger = FindObjectOfType<EnemyAnimatorManager>();
        _enemyManager = FindObjectOfType<EnemyManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        _enemyManager._currentTarget = other.gameObject.GetComponent<CharacterStatsManager>();
        _worldEventManager.ActivateBossFight();

        _enemyAnimatorManger._anim.Play(_inAnim);
        _enemyAnimatorManger._anim.SetBool("isPreformingAction", true);
    }

    private void OnTriggerExit(Collider other)
    {
        //_boxCollider.isTrigger = false;
    }
}
