using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Monster : Unit
{

    //Q2. Unit���� hp�� attackPoint, defentse���� ��� ������? -> �����տ��� ���ִ°Ŷ� ���⼭ �����ϸ��
    //(�ƴѵ�? �ٵ� Player������ ���� ��������Ұ�� �����ǵ�_ �׳� �̷��� �ϴ°� ������?)�ٵ� SerializeField�� �ᵵ���� ���������� ���������(�׳� �������� ����.)
    [field: SerializeField] public MonsterId _monsterId { get; private set; } //���� ���̵� (���ʹ� 1��. �ߺ������ʰ� �����ؾ��ϰ�, ���� ���� �߰��ɶ����� enum �߰� �ʿ�)
    [SerializeField] private float _reward = 1.0f; //���� óġ�� �����]
    [SerializeField] public event Action<float> _hpValueChange;
    //Monster������ �ش簪�� Prefab���� ���� ����������ҰŰ��Ƽ� �߰�. Init()�޼��忡�� ��ӵȰ��� �ٽ� �־���
    [SerializeField] private float _initHp = 1.0f;
    [SerializeField] private int _initAttackPoint = 1;
    [SerializeField] private int _initDefensePoint = 1;

    //��������Ʈ ����
    private float _moveSpeed = 3.0f;
    [SerializeField] List<Vector3> _waypointsPosition = new List<Vector3>(); //��������Ʈ�鿡 ���� ����
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
    
    public override void Attack()
    {
        //������ �Ѵ�? ������ ?
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
    //Invoke�ΰ�? -> Enemy���� Invoke��(�Ķ���Ͱ� �־) -> UI���� UI�� Set�Ѵ�. �̰ǰ�?? //�׷� Enemy���� UI�� �˾ƾߵǴµ�? �³�?
    public void InvokeHPChange() //HP������ UI�� �˸� -> UI Manager��?
    {
        _hpValueChange.Invoke(_Hp);
    }

    //HP������ UI�� �˸� -> UI Manager��?
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
