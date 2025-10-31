using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : Piece
{
    [SerializeField] private List<GameObject> _monsterList;
    [SerializeField] private float _attackRange = 1f;
   

    private void Start()
    {
        _attackPoint = 7;//����� ���ݷ� �󸶳� ����
        _unitName = GoldManager.UnitNameEnum.Knight;
    }
    BattleManager battleManager = FindObjectOfType<BattleManager>();
    public override void Attack()
    {
       GameObject targetMonster = TargetFirstMonster();
        if (targetMonster != null)
        {
            if (battleManager != null)
            {
                battleManager.UnitAttack(_attackPoint, _attackRange);
            }
        }
    }
    /// <summary>
    /// ù��° ���͸� Ÿ����
    /// </summary>
    private GameObject TargetFirstMonster()
    {
        if (battleManager == null)
        {
            return battleManager.Target(_attackRange);
        }
        return null;
    }
}
