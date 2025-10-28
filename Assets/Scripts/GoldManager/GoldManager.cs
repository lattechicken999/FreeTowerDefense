using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldManager : MonoBehaviour
{
    // 유닛 이름 enum 나중에 업그레이드 유닛 추가
    public enum UnitNameEnum
    {
        Knight,
        Wizard,
        _End
    }
    // 몬스터 이름 enum
    public enum MonsterNameEnum
    {
        Slime,
        Turtle,
        Box,
        _End
    }
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
    
    // Dictionary로 몬스터별 보상 적기
    private Dictionary<MonsterNameEnum, int> _monsterGold = new Dictionary<MonsterNameEnum, int>()
    {
        { MonsterNameEnum.Slime, 1 },
        { MonsterNameEnum.Turtle, 1 },
        { MonsterNameEnum.Box, 2 },
    };
    // Dictionary로 유닛 가격 적기
    private Dictionary<UnitNameEnum, int> _unitGold = new Dictionary<UnitNameEnum, int>()
    {
        { UnitNameEnum.Knight, 10 },
        { UnitNameEnum.Wizard, 15 }
    };



    // 몬스터 사망시 이름을 가져와서 골드 추가
    public void GoldAdd(MonsterNameEnum monsterName)
    {
        if(_monsterGold.TryGetValue(monsterName, out int reward))
        {
            Gold += reward;
        }
        else
        {
            Debug.Log("등록되지 않은 몬스터");
        }
    }

    // 유닛 구매시 골드 차감
    public void UnitBuy(UnitNameEnum unitName)
    {
        if (_unitGold.TryGetValue(unitName, out int price))
        {
            Gold -= price;
        }
        else
        {
            Debug.Log("등록되지 않은 유닛");
        }
    }

    // 유닛 판매시 골드 추가 소수점은 반올림해서 계산
    public void UnitSell(UnitNameEnum unitName)
    {
        if (_unitGold.TryGetValue(unitName, out int price))
        {
            int sellingPrice = Mathf.RoundToInt(price * 0.8f);
            Gold += sellingPrice;
        }
    }
}
