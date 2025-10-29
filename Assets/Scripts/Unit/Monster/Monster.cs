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

    //���ӹ��� hp,Attack,Defence�� [SerializeField]��� �߰�. Init()�޼��忡�� ��ӵȰ��� �ٽ� �־���
    [SerializeField] private float _initHp = 1.0f;
    [SerializeField] private int _initAttackPoint = 1;
    [SerializeField] private int _initDefensePoint = 1;

    //��������Ʈ ����
    private float _moveSpeed = 3.0f;
    private Vector3[] _wayPointPositions; //��������Ʈ�� Z����⸸ �˸�Ǵϱ� ȸ���� ��������
    //[SerializeField] List<Vector3> _waypointsPosition = new List<Vector3>(); //��������Ʈ�鿡 ���� ���� -> ���Ÿ� ��������Ʈ�ִ� �θ� ������Ʈ�� ã�Ƽ�? �������
    int _currentWaypoint = 0; //���� ��������Ʈ�� ���°����, �ϳ��� ���������� +1

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
    /// Hp�� 0�� �Ǿ����� Ȯ��, ���� ������ ������ �ı��Ѵ�
    /// </summary>
    private void CheckHpZero()
    {
        if (_Hp <= 0)
        {
            _monsterDeadNotified.Invoke(this);
            _monsterDeadNotifiedById.Invoke(_monsterId);
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
    private void Update()
    {
        //�׽�Ʈ��. �׳� �����Ǹ� ������ �̵��ǰ� �Ѵ� (� ��������� Ȯ�� ���ؼ�)

        //��������Ʈ�� ������ ��ǥ��� ������� �̵�
        if(_currentWaypoint < _wayPointPositions.Length)
        {
            Vector3 direction = (_wayPointPositions[_currentWaypoint] - transform.position).normalized;
            transform.position += direction * Time.deltaTime * _moveSpeed;
        }
        
    }
    /// <summary>
    /// MonserManager�κ��� ��������Ʈ�� �����޴� �޼���
    /// Position�� �ʿ��ϹǷ� ������ ��
    /// </summary>
    /// <param name="wayPoints">wayPoint����</param>
    public void SetWayPoints(List<Transform> wayPoints)
    {
        Debug.Log("SetWayPoints����");
        _wayPointPositions = new Vector3[wayPoints.Count];
        for(int index=0; index < wayPoints.Count; index++)
        {
            _wayPointPositions[index] = wayPoints[index].position; //��������Ʈ�� ȸ������ ����(Z�ุ ������)
        }
    }
    private void OnTriggerEnter(Collider other) //��������Ʈ �׽�Ʈ��... ������
    {
        if (other.gameObject.CompareTag("WayPoints"))
        {
            Debug.Log("Waypoint ����: " +other.name);
            if(_currentWaypoint < _wayPointPositions.Length-1)
            {
                _currentWaypoint++; //�̵��� ��ġ ����
            }
            else //��������Ʈ ������ ������ 
            {
                //���ݷ¸�ŭ ����HP���ҽ�Ű�����ؼ� ��Ʋ�Ŵ����� ��������
                Debug.Log("��������");
            }
            
        }
    }
}
