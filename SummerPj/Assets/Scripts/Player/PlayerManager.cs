using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    InputHandler _inputHandler;
    Animator _anim;

    void Start()
    {
        _inputHandler = GetComponent<InputHandler>();
        _anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        _inputHandler._isInteracting = _anim.GetBool("isInteracting");
        _inputHandler._dodgeFlag = false;
        _inputHandler._sprintFlag = false;
    }
}
