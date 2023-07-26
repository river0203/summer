using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class AnimatorManager : MonoBehaviour
    {
        public Animator _anim;
        public bool canRotate;

        // 애니메이션을 실행시키는 함수
        public void PlayTargetAnimation(string targetAnim, bool isInteracting) // isInteracting이란? 상호작용 중인가?
        {
            _anim.applyRootMotion = isInteracting;
            _anim.SetBool("canRotate", false);
            _anim.SetBool("isInteracting", isInteracting);
            _anim.CrossFade(targetAnim, 0.2f);
        }
        
        public virtual void TakeCriticalDamageAnimationEvent()
        {

        }
    }

