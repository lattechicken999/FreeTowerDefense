using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTarget : Unit
{
    public event Action _gameFailNotify;
    public event Action<float> _monsterTargetHpChanged;
    /// <summary>
    /// 죽은 기능
    /// </summary>
    public override void Attack()
    {
        return;
    }

    public override void TakenDamage(float _currentHp)
    {
        //TakeDamage 이지만 실제로 매개변수에는 현재 HP가 들어옴
        _hp = _currentHp;
        if(_hp <= 0)
        {
            _hp = 0;
            GameFailNotify();
        }
        HpNotify();
    }

    private void Awake()
    {
        StageManager.Instance.RegisterStageFailEvent(this); //이벤트 구독
    }
    private void OnDestroy()
    {
        StageManager.Instance.UnRegisterStageFailEvent(this); //이벤트 구독 해제
    }
    /// <summary>
    /// UI에 HP가 줄어든 것을 보고
    /// </summary>
    private void HpNotify()
    {
        _monsterTargetHpChanged.Invoke(_hp);
    }

    /// <summary>
    /// 체력이 0이되어 게임이 종료 됨을 알리는 함수
    /// </summary>
    private void GameFailNotify()
    {
        _gameFailNotify.Invoke();
    }


}
