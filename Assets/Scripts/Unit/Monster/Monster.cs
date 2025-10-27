using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Unit
{

    //Q2. Unit에서 hp와 attackPoint, defentse값은 어떻게 설정해?
    //A2. 몬스터 생성할때 프리팹? 이나 거기서 공격력 설정할듯? 일단해보기
    [field: SerializeField] public MonsterId _monsterId { get; private set; } //몬스터 아이디 (몬스터당 1개. 중복되지않고 고유해야하고, 몬스터 종류 추가될때마다 enum 추가 필요)
    [SerializeField] private float _reward = 1.0f; //유닛 처치시 현상금
    List<Vector3> _waypointsPosition = new List<Vector3>(); //웨이포인트들에 대한 정보
    int currentWaypoint = 0; //현재 웨이포인트가 몇번째인지, 하나씩 지날때마다 +1
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
        _hp -= dmg;
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

    }

    //HP변동시 UI에 알림 -> UI Manager에?
    //waypoint받아 이동 (Waypoint의 z방향으로 이동하는거임)

    private void Update()
    {
        //테스트용. 그냥 생성되면 앞으로 이동되게 한다 (몇개 생성됬는지 확인 위해서)
        transform.position += Vector3.forward * (Time.deltaTime * 3);
    }
}
