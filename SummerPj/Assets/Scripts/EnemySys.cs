using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.AI;
using static LivingEntity;
using static UnityEngine.GraphicsBuffer;

public class EnemySys : MonoBehaviour
{
    Transform _player;
    Rigidbody _sysRigid;
    NavMeshAgent _sysAgent;
    EnemyAnim _enemyAnim;

    public State _state;

    [SerializeField]
    float _startingHP;
    [SerializeField]
    float _damage;

    float _hp;
    
    void Start()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        _sysAgent = gameObject.GetComponent<NavMeshAgent>();
        _sysRigid  = gameObject.GetComponent<Rigidbody>();
        _hp = _startingHP;
    }

    private void BossMove()
    {
        _state = EnemyAnim.EnemyState;

        if (_state == State.Run)
        {
            _sysAgent.transform.LookAt(_player.position);
            _sysAgent.destination = _player.position;
        }
        else if (_state == State.Skill1)
        {
            _sysAgent.transform.LookAt(_player.position);
        }
        else if(_state == State.Skill4)
        {
            _sysAgent.transform.LookAt(_player.position);
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        BossMove();
        Debug.Log(EnemyAnim.EnemyState);
    } 

    void freeze_velocity()
    {
        _sysRigid.velocity = Vector3.zero;
        _sysRigid.angularVelocity = Vector3.zero;

    }

    private void FixedUpdate()
    {
        freeze_velocity();
    }

    private void HittingDelay()
    {
        _sysAgent.GetComponent<CapsuleCollider>().isTrigger = false;
        /**EnemyAnim.EnemyState = State.Run;*/
    }

    private void Die()
    {
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.CompareTag("Player"))
        {
            _hp -= _damage;
            Debug.Log(_hp);
            if(_hp > 0)
            {
                _sysAgent.GetComponent<CapsuleCollider>().isTrigger = true;
                EnemyAnim.EnemyState = State.Stage2;
                Invoke("HittingDelay", 7f);
            }

            else if (_hp <= 0 )
            {
                EnemyAnim.EnemyState = State.Dead;
                _sysAgent.GetComponent<CapsuleCollider>().isTrigger = true;
                _sysAgent.isStopped = true;
                _sysAgent.enabled = false;
                Invoke("Die", 5f);
               
            }
        }
    }
}
