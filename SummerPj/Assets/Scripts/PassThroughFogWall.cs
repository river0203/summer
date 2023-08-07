using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassThou : Interactable
{
    WorldEventManager _worldEventManager;

    private void Awake()
    {
        _worldEventManager = FindObjectOfType<WorldEventManager>();
    }

    public override void Interact(PlayerManager playerManager)
    {
        base.Interact(playerManager);
    }
}
