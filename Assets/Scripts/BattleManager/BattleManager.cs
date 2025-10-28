using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public List<Unit> units = new List<Unit>();
    public List<Monster> monsters = new List<Monster>();

    void Battle()
    {

    }

    // 유닛이 공격할 몬스터 탐색
    Monster FindTarget(Unit unit)
    {
        Monster selected = null;
        return selected;
    }

    // 몬스터가 공격할 유닛 탐색
    Unit FindTarget(Monster monster)
    {
        Unit selected = null;
        return selected;
    }

    // 유닛이 몬스터 공격할때
    void Damage(Unit attacker, Monster target)
    {

    }

    void Damage(Monster attacker, Unit target)
    {

    }
}