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
    public GameObject Trarget()
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