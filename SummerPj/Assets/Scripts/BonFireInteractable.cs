using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonFireInteractable : Interactable
{
    AudioSource _audioSource;

    public UIManager _UIManager;
    public AudioClip bonfireActivationSoundFX;

    public Transform bonfireTeleportTransform;

    public bool hasbeenActivated;

    public ParticleSystem activationFX;
    public ParticleSystem fireFX;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _UIManager = GameObject.FindWithTag("Player").GetComponent<UIManager>();

        if(hasbeenActivated)
        {
            fireFX.gameObject.SetActive(true);
            fireFX.Play();
            _interactableText = "Rest";
        }
        else
        {
            _interactableText = "Light Bonfire";
        }
    }
    public override void Interact(PlayerManager _playerManager)
    {
        base.Interact(_playerManager);

        if(hasbeenActivated)
        {
            _playerManager._playerAnimatorManager.PlayTargetAnimation("Bonfire_Activate", true);
            //_playerManager._playerStatsManager._currentHealth = _playerManager._playerStatsManager._maxHealth;
            _playerManager._playerInventoryManager.currentConsumable.currentItemAmount = _playerManager._playerInventoryManager.currentConsumable.maxItemAmount;
        }
        else
        {
            _playerManager._playerAnimatorManager.PlayTargetAnimation("Bonfire_Activate", true);
            // _UIManager.ActivateBonfirePopUp();
            hasbeenActivated = true;
            _interactableText = "Rest";
            activationFX.gameObject.SetActive(true);
            activationFX.Play();
            fireFX.gameObject.SetActive(true);
            fireFX.Play();
            _audioSource.PlayOneShot(bonfireActivationSoundFX);
        }
    }
}
