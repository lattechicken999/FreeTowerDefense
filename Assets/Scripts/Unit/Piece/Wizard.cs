using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : Piece
{
    [SerializeField] private List<GameObject> _monsterList;
    [SerializeField] private float _attackRange = 3f;
    
    private void Start()
    {
              _attackPoint = 7;//������� ���ݷ� �󸶳� ����
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
    /// ù��° ���͸� Ÿ����
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
