using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 콜라이더로 충돌 확인
// 충돌 확인 후 LivingEntity에서 데미지 계산
public class Weapon : LivingEntity 
{
    private State state;

    private void check_hitting()
    {
        void OnCollisionEnter(Collision other) 
        {
            Debug.Log("hit");
            state = State.IsHitting;
        }
    }

    IEnumerator hitting_delay()
    {
        yield return new WaitforSeconds(50f);
    }
}