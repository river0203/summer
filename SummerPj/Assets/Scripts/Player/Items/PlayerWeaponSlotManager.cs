using UnityEngine;

public class PlayerWeaponSlotManager : CharacterWeaponSlotManager
{
    PlayerInventoryManager _playerInventoryManager;
    QuickSlotsUI _quickSlotsUI;
    PlayerStatsManager _playerStats;
    PlayerEffectsManager _playerEffectsManager;
    PlayerSoundFXManager _playerSoundFXManager;

    public WeaponItem _attackingWeapon;

    private void Awake()
    {
        _weaponSlot = GameObject.Find("hand_r").GetComponent<WeaponHolderSlot>();

        _playerSoundFXManager = GetComponent<PlayerSoundFXManager>();
        _quickSlotsUI = FindObjectOfType<QuickSlotsUI>();
        _playerEffectsManager = GetComponent<PlayerEffectsManager>();
        _playerInventoryManager = GetComponent<PlayerInventoryManager>();
        _playerStats = GetComponent<PlayerStatsManager>();
        
        LoadWeaponHolderSlots();
    }

    void LoadWeaponHolderSlots()
    {
        WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
        foreach (WeaponHolderSlot weaponslot in weaponHolderSlots)
        {
            if (weaponslot._isWeaponHandSlot)
            {
                _weaponSlot = weaponslot;
            }
            else if (weaponslot._isBackSlot)
            {
                _backSlot = weaponslot;
            }
        }
    }

    public void LoadBothWeaponsOnSlot()
    {
        LoadWeaponOnSlot(_playerInventoryManager._currentWeapon);
    }
    public void LoadWeaponOnSlot(WeaponItem weaponItem)
    {
        if(weaponItem != null)
        {
             //animator.CrossFade(weaponItem.right_hand_idle, 0.2f);
             _backSlot.UnloadWeaponAndDestroy();

             _weaponSlot._currentWeapon = weaponItem;
             _weaponSlot.LoadWeaponModel(weaponItem);
             LoadWeaponDamageCollider();
             _quickSlotsUI.UpdateWeaponQuickSlotsUI(weaponItem, _playerInventoryManager._currentSpell);

        }
        else
        {
            weaponItem = _unarmedWeapon;

            //animator.CrossFade("Right Arm Empty", 0.2f);
            _playerInventoryManager._currentWeapon = _unarmedWeapon;
            _weaponSlot._currentWeapon = _unarmedWeapon;
            _weaponSlot.LoadWeaponModel(_unarmedWeapon);
            LoadWeaponDamageCollider();
            _quickSlotsUI.UpdateWeaponQuickSlotsUI(_unarmedWeapon, _playerInventoryManager._currentSpell);
        }
        // 무기 로드

        // 슬롯 UI 로드
        _quickSlotsUI.UpdateWeaponQuickSlotsUI(weaponItem, _playerInventoryManager._currentSpell);
    }

    #region 데미지 콜라이더
    private void LoadWeaponDamageCollider()
    {
        if (_weaponSlot._currentWeaponModel == null)
            return;

        //_rightHandDamageCollider._characterManager = _characterManager;
        _currentDamageCollider = _weaponSlot._currentWeaponModel.GetComponentInChildren<DamageCollider>(); 
        _currentDamageCollider._currentWeaponDamage = _playerInventoryManager._currentWeapon.baseDamage;
        _playerEffectsManager._weaponFX = _weaponSlot._currentWeaponModel.GetComponentInChildren<WeaponFX>();
    }

    public void OpenDamageCollier()
    {
        _playerSoundFXManager.PlayRandomWeaponWhoosh();
        _currentDamageCollider.EnableDamagecollider();
    }
    public void CloseDamageCollier()
    {
        _currentDamageCollider.DisableDamagecollider();
    }
    #endregion

    #region 스테미나
    public void DrainStaminaLightAttack()
    {
        _playerStats.TakeStaminaDamage(Mathf.RoundToInt(_attackingWeapon.baseStaminar * _attackingWeapon.lightAttackMultiplier));
    }

    public void DrainStaminaHeavyAttack()
    {
        _playerStats.TakeStaminaDamage(Mathf.RoundToInt(_attackingWeapon.baseStaminar * _attackingWeapon.heavyAttackMultiplier));
    }
    #endregion
}
