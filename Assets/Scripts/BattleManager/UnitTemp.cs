using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �׽�Ʈ�� �ӽ� ����
public class UnitTemp : MonoBehaviour
{
    public string unitName;
    public int Health;
    public int AttackPower;
    public int AttackRange;
    public int Defense;
    public float attackCooldown = 1f;

    public void Awake()
    {
        unitName = "knight";
        AttackPower = 3;
        AttackRange = 1;
        Defense = 1;
    }
}
