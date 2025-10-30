using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    /// <summary>
    /// �׽�Ʈ�� ����Ʈ ���߿� ����----------
    /// </summary>
    public List<GameObject> TestObjects;
    /// <summary>
    /// ���� ����� ������Ʈ
    /// </summary>
    public GameObject _shortEnemy = null;
    /// <summary>
    /// ���� ª�� �Ÿ� ������
    /// </summary>
    public float _shortDistance;

    /// <summary>
    /// �׽�Ʈ�� ��Ÿ� ���� ����----------
    /// </summary>
    public float _attackRange = 3f;


    /// <summary>
    /// ��Ÿ� ���� ���� ����� ���� ã�� �޼���
    /// </summary>
    public GameObject Target()
    {
        // �׽�Ʈ�� ������Ʈ 3�� ����-----------
        TestObjects = new List<GameObject>();
        GameObject object1 = new GameObject("Enemy1");
        GameObject object2 = new GameObject("Enemy2");
        GameObject object3 = new GameObject("Enemy3");

        // �׽�Ʈ�� ������Ʈ ��ġ ����------------
        object1.transform.position = new Vector3(0, 0, 0);
        object2.transform.position = new Vector3(1, 0, 0);
        object3.transform.position = new Vector3(2, 0, 0);

        // �׽�Ʈ�� ����Ʈ�� �߰�-----------
        TestObjects.Add(object1);
        TestObjects.Add(object2);
        TestObjects.Add(object3);

        // ����� ������ ���ֱ� ���� �ʱ�ȭ
        _shortDistance = Mathf.Infinity;
        _shortEnemy = null;

        // ��� �ѷ����� ���尡��� �� ã��
        foreach (GameObject obj in TestObjects)
        {
            float distance = Vector3.Distance(transform.position, obj.transform.position);

            // �� ���� ���� ã���� ����
            if (distance < _shortDistance)
            {
                _shortDistance = distance;
                _shortEnemy = obj;
            }
        }
        // ��Ÿ� ���� ª���� 
        if (_shortEnemy != null || _attackRange <= _shortDistance)
        {
            return _shortEnemy;
        }
        return null;
    }


    /// <summary>
    /// unit���� ������ ��û������ ��Ÿ� ���� ���� �ִٸ� ������ ���
    /// </summary>
    /// <param name="unitAttack">���� ���ݷ�</param>
    /// <returns></returns>
    public int UnitAttack(int unitAttack)
    {
        Monster monster = _shortEnemy.GetComponent<Monster>();
        // ��Ÿ� ���� ���� ���� ��
        if (_shortEnemy == null)
        {
            return 0;
        }
        // ���� ���ʹ� �Ǽ���, ������ �������̶� �ϴ� ���������� �ٲٰ� ���߿� ������ ���� ����
        int monsterDefense = (int)monster._DefensePoint;
        int damage = Damage(unitAttack, monsterDefense);
        
        return damage;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="attack">���� ���ݷ�</param>
    /// <param name="defense">���� ����</param>
    /// <returns></returns>
    public int Damage(int attack, int defense)
    {
        int damage = attack - defense;
        return damage;
    }

    /// <summary>
    /// ���Ͱ� ������ ������ ��
    /// </summary>
    /// <param name="attack">���� ���ݷ�</param>
    /// <param name="defense">���� ����</param>
    /// <param name="hp">���� ���ݷ�</param>
    /// <returns></returns>
    public int MonsterAttack(int attack, int defense, int hp)
    {
        hp -= attack - defense;
        int wallHp = hp;

        return wallHp;
    }
}