using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldManager : MonoBehaviour
{
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

    // ���� ����� �̸��� �����ͼ� ��� �߰�
    public void GoldAdd(string monster)
    {
        if (monster == "slime")
        {
            Gold += 1;
        }
        else if (monster == "turtle")
        {
            Gold += 1;
        } else if (monster == "box")
        {
            Gold += 2;
        }
        else
        {
            Debug.Log("��ϵ��� ���� �����Դϴ�.");
        }
    }

    // ���� ���Ž� ��� ����
    public void UnitBuy(int price)
    {
        if (Gold < price)
        {
            Debug.Log("��� ����");
        }
        Gold = Gold - price;
    }

    // ���� ���Ž� ��� �߰�
    public void UnitSell(int price)
    {
        int sellingPrice = (int)Math.Round(price * 0.8);
        Gold += sellingPrice;
    }
}
