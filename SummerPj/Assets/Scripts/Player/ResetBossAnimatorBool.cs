using UnityEngine;

public class ResetBossAnimatorBool : StateMachineBehaviour
{
    public string _isPreformingAction = "isPreformingAction";
    public bool _isPreformingStatus = false;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(_isPreformingAction, _isPreformingStatus);
    }
}
