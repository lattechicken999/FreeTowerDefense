using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Monster : Unit
{
    [field: SerializeField] public GoldManager.MonsterNameEnum _monsterId { get; private set; } //몬스터 아이디 (몬스터당 1개. 중복되지않고 고유해야하고, 몬스터 종류 추가될때마다 enum 추가 필요)
    //▼이벤트
    public event Action<float> _hpValueChange; //UIHpBarMonster.cs에 hp가 변경된걸 알리기 위해서
    public event Action<Monster> _monsterDeadNotified; //MonsterManager.cs와  죽었다고 알리기 위해서
    public event Action<GoldManager.MonsterNameEnum> _monsterDeadNotifiedById; //GoldManager.cs에 죽은 몬스터 ID로 재화관리하기 위해서
    public event Action<int> _monsterAttackAction; //MonsterManager.cs에 몬스터가 공격한다는 것을 알림 (웨이포인트 끝지점 도달했을때 발동)

    //▼상속받은 hp,Attack,Defence에 [SerializeField]없어서 추가. Init()메서드에서 상속된곳에 다시 넣어줌
    [SerializeField] private float _initHp = 1.0f;
    [SerializeField] private int _initAttackPoint = 1;
    [SerializeField] private int _initDefensePoint = 1;
    //▼이동시 회전 속도
    [SerializeField] private float _rotationSpeed = 5.0f;

    //▼웨이포인트 설정
    private float _moveSpeed = 1.0f;
    private Vector3[] _wayPointPositions; //웨이포인트는 Z축방향만 알면되니까 회전을 저장하자
    private int _currentWaypoint = 1; //현재 웨이포인트가 몇번째인지, 하나씩 지날때마다 +1
    //▼몬스터가 어떤 방식으로 죽었는지 (플레이어한테 죽었으면 돈줘야하는 등 차별점이 있어야해서 추가)
    public bool _isKilledByPlayer { get; private set; } //MonsterManager에서 플레이어에서 죽은건지 체크하기위해
    //▼몬스터에 붙어있는 HP바 게임오브젝트 (몬스터가 죽으면 HP바도 없어져야해서 추가)
    private GameObject _hpBarGameObject;


    //Get할수있는 프로퍼티도 만들어준다
    public float _Hp => _hp;
    public float _AttackPoint => _attackPoint;
    public float _DefensePoint => _defensePoint;

    private void OnEnable()
    {
        Init();
    }

    /// <summary>
    /// Monster에서는 해당값을 Prefab에서 각각 설정해줘야할거같아서 추가. Init()메서드에서 상속된곳에 다시 넣어줌
    /// moveDirection(초기 이동방향) 도 설정
    /// </summary>
    private void Init()
    {
        _hp = _initHp;
        _attackPoint = _initAttackPoint;
        _defensePoint = _initDefensePoint;
    }

    /// <summary>
    /// 공격할때 호출되는 메서드 (상속받은 메서드)
    /// </summary>
    public override void Attack()
    {
        _monsterAttackAction.Invoke(_attackPoint); //MonsterManager이 이벤트 구독. 공격력만큼 Attack
    }
    public void SetHpBarObject(GameObject hpBar)
    {
        _hpBarGameObject = hpBar;
    }

    /// <summary>
    /// 데미지를 받을때 호출되는 메서드 (상속받은 메서드)
    /// </summary>
    /// <param name="dmg">계산할 데미지값</param>
    public override void TakenDamage(float dmg)
    {
        float realDmg = dmg - _defensePoint;
        if (realDmg <= 0) realDmg = 0;
        _hp -= realDmg;
        InvokeHPChange();
    }

    /// <summary>
    /// Hp가 0이 되었는지 확인, 이후 스스로 본인을 파괴한다
    /// </summary>
    private void CheckHpZero()
    {
        if (_Hp <= 0)
        {
            _isKilledByPlayer = true;
            MonsterDeath();
        }
    }

    private void MonsterDeath() //몬스터가 죽을경우 통합 메서드
    {
        _monsterDeadNotified?.Invoke(this);
        _monsterDeadNotifiedById?.Invoke(_monsterId);
        Destroy(_hpBarGameObject);
        Destroy(gameObject);
    }

    /// <summary>
    /// Monster마다 HP를 가지고있으므로 HpUI에 이벤트 추가하고 여기서는 Invoke만 시켜준다 (매개변수로 변동된 HP값을 넣어줌)
    /// </summary>
    public void InvokeHPChange() //HP변동시 UI에 알림 -> UI Manager에?
    {
        _hpValueChange.Invoke(_Hp);
        CheckHpZero(); //hp가 0이하인지 확인
    }

    private void Update()
    {
        //테스트용. 그냥 생성되면 앞으로 이동되게 한다 (몇개 생성됬는지 확인 위해서)

        //웨이포인트에 설정된 좌표대로 순서대로 이동

        //거리로 판단하기로 결정. 
        float _distance = 0.1f;
        Vector3 direction = (_wayPointPositions[_currentWaypoint] - transform.position).normalized;
        transform.position += direction * Time.deltaTime * _moveSpeed;

        //오브젝트가 해당 방향을 바라보게 만들어야 한다. 서서히
        Vector3 direction2 = (_wayPointPositions[_currentWaypoint] - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction2), Time.deltaTime * _rotationSpeed);

        var diff = Vector3.Distance(transform.position, _wayPointPositions[_currentWaypoint]);
        if (diff < _distance)
        {
            _currentWaypoint++;
        }
        if (_currentWaypoint > _wayPointPositions.Length - 1) //웨이포인트 끝까지 왔으면 
        {
            //공격력만큼 성문HP감소시키기위해서 배틀매니저에 보내야함
            Debug.Log("성문도달");
            _isKilledByPlayer = false;
            Attack();
            MonsterDeath();

        }
    }
    /// <summary>
    /// MonserManager로부터 웨이포인트를 설정받는 메서드
    /// Position만 필요하므로 꺼내서 씀
    /// </summary>
    /// <param name="wayPoints">wayPoint정보</param>
    public void SetWayPoints(List<Transform> wayPoints)
    {
        _wayPointPositions = new Vector3[wayPoints.Count];
        for (int index = 0; index < wayPoints.Count; index++)
        {
            _wayPointPositions[index] = wayPoints[index].position; //웨이포인트의 회전정보 저장(Z축만 쓸거임)
        }
    }

}
