using UnityEngine;

public class ResetPlayerAnimatorBool : StateMachineBehaviour
{
    public string _isInteractingBool = "isInteracting";
    public bool _isInteractingStatus = false;

    public string _canRotate = "canRotate";
    public bool _canRotateStatus = true;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(_isInteractingBool, _isInteractingStatus);
        animator.SetBool(_canRotate, _canRotateStatus);
        animator.applyRootMotion = true;
    }
}
