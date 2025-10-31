﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 수정 필요 사항 enum안에 유닛과 몬스터 다른 cs파일 만들고 삭제, 딕셔너리에 보상 수정, 판매 금액 조정
/// <summary>
/// 재화를 관리하는 매니저 클래스, 몬스터 처치 보상, 유닛 구매 및 판매시 재화 변경
/// </summary>
public class GoldManager : Singleton<GoldManager>
{
    /// <summary>
    /// 몬스터 이름 열거형
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
    /// 현재 보유중인 재화
    /// </summary>
    private int _gold = 50;

    private List<ICashObserver> _goldObservers = new List<ICashObserver>();

    /// <summary>
    /// 메인 재화인 골드 프로퍼티
    /// 값이 0 아래가 되지 않도록 if문 사용
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
            NotifyChangeGold();
        }
    }

    /// <summary>
    /// 몬스터 처치 보상 정보
    /// </summary>
    private Dictionary<MonsterNameEnum, int> _monsterGold = new Dictionary<MonsterNameEnum, int>()
    {
        { MonsterNameEnum.Slime, 5 },
        { MonsterNameEnum.Turtle, 8 },
        { MonsterNameEnum.Mummy, 10 },
        { MonsterNameEnum.Ghost, 15 }
    };
    /// <summary>
    /// 유닛별 구매 비용 정보
    /// </summary>
    private Dictionary<UnitEnum, int> _unitGold = new Dictionary<UnitEnum, int>()
    {
        { UnitEnum.Warrior, 10 },
        { UnitEnum.Wizard, 15 }
    };



    /// <summary>
    /// 몬스터 처치 시 재화를 추가하는 메서드
    /// </summary>
    /// <param name="monsterName">사망한 몬스터의 이름</param>
    public void GoldAdd(MonsterNameEnum monsterName)
    {
        if (_monsterGold.TryGetValue(monsterName, out int reward))
        {
            SoundManager.Instance.PlayUiSound(UISoundClipEnum.Cash);
            Wallet += reward;
        }
        else
        {
            Debug.Log("등록되지 않은 몬스터");
        }
    }

    /// <summary>
    /// 유닛 구매시 메인 재화를 차감하는 메서드
    /// </summary>
    /// <param name="unitName">구매한 유닛 이름</param>
    public void UnitBuy(UnitEnum unitName)
    {
        if (_unitGold.TryGetValue(unitName, out int price))
        {
            Wallet -= price;
            Debug.Log($"{unitName} 구매");
        }
        else
        {
            Debug.Log("등록되지 않은 유닛");
        }
    }

    public bool IsPossibleUnitBuy(UnitEnum unitName)
    {
        if (_unitGold.TryGetValue(unitName, out int price))
        {
            return (Wallet - price) >= 0;
        }
        return false;
    }
    /// <summary>
    /// 유닛 판매시 메인 재화 추가
    /// 지금은 판매 금액은 원래 가격의 80%, 소수점은 반올림해서 계산
    /// </summary>
    /// <param name="unitName">판매할 유닛 이름</param>
    public void UnitSell(UnitEnum unitName)
    {
        if (_unitGold.TryGetValue(unitName, out int price))
        {
            int sellingPrice = Mathf.RoundToInt(price * 0.8f);
            Wallet += sellingPrice;
        }
    }

    /// <summary>
    /// 초기화시 호출
    /// </summary>
    protected override void init()
    {
        // 부모 init 호출
        base.init();
        //_gold = 100;
        //NotifyChangeGold();
    }

    /// <summary>
    /// 구독
    /// </summary>
    public void RegistrationObserver(ICashObserver cashObserver)
    {
        _goldObservers.Add(cashObserver);
    }

    /// <summary>
    /// 구독 해제
    /// </summary>
    public void UnregisterObserver(ICashObserver cashObserver)
    {
        _goldObservers.Remove(cashObserver);
    }
    /// <summary>
    /// 지갑이 바뀔 때마다 알림 역할
    /// </summary>
    private void NotifyChangeGold()
    {
        foreach (var cashObserver in _goldObservers)
        {
            cashObserver.NotifyChangeGold(Wallet);
        }
    }
}
