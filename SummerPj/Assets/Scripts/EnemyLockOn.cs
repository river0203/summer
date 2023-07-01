using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEditor.Experimental.GraphView;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class EnemyLockOn : MonoBehaviour
{
    #region  LockOn
    [Header("Lock On")]

    [Tooltip("락온 탐지 범위")]
    [SerializeField] float noticeZone = 10;

    [Tooltip("최대 탐지 범위")]
    [SerializeField] float maxNoticeAngle = 60;
    
    [Tooltip("락온 타겟 레이어")]
    [SerializeField] LayerMask targetLayers;

    [Tooltip("락온 UI")]
    [SerializeField] Transform LockOn_UI;

    bool TargetLockOn;
    Transform currentTarget;
    Transform _camera;
    float currentYOffset;
    PlayerInputActions _input;
    Vector3 position;
    #endregion
    void Start()
    {
        _camera = Camera.main.transform;
        _input = GetComponent<PlayerInputActions>();
    }
    
    void Update()
    {
        if(_input.LockOn)
        {
            if (currentTarget)
            {
                ResetTarget();
                return;
            }
            
            if (currentTarget = Scan()) FoundTarget(); else ResetTarget();
            _input.LockOn = false;
        }

        if(TargetLockOn) 
        { 
            if (!TargetOnRange()) ResetTarget();
            // Target방향 쳐다보기 & 락온UI 위치 변경
        }
    }

    Transform Scan()
    {
            Collider[] nearbyTargets = Physics.OverlapSphere(transform.position, noticeZone, targetLayers);
            float closestAngle = maxNoticeAngle;
            Transform closestTarget = null;
            if (nearbyTargets.Length <= 0) return null;

            for (int i = 0; i < nearbyTargets.Length; i++)
            {
                Vector3 CurrentCalculationTarget = nearbyTargets[i].transform.position - _camera.position;
                CurrentCalculationTarget.y = 0;
                float _angle = Vector3.Angle(_camera.forward, CurrentCalculationTarget);

                if (_angle < closestAngle)
                {
                    closestTarget = nearbyTargets[i].transform;
                    closestAngle = _angle;
                }
            }
            if(!closestTarget) return null;

            float height = closestTarget.GetComponent<CapsuleCollider>().height * closestTarget.localScale.y;
            float half_height = (height / 2) / 2;

            Vector3 targetPosition = closestTarget.position + new Vector3(0, height - half_height, 0);
            if (Blocked(targetPosition)) return null;
            return closestTarget;
        }
    bool Blocked(Vector3 targetPosition)
    {
        RaycastHit hit;
        if (Physics.Linecast(transform.position + Vector3.up * 0.5f, targetPosition, out hit))
        {
            if (!hit.transform.CompareTag("Enemy")) return true;
        }
        return false;
    }
    bool TargetOnRange()
    {
        float distance = (transform.position - position).magnitude;
        if (distance / 2 > noticeZone) return false; else return true;
    }
    void FoundTarget()
    {
        LockOn_UI.gameObject.SetActive(true);
        TargetLockOn = true;
    }
    void ResetTarget()
    {
        LockOn_UI.gameObject.SetActive(false);
        currentTarget = null;
        TargetLockOn = false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, noticeZone);
    }
}
