using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentManager : MonoBehaviour
{
    InputHandler _inputHandler;
    PlayerInventory _playerInventory;
    public BlockingCollider _blockingCollider;

    private void Awake()
    {
        _inputHandler = GetComponentInParent<InputHandler>();
        _playerInventory = GetComponentInParent<PlayerInventory>();
    }
    
    public void OpenBlockingCollider()
    {
        if (_inputHandler._twoHandFlag)
        {
            _blockingCollider.SetColliderDamageAbsorption(_playerInventory._rightWeapon);
        }
        else
        {
            _blockingCollider.SetColliderDamageAbsorption(_playerInventory._leftWeapon);
        }

        _blockingCollider.EnableBlockingCollider();
    }

    public void CloseBlockingCollider()
    {
        _blockingCollider.DisableBlockingCollider();

    }

}
