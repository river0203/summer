using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventColliderBeginBossFight : MonoBehaviour
{
    WorldEventManager _worldEventManager;
    EnemyAnimatorManager _enemyAnimatorManger;

    public string _inAnim;
    public bool _startingAnim;

    private void Awake()
    {
        _worldEventManager = FindObjectOfType<WorldEventManager>();
        _enemyAnimatorManger = FindObjectOfType<EnemyAnimatorManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        /*if(other.tag == "player")
        {
            _worldEventManager.ActivateBossFight();
        }*/
        _worldEventManager.ActivateBossFight();

        _enemyAnimatorManger.PlayTargetAnimation(_inAnim, true);
    }

}
