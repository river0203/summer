using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class AnimatorManager : MonoBehaviour
    {
        public Animator _anim;
        public bool canRotate;

        // �ִϸ��̼��� �����Ű�� �Լ�
        public void PlayTargetAnimation(string targetAnim, bool isInteracting) // isInteracting�̶�? ��ȣ�ۿ� ���ΰ�?
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

