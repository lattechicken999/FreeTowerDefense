using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : Piece
{
    [SerializeField] private float _attackRange = 3f;
    
    protected override void Start()
    {
        _attackPoint = 15;//������� ���ݷ� �󸶳� ����
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
    /// ù��° ���͸� Ÿ����
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
