using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Monster : Unit
{
    [field: SerializeField] public MonsterId _monsterId { get; private set; } //���� ���̵� (���ʹ� 1��. �ߺ������ʰ� �����ؾ��ϰ�, ���� ���� �߰��ɶ����� enum �߰� �ʿ�)
    [SerializeField] private float _reward = 1.0f; //���� óġ�� �����]
    //���̺�Ʈ
    public event Action<float> _hpValueChange; //hp�� ����Ȱ� �˸��� ���ؼ�
    public event Action<Monster> _onDeath; //�׾��ٰ� �˸��� ���ؼ�

    //���ӹ��� hp,Attack,Defence�� [SerializeField]��� �߰�. Init()�޼��忡�� ��ӵȰ��� �ٽ� �־���
    [SerializeField] private float _initHp = 1.0f;
    [SerializeField] private int _initAttackPoint = 1;
    [SerializeField] private int _initDefensePoint = 1;

    //��������Ʈ ����
    private float _moveSpeed = 3.0f;
    [SerializeField] List<Vector3> _waypointsPosition = new List<Vector3>(); //��������Ʈ�鿡 ���� ���� -> ���Ÿ� ��������Ʈ�ִ� �θ� ������Ʈ�� ã�Ƽ�? �������
    int currentWaypoint = 0; //���� ��������Ʈ�� ���°����, �ϳ��� ���������� +1

    //Get�Ҽ��ִ� ������Ƽ�� ������ش�
    public float _Hp => _hp;
    public float _AttackPoint => _attackPoint;
    public float _DefensePoint => _defensePoint;

    private void Awake()
    {
        Init();
    }
    
    /// <summary>
    /// Monster������ �ش簪�� Prefab���� ���� ����������ҰŰ��Ƽ� �߰�. Init()�޼��忡�� ��ӵȰ��� �ٽ� �־���
    /// moveDirection(�ʱ� �̵�����) �� ����
    /// </summary>
    private void Init()
    {
        _hp = _initHp;
        _attackPoint = _initAttackPoint;
        _defensePoint = _initDefensePoint;
    }

    /// <summary>
    /// �����Ҷ� ȣ��Ǵ� �޼��� (��ӹ��� �޼���)
    /// </summary>
    public override void Attack()
    {
        //������ �Ѵ�? ���� Ÿ����
        //Ÿ�ٿ� ���� ������, ���� ���Ϳ� ���� ���ݷ��� MonsterManager�� ������
        //�׸��� MonsterManager�� BattleManager�� �ش� Ÿ�ٿ� ��������(���,ü��)�� ���� ���ݷ��� �Ѱ���
        //��Ʋ�Ŵ����� ����������ؼ� Ÿ�ٿ��ٰ� �Ѱ���(�̰� ��Ʋ�Ŵ��� �ȿ��� �ϱ⶧���� ���ʹ� �����ֱ⸸ �ϸ��)

        throw new System.NotImplementedException();
    }
    /// <summary>
    /// �������� ������ ȣ��Ǵ� �޼��� (��ӹ��� �޼���)
    /// </summary>
    /// <param name="dmg">����� ��������</param>
    public override void TakenDamage(float dmg)
    {
        float realDmg = dmg - _defensePoint;
        if (realDmg <= 0) realDmg = 0;
        _hp -= realDmg;
        InvokeHPChange();
    }

    /// <summary>
    /// ����� ��ȯ (������ ��ȭ �߻� ���)
    /// </summary>
    /// <returns></returns>
    public float GetReward()
    {
        return _reward;
    }
    /// <summary>
    /// Hp�� 0�� �Ǿ����� Ȯ��, ���� ������ ������ �ı��Ѵ�
    /// </summary>
    private void CheckHpZero() 
    {
        if(_Hp <= 0)
        {
            _onDeath.Invoke(this);
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Monster���� HP�� �����������Ƿ� HpUI�� �̺�Ʈ �߰��ϰ� ���⼭�� Invoke�� �����ش� (�Ű������� ������ HP���� �־���)
    /// </summary>
    public void InvokeHPChange() //HP������ UI�� �˸� -> UI Manager��?
    {
        _hpValueChange.Invoke(_Hp);
        CheckHpZero(); //hp�� 0�������� Ȯ��
    }
    //waypoint�޾� �̵� (Waypoint�� z�������� �̵��ϴ°���)
    private void Update()
    {
        //�׽�Ʈ��. �׳� �����Ǹ� ������ �̵��ǰ� �Ѵ� (� ��������� Ȯ�� ���ؼ�)
        transform.localPosition += transform.forward * (Time.deltaTime * _moveSpeed); //transform.forward  �ٶ󺸴� ���� ��ǥ
    }
    private void OnTriggerEnter(Collider other) //��������Ʈ �׽�Ʈ��... ������
    {
        if(other.gameObject.CompareTag("WayPoints"))
        {
            Debug.Log("Waypoint ����");
            Vector3 vec = other.gameObject.transform.position;
            gameObject.transform.rotation = other.gameObject.transform.rotation;
        }
    }
}
