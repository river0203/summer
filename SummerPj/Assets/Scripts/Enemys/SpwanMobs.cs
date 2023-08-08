using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpwanMobs : MonoBehaviour
{
    public GameObject rangeObject;
    BoxCollider rangeCollider;
    public GameObject capsul;
    private AttackState _attackState;
    private int _countMobs = 0;
    private float _spawnRange = 8f;
    public Transform _boss;
    private void Update()
    {
        if(_attackState._spawingMobs)
        {
            RandomRespawn_Coroutine();
            _attackState._spawingMobs = false;
        }
    }

    void RandomRespawn_Coroutine()
    {

        while (_countMobs < 5)
        {
            // ���� ��ġ �κп� ������ ���� �Լ� Return_RandomPosition() �Լ� ����
            GameObject instantCapsul = Instantiate(capsul, Return_RandomPosition(), Quaternion.identity);
            _countMobs++;
        }
        
        _countMobs = 0;
    }

    private void Awake()
    {
        rangeCollider = rangeObject.GetComponent<BoxCollider>();
        _attackState = FindObjectOfType<AttackState>();
    }

    Vector3 Return_RandomPosition()
    {
        Vector3 originPosition = rangeObject.transform.position;
        // �ݶ��̴��� ����� �������� bound.size ���
        float range_X = rangeCollider.bounds.size.x;
        float range_Z = rangeCollider.bounds.size.z;

        range_X = Random.Range((range_X / 2) * -1, range_X / 2);
        range_Z = Random.Range((range_Z / 2) * -1, range_Z / 2);
        Vector3 RandomPostion = new Vector3(range_X, 0f, range_Z);

        Vector3 respawnPosition = originPosition + RandomPostion;
        return respawnPosition;
    }
}
