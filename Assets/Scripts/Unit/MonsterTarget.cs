using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTarget : Unit
{
    /// <summary>
    /// 죽은 기능
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
    /// UI에 HP가 줄어든 것을 보고
    /// </summary>
    private void HpNotify()
    {

    }

    /// <summary>
    /// 체력이 0이되어 게임이 종료 됨을 알리는 함수
    /// </summary>
    private void GameFailNotify()
    {

    }


}
