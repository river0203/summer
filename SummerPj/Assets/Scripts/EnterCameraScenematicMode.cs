using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterCameraScenematicMode : StateMachineBehaviour
{
    CameraHandler _cameraHandler;
    Animator _cameraPivot;
    Transform _player;

    private void Awake()
    {
        _cameraHandler = FindAnyObjectByType<CameraHandler>();
        _cameraPivot = GameObject.Find("CameraPivot").GetComponent<Animator>();
        _player = GameObject.Find("Player").transform;
    }

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _cameraPivot.rootRotation = _player.transform.rotation;
        _cameraHandler.ScenematicMode();
    }
}
