using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventColliderBeginBossFight : MonoBehaviour
{
    WorldEventManager _worldEventManager;
    EnemyAnimatorManager _enemyAnimatorManger;
    BoxCollider _boxCollider;

    public string _inAnim;

    private void Awake()
    {
        _worldEventManager = FindObjectOfType<WorldEventManager>();
        _enemyAnimatorManger = FindObjectOfType<EnemyAnimatorManager>();
        _boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        /*if(other.tag == "player")
        {
            _worldEventManager.ActivateBossFight();
        }*/
        _worldEventManager.ActivateBossFight();

        // _enemyAnimatorManger.PlayTargetAnimation(_inAnim, true);
        _enemyAnimatorManger._anim.Play(_inAnim);
        _enemyAnimatorManger._anim.SetBool("isPreformingAction", true);
    }

    private void OnTriggerExit(Collider other)
    {
        //_boxCollider.isTrigger = false;
    }
}
