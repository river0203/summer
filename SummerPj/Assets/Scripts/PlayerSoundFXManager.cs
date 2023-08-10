using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundFXManager : MonoBehaviour
{
    PlayerInventoryManager _playerInventoryManager;
    public AudioSource _audioSource;

    [Header("Taking Damage Sounds")]
    public AudioClip[] takingDamageSounds;
    List<AudioClip> potentialDamageSounds;
    AudioClip lastDamageSoundPlayed;

    [Header("Weapon Whooshes")]
    List<AudioClip> potentialWeaponWhooshes;
    AudioClip lastWeaponWhooshes;

    public AudioClip WalkSound;
    public AudioClip RunSound;
    public AudioClip JumpSound;
    public AudioClip LandSound;
    public AudioClip dodgeSound;
    public AudioClip DeadSound;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _playerInventoryManager = GetComponent<PlayerInventoryManager>();
    }
    public virtual void PlayRandomDamageSoundFX()
    {
        potentialDamageSounds = new List<AudioClip>();
        
        foreach(var damageSound in takingDamageSounds)
        {
            if(damageSound != lastDamageSoundPlayed)
            {
                potentialDamageSounds.Add(damageSound);
            }
        }
        int randomValue = Random.Range(0, potentialDamageSounds.Count);
        lastDamageSoundPlayed = takingDamageSounds[randomValue];
        _audioSource.PlayOneShot(takingDamageSounds[randomValue]);
    }
    public virtual void PlayRandomWeaponWhoosh()
    {
        potentialWeaponWhooshes = new List<AudioClip>();

        foreach (var whooshSound in _playerInventoryManager._currentWeapon.weaponWhooshes)
        {
            if (whooshSound != lastWeaponWhooshes)
            {
                potentialWeaponWhooshes.Add(whooshSound);
            }
        }

        int randomValue = Random.Range(0, potentialWeaponWhooshes.Count);
        lastWeaponWhooshes = _playerInventoryManager._currentWeapon.weaponWhooshes[randomValue];
        _audioSource.PlayOneShot(_playerInventoryManager._currentWeapon.weaponWhooshes[randomValue]);

    }

    #region Animation Event
    public void PlayWalkSound()
    {
        _audioSource.PlayOneShot(WalkSound);
    }

    public void PlayRunSound()
    {
        _audioSource.PlayOneShot(RunSound);
    }

    public void PlayJumpSound()
    {
        _audioSource.PlayOneShot(JumpSound);
    }

    public void PlayLandSound()
    {
        _audioSource.PlayOneShot(LandSound);
    }

    public void PlayDodgeSound()
    {
        _audioSource.PlayOneShot(dodgeSound);
    }
    #endregion
}