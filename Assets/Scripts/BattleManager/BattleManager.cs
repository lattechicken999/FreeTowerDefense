using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    /// <summary>
    /// 테스트용 리스트 나중에 변경----------
    /// </summary>
    public List<GameObject> TestObjects;
    /// <summary>
    /// 가장 가까운 오브젝트
    /// </summary>
    public GameObject _shortEnemy = null;
    /// <summary>
    /// 가장 짧은 거리 측정용
    /// </summary>
    public float _shortDistance;

    /// <summary>
    /// 테스트용 사거리 삭제 예정----------
    /// </summary>
    public float _attackRange = 3f;


    /// <summary>
    /// 사거리 내에 가장 가까운 적을 찾는 메서드
    /// </summary>
    public GameObject Target()
    {
        // 테스트용 오브젝트 3개 생성-----------
        TestObjects = new List<GameObject>();
        GameObject object1 = new GameObject("Enemy1");
        GameObject object2 = new GameObject("Enemy2");
        GameObject object3 = new GameObject("Enemy3");

        // 테스트용 오브젝트 위치 설정------------
        object1.transform.position = new Vector3(0, 0, 0);
        object2.transform.position = new Vector3(1, 0, 0);
        object3.transform.position = new Vector3(2, 0, 0);

        // 테스트용 리스트에 추가-----------
        TestObjects.Add(object1);
        TestObjects.Add(object2);
        TestObjects.Add(object3);

        // 재사용시 문제를 없애기 위해 초기화
        _shortDistance = Mathf.Infinity;
        _shortEnemy = null;

        // 모두 둘러보고 가장가까운 곳 찾기
        foreach (GameObject obj in TestObjects)
        {
            float distance = Vector3.Distance(transform.position, obj.transform.position);

            // 더 작은 값을 찾으면 갱신
            if (distance < _shortDistance)
            {
                _shortDistance = distance;
                _shortEnemy = obj;
            }
        }
        // 사거리 보다 짧으면 
        if (_shortEnemy != null || _attackRange <= _shortDistance)
        {
            return _shortEnemy;
        }
        return null;
    }


    /// <summary>
    /// unit에서 공격을 요청했을때 사거리 내에 적이 있다면 데미지 계산
    /// </summary>
    /// <param name="unitAttack">유닛 공격력</param>
    /// <returns></returns>
    public int UnitAttack(int unitAttack)
    {
        Monster monster = _shortEnemy.GetComponent<Monster>();
        // 사거리 내에 적이 없을 때
        if (_shortEnemy == null)
        {
            return 0;
        }
        // 지금 몬스터는 실수형, 유닛은 정수형이라 일단 정수형으로 바꾸고 나중에 상의후 변경 예정
        int monsterDefense = (int)monster._DefensePoint;
        int damage = Damage(unitAttack, monsterDefense);
        
        return damage;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="attack">유닛 공격력</param>
    /// <param name="defense">몬스터 방어력</param>
    /// <returns></returns>
    public int Damage(int attack, int defense)
    {
        int damage = attack - defense;
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