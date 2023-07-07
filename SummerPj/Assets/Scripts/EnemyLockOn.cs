using UnityEngine;
using Cinemachine;

public class EnemyLockOn : MonoBehaviour
{
    #region  LockOn
    [Header("Lock On")]

    [Tooltip("락온 탐지 범위")]
    [SerializeField] float noticeZone = 10f;

    [Tooltip("최대 탐지 범위")]
    [SerializeField] float maxNoticeAngle = 60f;
    
    [Tooltip("락온 타겟 레이어")]
    [SerializeField] LayerMask targetLayers;

    [Tooltip("락온 UI")]
    [SerializeField] Transform LockOn_UI;

    [Tooltip("락온 대상과의 최대 거리")]
    [SerializeField] float maxDistance = 10f;

    [Tooltip("락온 카메라 위치")]
    [SerializeField] Transform LockOn_CameraTarget;

    [HideInInspector]public Transform _currentTarget;
    Transform _camera;
    PlayerInputActions _input;
    [SerializeField] CinemachineVirtualCamera _cinemachine;
    #endregion

    void Start()
    {
        _camera = Camera.main.transform;
        _input = GetComponent<PlayerInputActions>();
        LockOn_UI.gameObject.SetActive(false);
    }
    
    void Update()
    {
        if(_input.lockOn)
        {
            if (_currentTarget == null)
            {
                _currentTarget = Scan();
                Debug.Log(Scan());

                FoundTarget();
            }
            else
            {
                ResetTarget();
            }

            _input.lockOn = false;
        }

        if (_currentTarget != null)
        {
            _cinemachine.LookAt = _currentTarget;
            if (!TargetOnRange() || Blocked()) ResetTarget();
        }
        else _cinemachine.LookAt = null;
    }
        
    Transform Scan()
    {
            Collider[] nearbyTargets = Physics.OverlapSphere(transform.position, noticeZone, targetLayers);
            float closestAngle = maxNoticeAngle;
            Transform currentTarget = null;
            if (nearbyTargets.Length <= 0) return null;

            for (int i = 0; i < nearbyTargets.Length; i++)
            {
                Vector3 CurrentCalculationTarget = nearbyTargets[i].transform.position - _camera.position;
                CurrentCalculationTarget.y = 0;
                float _angle = Vector3.Angle(_camera.forward, CurrentCalculationTarget);

                if (_angle < closestAngle)
                {
                    currentTarget = nearbyTargets[i].transform;
                    closestAngle = _angle;
                }
            }
            if(!currentTarget) return null;

            return currentTarget;
        }
    bool Blocked()
    {
        float height = _currentTarget.GetComponent<CapsuleCollider>().height * _currentTarget.localScale.y;
        float half_height = (height / 2) / 2;
        Vector3 targetPosition = _currentTarget.position + new Vector3(0, height - half_height, 0);
       
        RaycastHit hit;
        if (Physics.Linecast(transform.position + Vector3.up * 0.5f, targetPosition, out hit))
        {
            if (!hit.transform.CompareTag("Enemy")) return true;
        }
        return false;
    }
    bool TargetOnRange()
    {
        float distance = (transform.position - _currentTarget.position).magnitude;
        if (distance / 2 > maxDistance) return false; else return true;
    }
    void FoundTarget()
    {
        LockOn_UI.gameObject.SetActive(true);
    }
    void ResetTarget()
    {
        LockOn_UI.gameObject.SetActive(false);
        _currentTarget = null;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, noticeZone);
    }
}
