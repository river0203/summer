using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BornFireInteractable : Interactable
{
    AudioSource _audioSource;

    public UIManager _uiManager;
    public AudioClip bonfireActivationSoundFX;

    public Transform bonfireTeleportTransform;

    public bool hasbeenActivated;

    public ParticleSystem activationFX;
    public ParticleSystem fireFX;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

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

        }
        else
        {
            _playerManager._playerAnimatorManager.PlayTargetAnimation("Bonfire_Activate", true);
            _uiManager.ActivateBonfirePopUp();
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
