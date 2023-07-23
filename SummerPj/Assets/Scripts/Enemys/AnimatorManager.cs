using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{ 
    public class AnimatorManager : MonoBehaviour
    {
        public Animator _anim;

        public void PlayTargetAnimation(string targetAnim, bool isInteracting)
        {
            _anim.applyRootMotion = isInteracting;
            _anim.SetBool("isInteracting", isInteracting);
            _anim.CrossFade(targetAnim, 0.2f);
        }
    }
}
