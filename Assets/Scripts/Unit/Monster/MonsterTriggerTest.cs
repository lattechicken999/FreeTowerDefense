using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTriggerTest : MonoBehaviour //�׽�Ʈ�뵵(������ ��������) -> ���� Hp��UI �׽�Ʈ��
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            Debug.Log("�浹�߻�");
            Monster k = other.gameObject.GetComponent<Monster>();
            k.TakenDamage(2);
        }
    }
}
