using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : Piece
{
    [SerializeField] private float _attackRange = 3f;
    
    protected override void Start()
    {
        _attackPoint = 15;//마법사는 공격력 얼마나 할지
        _unitName = UnitEnum.Wizard;
        base.Start();
    }
    
    public override void Attack()
    {     
       targetMonster = TargetFirstMonster();

        if (targetMonster != null)
        {
            if (BattleManager.Instance != null)
            {
                BattleManager.Instance.UnitAttack(_attackPoint,_attackRange);
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
         return BattleManager.Instance.Target(_attackRange);
        }
            return null;
    }
}
