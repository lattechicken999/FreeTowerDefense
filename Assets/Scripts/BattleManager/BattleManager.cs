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
    public GameObject Trarget()
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

    public int UintAttack(string unitName, string monsterName)
    {


        //GameObject target = Trarget();
        //if (target != null)
        //{
        //    int damage = Damage(attackPower, defensePower);
        //    return damage;
        //}
        return 0;
    }

    public int Damage(int attack, int defense)
    {
        int damage = attack - defense;
        return damage;
    }

    public void MonsterAttack()
    {

    }
}