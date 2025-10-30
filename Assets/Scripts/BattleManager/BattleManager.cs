using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : Singleton<BattleManager>
{
    /// <summary>
    /// 몬스터 정보를 저장하는 리스트
    /// </summary>
    public List<GameObject> MonsterObjects;
    /// <summary>
    /// 가장 가까운 오브젝트
    /// </summary>
    public GameObject _shortEnemy = null;
    /// <summary>
    /// 가장 짧은 거리 측정용
    /// </summary>
    public float _shortDistance;

    // 테스트용 사거리 삭제 예정
    #region
    //public float _attackRange = 3f;
    #endregion

    /// <summary>
    /// 몬스터 매니저 구독
    /// </summary>
    private void OnEnable()
    {
        MonsterManager.Instance._notifiedMonsterMake += OnMonstersUpdated;
    }

    /// <summary>
    ///  몬스터 매니저 구독 해제
    /// </summary>
    private void OnDisable()
    {
        MonsterManager.Instance._notifiedMonsterMake -= OnMonstersUpdated;
    }

    /// <summary>
    /// 구독에서 몬스터 리스트를 받아와서 저장
    /// </summary>
    /// <param name="aliveMonsters"></param>
    private void OnMonstersUpdated(List<Monster> addMonster)
    {
        MonsterObjects = new List<GameObject>();
        foreach (var  monster in addMonster)
        {
            if (monster != null)
            {
                MonsterObjects.Add(monster.gameObject);
            }
        }
    }


    /// <summary>
    /// 사거리 내에 가장 가까운 적을 찾는 메서드
    /// </summary>
    /// <param name="range">유닛 사거리</param>
    /// <returns></returns>
    public GameObject Target(float range)
    {
        // 테스트용 몬스터 리스트 삭제 예정 주석 처리 해둠
        #region
        //// 테스트용 오브젝트 3개 생성-----------
        //MonsterObjects = new List<GameObject>();
        //GameObject object1 = new GameObject("Enemy1");
        //GameObject object2 = new GameObject("Enemy2");
        //GameObject object3 = new GameObject("Enemy3");

        //// 테스트용 오브젝트 위치 설정------------
        //object1.transform.position = new Vector3(0, 0, 0);
        //object2.transform.position = new Vector3(1, 0, 0);
        //object3.transform.position = new Vector3(2, 0, 0);

        //// 테스트용 리스트에 추가-----------
        //MonsterObjects.Add(object1);
        //MonsterObjects.Add(object2);
        //MonsterObjects.Add(object3);
        #endregion

        // 재사용시 문제를 없애기 위해 초기화
        _shortDistance = Mathf.Infinity;
        _shortEnemy = null;

        // 모두 둘러보고 가장가까운 곳 찾기
        foreach (GameObject obj in MonsterObjects)
        {
            // 터지는거 방지용
            if (obj == null)
            {
                continue;
            }
            float distance = Vector3.Distance(transform.position, obj.transform.position);

            // 더 작은 값을 찾으면 갱신
            if (distance < _shortDistance)
            {
                _shortDistance = distance;
                _shortEnemy = obj;
            }
        }
        // 사거리 보다 짧으면 
        if (_shortEnemy != null && range <= _shortDistance)
        {
            return _shortEnemy;
        }
        return null;
    }


    /// <summary>
    /// unit에서 공격을 요청했을때 사거리 내에 적이 있다면 데미지 계산
    /// </summary>
    /// <param name="unitAttack">유닛 공격력</param>
    /// <param name="range">유닛 사거리</param>
    public void UnitAttack(float unitAttack, float range)
    {
        GameObject target = Target(range);
        // 사거리 내에 적이 없을 때
        if (target == null)
        {
            return;
        }
        Monster monster = target.GetComponent<Monster>();

        if (monster == null)
        {
            return;
        }
        // 지금 몬스터는 실수형, 유닛은 정수형이라 일단 정수형으로 바꾸고 나중에 상의후 변경 예정
        float monsterDefense = monster._DefensePoint;
        float damage = Damage(unitAttack, monsterDefense);

        monster.TakenDamage(damage);
    }

    /// <summary>
    /// 데미지 계산 로직
    /// </summary>
    /// <param name="attack">유닛 공격력</param>
    /// <param name="defense">몬스터 방어력</param>
    /// <returns></returns>
    public float Damage(float attack, float defense)
    {
        float damage = attack - defense;
        return damage;
    }

    /// <summary>
    /// 몬스터가 성벽을 공격할 때
    /// </summary>
    /// <param name="attack">몬스터 공격력</param>
    /// <param name="defense">성벽 방어력</param>
    /// <param name="hp">성벽 공격력</param>
    /// <returns></returns>
    public int MonsterAttack(int attack, int defense, int hp)
    {
        hp -= attack - defense;
        int wallHp = hp;

        return wallHp;
    }
}