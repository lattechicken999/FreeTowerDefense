﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : Piece
{
    [SerializeField] private float _attackRange = 1.5f;
    

    protected override void Start()
    {
        
        _attackPoint = 7;//전사는 공격력 얼마나 할지
        _unitName = UnitEnum.Warrior;
        base.Start();
    }
    
    public override void Attack()
    {
        targetMonster = TargetFirstMonster();
        if (targetMonster != null)
        {
            if (BattleManager.Instance != null)
            {
                BattleManager.Instance.UnitAttack(_attackPoint, _attackRange,transform);
            }
        }
    }
    /// <summary>
    /// 첫번째 몬스터를 타겟팅
    /// </summary>
    private GameObject TargetFirstMonster()
    {
        if (BattleManager.Instance != null)
        {
            return BattleManager.Instance.Target(_attackRange,transform);
        }
        return null;
    }
}
