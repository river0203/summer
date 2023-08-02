using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentManager : MonoBehaviour
{
    InputHandler _inputHandler;
    PlayerInventoryManager _playerInventoryManager;
    public BlockingCollider _blockingCollider;

    private void Awake()
    {
        _inputHandler = GetComponent<InputHandler>();
        _playerInventoryManager = GetComponent<PlayerInventoryManager>();
    }
    
    public void OpenBlockingCollider()
    {
        if (_inputHandler._twoHandFlag)
        {
            _blockingCollider.SetColliderDamageAbsorption(_playerInventoryManager._currentWeapon);
        }

        _blockingCollider.EnableBlockingCollider();
    }

    public void CloseBlockingCollider()
    {
        _blockingCollider.DisableBlockingCollider();

    }

}
