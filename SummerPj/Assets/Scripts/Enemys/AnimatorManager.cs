using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

    public class AnimatorManager : MonoBehaviour
    {
        protected CharacterManager _characterManager;
        protected CharacterStatsManager _characterStatsManager;
        public Animator _anim;
        public bool canRotate;
  
    protected virtual void Awake()
        {
            _characterManager = GetComponent<CharacterManager>();
            _characterStatsManager = GetComponent<CharacterStatsManager>();
        }
        public void PlayTargetAnimation(string targetAnim, bool isInteracting, bool canRotate = false)
        {
            _anim.applyRootMotion = isInteracting;
            _anim.SetBool("canRotate", canRotate);
            _anim.SetBool("isInteracting", isInteracting);
            _anim.CrossFade(targetAnim, 0.2f);
        }
        public virtual void CanRotate()
    {
        _anim.SetBool("canRotate", true);
    }
        public virtual void StopRotate()
    {
        _anim.SetBool("canRotate", false);
    }
        public virtual void EnableCombo()
    {
        _anim.SetBool("canDoCombo", true);
    }
        public virtual void Disablecombo()
    {
        _anim.SetBool("canDoCombo", false);
    }
        public virtual void EnableIsInvulerable()
    {
        _anim.SetBool("isInvulnerable", true);
    }
        public virtual void DisableIsvulerable()
    {
        _anim.SetBool("isInvulnerable", false);
    }

        public virtual void EnableParrying()
        {
            _characterManager.isParrying = true;
        }
        public virtual void DisableParrying()
        {
            _characterManager.isParrying = false;
        }
        public virtual void EnableCanBeRiposted()
        {
            _characterManager.canBeRiposted = true;
        }
        public virtual void DisableCanBeRiposted()
        {
            _characterManager.canBeRiposted = false;
        }

        public virtual void TakeCriticalDamageAnimationEvent()
        {
        _characterStatsManager.TakeDamageNoAnimation(_characterManager.pendingCriticalDamage);
        _characterManager.pendingCriticalDamage = 0;
        }
}

