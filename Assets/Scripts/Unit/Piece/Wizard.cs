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
            var monsterUnit = targetMonster.GetComponent<Monster>();//���� ���� ��������
            if (monsterUnit != null)
            {
                monsterUnit.TakenDamage(_attackPoint);//���Ϳ��� ������ �ֱ�
            }
        }
    }
    /// <summary>
    /// ù��° ���͸� Ÿ����
    /// </summary>
    private GameObject TargetFirstMonster()
    {
        foreach (var monster in _monsterList)
        {
            if (monster == null)//���� ���ʹ� �н�
                continue;
            if (Vector3.Distance(monster.transform.position, transform.position) <= _attackRange)
                return monster;//��Ÿ� ���� �ִ� ù��° ���� ����
        }
        return null;
    }
}
