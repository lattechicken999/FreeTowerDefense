using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTarget : Unit
{
    //▼이벤트
    public event Action _gameFailNotify; //StageManager에 전달. 성벽HP 0이되면 실패.
    public event Action<float> _monsterTargetHpChanged; //UIManager에 전달. 체력이 바뀌면 UI체력바에 전달

    [SerializeField] private float _hpInit = 5.0f;
    [SerializeField] private int _defenceInit;

    //▼상속받은 필드들 Get만 가능하도록 프로퍼티로
    public float Hp => _hp;
    public int DefensePoint => _defensePoint;
    /// <summary>
    /// 죽은 기능
    /// </summary>
    public override void Attack()
    {
        return;
    }
    /// <summary>
    /// UIManager에 데미지 받으면 변경 hp 전달
    /// StageManager에 hp가 0이면 게임 종료 전달
    /// </summary>
    /// <param name="_currentHp"></param>
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

    #region 동적 할당 영역
    //▼이벤트 동적 할당
    private void SubScribeEvent()
    {
        StageManager.Instance.SubscribeMonsterTargetEvent(this);
    }
    private void UnSubScribeEvent()
    {
        StageManager.Instance.UnSubscribeMonsterTargetEvent(this);
    }
    //▼필드 등록 동적 할당
    private void SetThisForMonsterManager()
    {
        MonsterManager.Instance.SetMonsterTargetInfo(this);
        StageManager.Instance.SetMonsterTargetInfo(this);
    }
    private void UnSetThisForMonsterManager()
    {
        MonsterManager.Instance.UnSetMonsterTargetInfo();
    }
    #endregion
    private void Init()
    {
        _hp = _hpInit;
        _defensePoint = _defenceInit;
    }
    private void Awake()
    {
        Init();
    }
    private void Start()
    {
        SubScribeEvent(); //이벤트 구독시킴 (동적)
        SetThisForMonsterManager(); //필드 설정 (동적)
    }
    private void OnDestroy()
    {
        UnSubScribeEvent();
        UnSetThisForMonsterManager();
    }
    /// <summary>
    /// UI에 HP가 줄어든 것을 보고
    /// </summary>
    private void HpNotify()
    {
        _monsterTargetHpChanged?.Invoke(_hp);
    }

    /// <summary>
    /// StageManager에 체력이 0이되어 게임이 종료 됨을 알리는 함수
    /// </summary>
    private void GameFailNotify()
    {
        _gameFailNotify.Invoke();
    }


}
