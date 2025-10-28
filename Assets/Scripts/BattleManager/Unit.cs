using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 테스트용 임시 파일
public class Unit : MonoBehaviour
{
    public string unitName;
    public int Health;
    public int AttackPower;
    public int AttackRange;
    public int Defense;

    public Unit()
    {
        unitName = "knight";
        AttackPower = 3;
        AttackRange = 1;
        Defense = 1;
    }
}
