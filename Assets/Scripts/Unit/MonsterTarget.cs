using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTarget : Unit
{
    /// <summary>
    /// ���� ���
    /// </summary>
    public override void Attack()
    {
        return;
    }

    public override void TakenDamage(float Damage)
    {
        HpNotify();
        if(_hp <= 0)
        {
            GameFailNotify();
        }
    }

    /// <summary>
    /// UI�� HP�� �پ�� ���� ����
    /// </summary>
    private void HpNotify()
    {

    }

    /// <summary>
    /// ü���� 0�̵Ǿ� ������ ���� ���� �˸��� �Լ�
    /// </summary>
    private void GameFailNotify()
    {

    }


}
