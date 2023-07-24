using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickUp : Interactable
{
    public WeaponItem _weapon;

    public override void Interact(PlayerManager playerManager)
    {
        base.Interact(playerManager);

        PickUpItem(playerManager);
    }

    public void PickUpItem(PlayerManager playerManager)
    {
        PlayerInventory _playerInventory = playerManager.GetComponent<PlayerInventory>();
        PlayerLocomotion _playerLocomotion = playerManager.GetComponent<PlayerLocomotion>();
        PlayerAnimatorManager _animHandler = playerManager.GetComponentInChildren<PlayerAnimatorManager>();

        _playerLocomotion._rigid.velocity = Vector3.zero;
        _animHandler.PlayTargetAnimation("PickUpItem", true);
        _playerInventory._weaponsInventory.Add(_weapon);
        Destroy(gameObject);
    }
}
