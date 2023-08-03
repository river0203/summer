using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventColliderBeginBossFight : MonoBehaviour
{
    WorldEventManager _worldEventManager;

    private void Awake()
    {
        _worldEventManager = FindObjectOfType<WorldEventManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        /*if(other.tag == "player")
        {
            _worldEventManager.ActivateBossFight();
        }*/
        _worldEventManager.ActivateBossFight();
    }
}
