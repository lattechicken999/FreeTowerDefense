using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public List<UnitTemp> units = new List<UnitTemp>();
    public List<Monster> monsters = new List<Monster>();


    void Battle()
    {

    }

    // ������ ������ ���� Ž��
    Monster FindTarget(UnitTemp unit)
    {
        Monster selected = null;
        return selected;
    }

    // ���Ͱ� ������ ���� Ž��
    Unit FindTarget(Monster monster)
    {
        Unit selected = null;
        return selected;
    }

    // ������ ���� �����Ҷ�
    void Damage(UnitTemp attacker, Monster target)
    {

    }

    void Damage(Monster attacker, UnitTemp target)
    {

    }
}