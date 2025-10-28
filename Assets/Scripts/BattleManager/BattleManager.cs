using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public List<GoldManager> units = new List<GoldManager>();
    public List<GoldManager> monsters = new List<GoldManager>();


    void Battle()
    {

    }

    // 유닛이 공격할 몬스터 탐색
    GoldManager FindTarget(GoldManager unit)
    {
        GoldManager selected = null;
        return selected;
    }

    // 유닛이 몬스터 공격할때
    void Damage(GoldManager attacker, GoldManager target)
    {

    }
}