using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Monster : Unit
{

    //Q2. Unit에서 hp와 attackPoint, defentse값은 어떻게 설정해? -> 프리팹에서 해주는거라서 여기서 설정하면됨
    //(아닌듯? 근데 Player에서는 설정 안해줘야할경우 있을건데_ 그냥 이렇게 하는게 나을듯?)근데 SerializeField로 써도될지 종수님한테 물어봐야함(그냥 쓰지말고 하자.)
    [field: SerializeField] public MonsterId _monsterId { get; private set; } //몬스터 아이디 (몬스터당 1개. 중복되지않고 고유해야하고, 몬스터 종류 추가될때마다 enum 추가 필요)
    [SerializeField] private float _reward = 1.0f; //유닛 처치시 현상금]
    [SerializeField] public event Action<float> _hpValueChange;
    //Monster에서는 해당값을 Prefab에서 각각 설정해줘야할거같아서 추가. Init()메서드에서 상속된곳에 다시 넣어줌
    [SerializeField] private float _initHp = 1.0f;
    [SerializeField] private int _initAttackPoint = 1;
    [SerializeField] private int _initDefensePoint = 1;

    //웨이포인트 설정
    private float _moveSpeed = 3.0f;
    [SerializeField] List<Vector3> _waypointsPosition = new List<Vector3>(); //웨이포인트들에 대한 정보
    int currentWaypoint = 0; //현재 웨이포인트가 몇번째인지, 하나씩 지날때마다 +1

    //Get할수있는 프로퍼티도 만들어준다
    public float _Hp => _hp;
    public float _AttackPoint => _attackPoint;
    public float _DefensePoint => _defensePoint;

    private void Awake()
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
    
    public override void Attack()
    {
        //공격을 한다? 성문을 ?
        throw new System.NotImplementedException();
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
    /// 현상금 반환 (죽으면 재화 발생 기능)
    /// </summary>
    /// <returns></returns>
    public float GetReward()
    {
        return _reward;
    }
    //Invoke인가? -> Enemy에서 Invoke해(파라미터값 넣어서) -> UI에서 UI를 Set한다. 이건가?? //그럼 Enemy에서 UI를 알아야되는데? 맞나?
    public void InvokeHPChange() //HP변동시 UI에 알림 -> UI Manager에?
    {
        _hpValueChange.Invoke(_Hp);
    }

    //HP변동시 UI에 알림 -> UI Manager에?
    //waypoint받아 이동 (Waypoint의 z방향으로 이동하는거임)

    private void Update()
    {
        //테스트용. 그냥 생성되면 앞으로 이동되게 한다 (몇개 생성됬는지 확인 위해서)
        transform.localPosition += transform.forward * (Time.deltaTime * _moveSpeed); //transform.forward  바라보는 방향 좌표
    }
    private void OnTriggerEnter(Collider other) //웨이포인트 테스트용... 가보자
    {
        if(other.gameObject.CompareTag("WayPoints"))
        {
            Debug.Log("Waypoint 들어옴");
            Vector3 vec = other.gameObject.transform.position;
            gameObject.transform.rotation = other.gameObject.transform.rotation;
        }
    }
}
