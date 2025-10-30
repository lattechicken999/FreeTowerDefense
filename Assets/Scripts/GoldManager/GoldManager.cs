using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� �ʿ� ���� enum�ȿ� ���ְ� ���� �ٸ� cs���� ����� ����, ��ųʸ��� ���� ����, �Ǹ� �ݾ� ����
/// <summary>
/// ��ȭ�� �����ϴ� �Ŵ��� Ŭ����, ���� óġ ����, ���� ���� �� �ǸŽ� ��ȭ ����
/// </summary>
public class GoldManager : Singleton<GameManager>
{
    /// <summary>
    /// ���� �̸� ������
    /// </summary>
    public enum UnitNameEnum
    {
        Knight,
        Wizard,
        _End
    }
    /// <summary>
    /// ���� �̸� ������
    /// </summary>
    public enum MonsterNameEnum
    {
        Slime,
        Turtle,
        Mummy,
        Ghost,
        _End
    }
    /// <summary>
    /// ���� �������� ��ȭ
    /// </summary>
    private int _gold = 0;

    /// <summary>
    /// ���� ��ȭ�� ��� ������Ƽ
    /// ���� 0 �Ʒ��� ���� �ʵ��� if�� ���
    /// </summary>
    public int Wallet
    {
        get
        {
            return _gold;
        }
        private set
        {
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

    /// <summary>
    /// ���� óġ ���� ����
    /// </summary>
    private Dictionary<MonsterNameEnum, int> _monsterGold = new Dictionary<MonsterNameEnum, int>()
    {
        { MonsterNameEnum.Slime, 1 },
        { MonsterNameEnum.Turtle, 1 },
        { MonsterNameEnum.Mummy, 2 },
        { MonsterNameEnum.Ghost, 2 }
    };
    /// <summary>
    /// ���ֺ� ���� ��� ����
    /// </summary>
    private Dictionary<UnitNameEnum, int> _unitGold = new Dictionary<UnitNameEnum, int>()
    {
        { UnitNameEnum.Knight, 10 },
        { UnitNameEnum.Wizard, 15 }
    };



    /// <summary>
    /// ���� óġ �� ��ȭ�� �߰��ϴ� �޼���
    /// </summary>
    /// <param name="monsterName">����� ������ �̸�</param>
    public void GoldAdd(MonsterNameEnum monsterName)
    {
        if (_monsterGold.TryGetValue(monsterName, out int reward))
        {
            Wallet += reward;
        }
        else
        {
            Debug.Log("��ϵ��� ���� ����");
        }
    }

    /// <summary>
    /// ���� ���Ž� ���� ��ȭ�� �����ϴ� �޼���
    /// </summary>
    /// <param name="unitName">������ ���� �̸�</param>
    public void UnitBuy(UnitNameEnum unitName)
    {
        if (_unitGold.TryGetValue(unitName, out int price))
        {
            Wallet -= price;
        }
        else
        {
            Debug.Log("��ϵ��� ���� ����");
        }
    }

    /// <summary>
    /// ���� �ǸŽ� ���� ��ȭ �߰�
    /// ������ �Ǹ� �ݾ��� ���� ������ 80%, �Ҽ����� �ݿø��ؼ� ���
    /// </summary>
    /// <param name="unitName">�Ǹ��� ���� �̸�</param>
    public void UnitSell(UnitNameEnum unitName)
    {
        if (_unitGold.TryGetValue(unitName, out int price))
        {
            int sellingPrice = Mathf.RoundToInt(price * 0.8f);
            Wallet += sellingPrice;
        }
    }

    /// <summary>
    /// �ʱ�ȭ�� ȣ��
    /// </summary>
    protected override void init()
    {
        // �θ� init ȣ��
        base.init();
    }
}
