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

    // ������ ������ ���� Ž��
    GoldManager FindTarget(GoldManager unit)
    {
        GoldManager selected = null;
        return selected;
    }

    // ������ ���� �����Ҷ�
    void Damage(GoldManager attacker, GoldManager target)
    {

    }
}