using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float _radius = 0.6f;
    public string _interactableText;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }

    public virtual void Interact(PlayerManager playerManager)
    {
        Debug.Log("Pick Up Item");
    }
}
