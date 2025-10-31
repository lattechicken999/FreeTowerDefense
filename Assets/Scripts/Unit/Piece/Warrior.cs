using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : Piece
{
    [SerializeField] private float _attackRange = 1f;
    

    protected override void Start()
    {
        
        _attackPoint = 7;//����� ���ݷ� �󸶳� ����
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
                BattleManager.Instance.UnitAttack(_attackPoint, _attackRange);
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
