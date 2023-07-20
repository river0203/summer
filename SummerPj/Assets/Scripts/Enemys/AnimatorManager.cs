using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{ 
    public class AnimatorManager : MonoBehaviour
    {
        //AnimatorHandler => AnimatorManager 

        public Animator anim;
        public void PlayerTargetAnimation(string targetAnim, bool isInteracting)
        {
            anim.applyRootMotion = isInteracting;
            anim.SetBool("isInteracting", isInteracting);
            anim.CrossFade(targetAnim, 0.2f);
        }
    }
}
