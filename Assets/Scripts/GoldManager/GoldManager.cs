using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldManager : MonoBehaviour
{
    // ���� �̸� enum ���߿� ���׷��̵� ���� �߰�
    public enum UnitNameEnum
    {
        Knight,
        Wizard,
        _End
    }
    // ���� �̸� enum
    public enum MonsterNameEnum
    {
        Slime,
        Turtle,
        Box,
        _End
    }
    // ���� ���� ��ȭ
    private int _gold = 0;

    // _gold ������Ƽ
    public int Gold
    {
        get
        {
            return _gold;
        }
        private set
        {
            // ���� ����
            if (value < 0)
            {
                _gold = 0;
            }
            else
            {
                _gold = value;
            }
        }
    }
    
    // Dictionary�� ���ͺ� ���� ����
    private Dictionary<MonsterNameEnum, int> _monsterGold = new Dictionary<MonsterNameEnum, int>()
    {
        { MonsterNameEnum.Slime, 1 },
        { MonsterNameEnum.Turtle, 1 },
        { MonsterNameEnum.Box, 2 },
    };
    // Dictionary�� ���� ���� ����
    private Dictionary<UnitNameEnum, int> _unitGold = new Dictionary<UnitNameEnum, int>()
    {
        { UnitNameEnum.Knight, 10 },
        { UnitNameEnum.Wizard, 15 }
    };



    // ���� ����� �̸��� �����ͼ� ��� �߰�
    public void GoldAdd(MonsterNameEnum monsterName)
    {
        if(_monsterGold.TryGetValue(monsterName, out int reward))
        {
            Gold += reward;
        }
        else
        {
            Debug.Log("��ϵ��� ���� ����");
        }
    }

    // ���� ���Ž� ��� ����
    public void UnitBuy(UnitNameEnum unitName)
    {
        if (_unitGold.TryGetValue(unitName, out int price))
        {
            Gold -= price;
        }
        else
        {
            Debug.Log("��ϵ��� ���� ����");
        }
    }

    // ���� �ǸŽ� ��� �߰� �Ҽ����� �ݿø��ؼ� ���
    public void UnitSell(UnitNameEnum unitName)
    {
        if (_unitGold.TryGetValue(unitName, out int price))
        {
            int sellingPrice = Mathf.RoundToInt(price * 0.8f);
            Gold += sellingPrice;
        }
    }
}
