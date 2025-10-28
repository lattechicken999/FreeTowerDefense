using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTriggerTest : MonoBehaviour //테스트용도(끝나면 지워도됨) -> 몬스터 Hp바UI 테스트용
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            Debug.Log("충돌발생");
            Monster k = other.gameObject.GetComponent<Monster>();
            k.TakenDamage(2);
        }
    }
}
