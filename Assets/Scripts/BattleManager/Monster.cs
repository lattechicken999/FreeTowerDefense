using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �׽�Ʈ�� �ӽ� ����
public class Monster : MonoBehaviour
{
    public string monsterName;
    public int Health;
    public int AttackPower;
    public int AttackRange;
    public int Defense;
    public float SpawnTime;
    public Monster()
    {
        monsterName = "Slime";
        Health = 10;
        AttackPower = 3;
        AttackRange = 1;
        Defense = 1;
    }
}
