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

    // ������ ������ ���� Ž��
    Monster FindTarget(Unit unit)
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
    void Damage(Unit attacker, Monster target)
    {

    }

    void Damage(Monster attacker, Unit target)
    {

    }
}