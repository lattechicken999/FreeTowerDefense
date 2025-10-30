using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    /// <summary>
    /// ���� ������ �����ϴ� ����Ʈ
    /// </summary>
    public List<GameObject> MonsterObjects;
    /// <summary>
    /// ���� ����� ������Ʈ
    /// </summary>
    public GameObject _shortEnemy = null;
    /// <summary>
    /// ���� ª�� �Ÿ� ������
    /// </summary>
    public float _shortDistance;

    // �׽�Ʈ�� ��Ÿ� ���� ����
    #region
    //public float _attackRange = 3f;
    #endregion

    /// <summary>
    /// ���� �Ŵ��� ����
    /// </summary>
    private void OnEnable()
    {
        MonsterManager.Instance._notifiedMonsterMake += OnMonstersUpdated;
    }

    /// <summary>
    ///  ���� �Ŵ��� ���� ����
    /// </summary>
    private void OnDisable()
    {
        MonsterManager.Instance._notifiedMonsterMake -= OnMonstersUpdated;
    }

    /// <summary>
    /// �������� ���� ����Ʈ�� �޾ƿͼ� ����
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
    /// ��Ÿ� ���� ���� ����� ���� ã�� �޼���
    /// </summary>
    /// <param name="range">���� ��Ÿ�</param>
    /// <returns></returns>
    public GameObject Target(float range)
    {
        // �׽�Ʈ�� ���� ����Ʈ ���� ���� �ּ� ó�� �ص�
        #region
        //// �׽�Ʈ�� ������Ʈ 3�� ����-----------
        //MonsterObjects = new List<GameObject>();
        //GameObject object1 = new GameObject("Enemy1");
        //GameObject object2 = new GameObject("Enemy2");
        //GameObject object3 = new GameObject("Enemy3");

        //// �׽�Ʈ�� ������Ʈ ��ġ ����------------
        //object1.transform.position = new Vector3(0, 0, 0);
        //object2.transform.position = new Vector3(1, 0, 0);
        //object3.transform.position = new Vector3(2, 0, 0);

        //// �׽�Ʈ�� ����Ʈ�� �߰�-----------
        //MonsterObjects.Add(object1);
        //MonsterObjects.Add(object2);
        //MonsterObjects.Add(object3);
        #endregion

        // ����� ������ ���ֱ� ���� �ʱ�ȭ
        _shortDistance = Mathf.Infinity;
        _shortEnemy = null;

        // ��� �ѷ����� ���尡��� �� ã��
        foreach (GameObject obj in MonsterObjects)
        {
            // �����°� ������
            if (obj == null)
            {
                continue;
            }
            float distance = Vector3.Distance(transform.position, obj.transform.position);

            // �� ���� ���� ã���� ����
            if (distance < _shortDistance)
            {
                _shortDistance = distance;
                _shortEnemy = obj;
            }
        }
        // ��Ÿ� ���� ª���� 
        if (_shortEnemy != null && range <= _shortDistance)
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
        // ��Ÿ� ���� ���� ���� ��
        if (_shortEnemy == null)
        {
            return 0;
        }
        Monster monster = _shortEnemy.GetComponent<Monster>();
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