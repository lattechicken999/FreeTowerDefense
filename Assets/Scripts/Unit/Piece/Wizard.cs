using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : Piece
{
    [SerializeField] private List<GameObject> _monsterList;
    [SerializeField] private float _attackRange = 3f;
    
    private void Start()
    {
              _attackPoint = 7;//마법사는 공격력 얼마나 할지
    }
    
    public override void Attack()
    {     
       GameObject targetMonster = TargetFirstMonster();

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
        if (BattleManager.Instance == null)
        { 
         return BattleManager.Instance.Target(_attackRange);
        }
            return null;
    }
}
