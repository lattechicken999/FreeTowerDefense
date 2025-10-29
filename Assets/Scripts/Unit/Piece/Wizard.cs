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
            var monsterUnit = targetMonster.GetComponent<Monster>();//몬스터 정보 가져오기
            if (monsterUnit != null)
            {
                monsterUnit.TakenDamage(_attackPoint);//몬스터에게 데미지 주기
            }
        }
    }
    /// <summary>
    /// 첫번째 몬스터를 타겟팅
    /// </summary>
    private GameObject TargetFirstMonster()
    {
        foreach (var monster in _monsterList)
        {
            if (monster == null)//죽은 몬스터는 패스
                continue;
            if (Vector3.Distance(monster.transform.position, transform.position) <= _attackRange)
                return monster;//사거리 내에 있는 첫번째 몬스터 리턴
        }
        return null;
    }
}
