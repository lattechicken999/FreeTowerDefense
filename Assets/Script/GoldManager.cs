using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldManager : MonoBehaviour
{
    // 게임 메인 재화
    private int _gold = 0;

    // _gold 프로퍼티
    public int Gold
    {
        get
        {
            return _gold;
        }
        private set
        {
            // 음수 방지
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

    // 몬스터 사망시 이름을 가져와서 골드 추가
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
            Debug.Log("등록되지 않은 몬스터입니다.");
        }
    }

    // 유닛 구매시 골드 차감
    public void UnitBuy(int price)
    {
        if (Gold < price)
        {
            Debug.Log("골드 부족");
        }
        Gold = Gold - price;
    }

    // 유닛 구매시 골드 추가
    public void UnitSell(int price)
    {
        int sellingPrice = (int)Math.Round(price * 0.8);
        Gold += sellingPrice;
    }
}
