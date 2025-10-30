using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Monster : Unit
{
    [field: SerializeField] public GoldManager.MonsterNameEnum _monsterId { get; private set; } //���� ���̵� (���ʹ� 1��. �ߺ������ʰ� �����ؾ��ϰ�, ���� ���� �߰��ɶ����� enum �߰� �ʿ�)
    //���̺�Ʈ
    public event Action<float> _hpValueChange; //UIHpBarMonster.cs�� hp�� ����Ȱ� �˸��� ���ؼ�
    public event Action<Monster> _monsterDeadNotified; //MonsterManager.cs��  �׾��ٰ� �˸��� ���ؼ�
    public event Action<GoldManager.MonsterNameEnum> _monsterDeadNotifiedById; //GoldManager.cs�� ���� ���� ID�� ��ȭ�����ϱ� ���ؼ�
    public event Action<int> _monsterAttackAction; //MonsterManager.cs�� ���Ͱ� �����Ѵٴ� ���� �˸� (��������Ʈ ������ ���������� �ߵ�)

    //���ӹ��� hp,Attack,Defence�� [SerializeField]��� �߰�. Init()�޼��忡�� ��ӵȰ��� �ٽ� �־���
    [SerializeField] private float _initHp = 1.0f;
    [SerializeField] private int _initAttackPoint = 1;
    [SerializeField] private int _initDefensePoint = 1;
    //���̵��� ȸ�� �ӵ�
    [SerializeField] private float _rotationSpeed = 5.0f;

    //���������Ʈ ����
    private float _moveSpeed = 1.0f;
    private Vector3[] _wayPointPositions; //��������Ʈ�� Z����⸸ �˸�Ǵϱ� ȸ���� ��������
    private int _currentWaypoint = 1; //���� ��������Ʈ�� ���°����, �ϳ��� ���������� +1
    //����Ͱ� � ������� �׾����� (�÷��̾����� �׾����� ������ϴ� �� �������� �־���ؼ� �߰�)
    public bool _isKilledByPlayer { get; private set; } //MonsterManager���� �÷��̾�� �������� üũ�ϱ�����
    //����Ϳ� �پ��ִ� HP�� ���ӿ�����Ʈ (���Ͱ� ������ HP�ٵ� ���������ؼ� �߰�)
    private GameObject _hpBarGameObject;


    //Get�Ҽ��ִ� ������Ƽ�� ������ش�
    public float _Hp => _hp;
    public float _AttackPoint => _attackPoint;
    public float _DefensePoint => _defensePoint;

    private void OnEnable()
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
        _monsterAttackAction.Invoke(_attackPoint); //MonsterManager�� �̺�Ʈ ����. ���ݷ¸�ŭ Attack
    }
    public void SetHpBarObject(GameObject hpBar)
    {
        _hpBarGameObject = hpBar;
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
    /// Hp�� 0�� �Ǿ����� Ȯ��, ���� ������ ������ �ı��Ѵ�
    /// </summary>
    private void CheckHpZero()
    {
        if (_Hp <= 0)
        {
            _isKilledByPlayer = true;
            MonsterDeath();
        }
    }

    private void MonsterDeath() //���Ͱ� ������� ���� �޼���
    {
        _monsterDeadNotified?.Invoke(this);
        _monsterDeadNotifiedById?.Invoke(_monsterId);
        Destroy(_hpBarGameObject);
        Destroy(gameObject);
    }

    /// <summary>
    /// Monster���� HP�� �����������Ƿ� HpUI�� �̺�Ʈ �߰��ϰ� ���⼭�� Invoke�� �����ش� (�Ű������� ������ HP���� �־���)
    /// </summary>
    public void InvokeHPChange() //HP������ UI�� �˸� -> UI Manager��?
    {
        _hpValueChange.Invoke(_Hp);
        CheckHpZero(); //hp�� 0�������� Ȯ��
    }

    private void Update()
    {
        //�׽�Ʈ��. �׳� �����Ǹ� ������ �̵��ǰ� �Ѵ� (� ��������� Ȯ�� ���ؼ�)

        //��������Ʈ�� ������ ��ǥ��� ������� �̵�

        //�Ÿ��� �Ǵ��ϱ�� ����. 
        float _distance = 0.1f;
        Vector3 direction = (_wayPointPositions[_currentWaypoint] - transform.position).normalized;
        transform.position += direction * Time.deltaTime * _moveSpeed;

        //������Ʈ�� �ش� ������ �ٶ󺸰� ������ �Ѵ�. ������
        Vector3 direction2 = (_wayPointPositions[_currentWaypoint] - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction2), Time.deltaTime * _rotationSpeed);

        var diff = Vector3.Distance(transform.position, _wayPointPositions[_currentWaypoint]);
        if (diff < _distance)
        {
            _currentWaypoint++;
        }
        if (_currentWaypoint > _wayPointPositions.Length - 1) //��������Ʈ ������ ������ 
        {
            //���ݷ¸�ŭ ����HP���ҽ�Ű�����ؼ� ��Ʋ�Ŵ����� ��������
            Debug.Log("��������");
            _isKilledByPlayer = false;
            Attack();
            MonsterDeath();

        }
    }
    /// <summary>
    /// MonserManager�κ��� ��������Ʈ�� �����޴� �޼���
    /// Position�� �ʿ��ϹǷ� ������ ��
    /// </summary>
    /// <param name="wayPoints">wayPoint����</param>
    public void SetWayPoints(List<Transform> wayPoints)
    {
        _wayPointPositions = new Vector3[wayPoints.Count];
        for (int index = 0; index < wayPoints.Count; index++)
        {
            _wayPointPositions[index] = wayPoints[index].position; //��������Ʈ�� ȸ������ ����(Z�ุ ������)
        }
    }

}
