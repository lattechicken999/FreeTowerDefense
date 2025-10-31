using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTarget : Unit
{
    //���̺�Ʈ
    public event Action _gameFailNotify; //StageManager�� ����. ����HP 0�̵Ǹ� ����.
    public event Action<float> _monsterTargetHpChanged; //UIManager�� ����. ü���� �ٲ�� UIü�¹ٿ� ����

    [SerializeField] private float _hpInit = 5.0f;
    [SerializeField] private int _defenceInit;

    //���ӹ��� �ʵ�� Get�� �����ϵ��� ������Ƽ��
    public float Hp => _hp;
    public int DefensePoint => _defensePoint;
    /// <summary>
    /// ���� ���
    /// </summary>
    public override void Attack()
    {
        return;
    }
    /// <summary>
    /// UIManager�� ������ ������ ���� hp ����
    /// StageManager�� hp�� 0�̸� ���� ���� ����
    /// </summary>
    /// <param name="_currentHp"></param>
    public override void TakenDamage(float _currentHp)
    {
        //TakeDamage ������ ������ �Ű��������� ���� HP�� ����
        _hp = _currentHp;
        if(_hp <= 0)
        {
            _hp = 0;
            GameFailNotify();
        }
        HpNotify();
    }

    #region ���� �Ҵ� ����
    //���̺�Ʈ ���� �Ҵ�
    private void SubScribeEvent()
    {
        StageManager.Instance.SubscribeMonsterTargetEvent(this);
    }
    private void UnSubScribeEvent()
    {
        StageManager.Instance.UnSubscribeMonsterTargetEvent(this);
    }
    //���ʵ� ��� ���� �Ҵ�
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
        SubScribeEvent(); //�̺�Ʈ ������Ŵ (����)
        SetThisForMonsterManager(); //�ʵ� ���� (����)
    }
    private void OnDestroy()
    {
        UnSubScribeEvent();
        UnSetThisForMonsterManager();
    }
    /// <summary>
    /// UI�� HP�� �پ�� ���� ����
    /// </summary>
    private void HpNotify()
    {
        _monsterTargetHpChanged?.Invoke(_hp);
    }

    /// <summary>
    /// StageManager�� ü���� 0�̵Ǿ� ������ ���� ���� �˸��� �Լ�
    /// </summary>
    private void GameFailNotify()
    {
        _gameFailNotify.Invoke();
    }


}
