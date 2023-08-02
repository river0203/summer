using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSoundFXManager : MonoBehaviour
{
    CharacterManager _characterManager;
    AudioSource _audioSource;

    [Header("Taking Damage Sounds")]
    public AudioClip[] takingDamageSounds;
    List<AudioClip> potentialDamageSounds;
    AudioClip lastDamageSoundPlayed;

    [Header("Weapon Whooshes")]
    List<AudioClip> potentialWeaponWhooshes;
    AudioClip lastWeaponWhooshes;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _characterManager = GetComponent<CharacterManager>();
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

        if(_characterManager.isUsingRightHand)
        {
            foreach(var whooshSound in _characterManager._playerInventoryManager._rightWeapon.weaponWhooshes)
            {
                if(whooshSound != lastWeaponWhooshes)
                {
                    potentialWeaponWhooshes.Add(whooshSound);
                }

                int randomValue = Random.Range(0, potentialWeaponWhooshes.Count);
                lastWeaponWhooshes = takingDamageSounds[randomValue];
                _audioSource.PlayOneShot(_characterManager._playerInventoryManager._rightWeapon.weaponWhooshes[randomValue]);
            }
        }
    }
}
