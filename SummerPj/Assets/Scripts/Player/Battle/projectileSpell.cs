using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Projectile Spell")]
public class projectileSpell : SpellItem
{
    public float baseDamage;
    public float projectileVelocity;
    public float projectileUpwardVelocity;
    public float projectileMass;
    public bool isEffectedbyGravity;
    Rigidbody rigid;

    public override void AttemptToCastSpell(PlayerAnimatorManager _animatorHandler, PlayerStats _playerStats, WeaponSlotManager _weaponSlotManager)
    {
        base.AttemptToCastSpell(_animatorHandler, _playerStats, _weaponSlotManager);
        GameObject instatiatedWarmUpSpellFX = Instantiate(spellWarmUpFX, _weaponSlotManager._rightHandSlot.transform);
        instatiatedWarmUpSpellFX.gameObject.transform.localScale = new Vector3(100, 100, 100);
        _animatorHandler.PlayTargetAnimation(spellAnimation, true);
    }

    public override void SucessfullyCastSpell(PlayerAnimatorManager _animatorHandler, PlayerStats _playerStats, CameraHandler _cameraHandler, WeaponSlotManager _weaponSlotManager)
    {
        base.SucessfullyCastSpell(_animatorHandler, _playerStats, _cameraHandler, _weaponSlotManager);
        GameObject instantiatedSpellFX = Instantiate(spellCastFX, _weaponSlotManager._rightHandSlot.transform.position, _cameraHandler._cameraPivotTransform.rotation);
        rigid = instantiatedSpellFX.GetComponent<Rigidbody>();

        if(_cameraHandler._currentLockOnTarget != null)
        {
            instantiatedSpellFX.transform.LookAt(_cameraHandler._currentLockOnTarget.transform);
        }
        else
        {
            instantiatedSpellFX.transform.rotation = Quaternion.Euler(_cameraHandler._cameraPivotTransform.eulerAngles.x, _playerStats.transform.eulerAngles.y, 0);
        }

        rigid.AddForce(instantiatedSpellFX.transform.forward * projectileVelocity);
        rigid.AddForce(instantiatedSpellFX.transform.up * projectileUpwardVelocity);
        rigid.useGravity = isEffectedbyGravity;
        rigid.mass = projectileMass;
    }
}
