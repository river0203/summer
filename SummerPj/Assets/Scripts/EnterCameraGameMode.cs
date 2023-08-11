using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterCameraGameMode : StateMachineBehaviour
{
    CameraHandler _camera;

    private void Awake()
    {
        _camera = FindAnyObjectByType<CameraHandler>();
    }

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _camera.PlayerMode();
    }
}
