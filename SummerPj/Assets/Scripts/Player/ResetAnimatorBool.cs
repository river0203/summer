using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ResetAnimatorBool : StateMachineBehaviour
{
    public string _isPreformingAction = "isPreformingAction";
    public bool _isPreformingStatus = false;

    public string _isInteractingBool = "isInteracting";
    public bool _isInteractingStatus = false;

    public string _isFiringSpellBool = "isFiringSpell";
    public bool _isFiringSpellstatus = false;

    public string _canRotate = "canRotate";
    public bool _canRotateStatus = true;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(_isInteractingBool, _isInteractingStatus);
        animator.SetBool(_isFiringSpellBool, _isFiringSpellstatus);
        animator.SetBool(_canRotate, _canRotateStatus);
        animator.SetBool(_isPreformingAction, _isPreformingStatus);
    }
}
