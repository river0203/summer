using UnityEngine;

public class EnemyLocomotionManager : MonoBehaviour
{
    public CapsuleCollider _characterCollider;
    public CapsuleCollider _characterCollisionBlockerCollider;

    private void Start()
    {
        Physics.IgnoreCollision(_characterCollider, _characterCollisionBlockerCollider, true);
    }

}