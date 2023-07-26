using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableItem : Item
{
    [Header("Item Quantity")]
    public int maxItemAmount;
    public int currentItemAmount;

    [Header("Item Model")]
    public GameObject itemModel;

    [Header("Animations")]
    public string consumeAnimation;
    public bool isInteracting;

    public virtual void AttemptToConsumeItem(PlayerAnimatorManager _playerAnimatorManager)
    {
        if(currentItemAmount > 0)
        {
            _playerAnimatorManager.PlayTargetAnimation(consumeAnimation, isInteracting);
        }
        else
        {
            _playerAnimatorManager.PlayTargetAnimation("Shrug", true);
        }
    }
}
