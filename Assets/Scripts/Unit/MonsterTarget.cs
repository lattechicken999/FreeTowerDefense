using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTarget : Unit
{
    public event Action _gameFailNotify;
    public event Action<float> _monsterTargetHpChanged;
    /// <summary>
    /// ���� ���
    /// </summary>
    public override void Attack()
    {
        return;
    }

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

    private void Awake()
    {
        StageManager.Instance.RegisterStageFailEvent(this); //�̺�Ʈ ����
    }
    private void OnDestroy()
    {
        StageManager.Instance.UnRegisterStageFailEvent(this); //�̺�Ʈ ���� ����
    }
    /// <summary>
    /// UI�� HP�� �پ�� ���� ����
    /// </summary>
    private void HpNotify()
    {
        _monsterTargetHpChanged.Invoke(_hp);
    }

    /// <summary>
    /// ü���� 0�̵Ǿ� ������ ���� ���� �˸��� �Լ�
    /// </summary>
    private void GameFailNotify()
    {
        _gameFailNotify.Invoke();
    }


}
