using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Unit
{

    //Q2. Unit���� hp�� attackPoint, defentse���� ��� ������?
    //A2. ���� �����Ҷ� ������? �̳� �ű⼭ ���ݷ� �����ҵ�? �ϴ��غ���
    [field: SerializeField] public MonsterId _monsterId { get; private set; } //���� ���̵� (���ʹ� 1��. �ߺ������ʰ� �����ؾ��ϰ�, ���� ���� �߰��ɶ����� enum �߰� �ʿ�)
    [SerializeField] private float _reward = 1.0f; //���� óġ�� �����
    List<Vector3> _waypointsPosition = new List<Vector3>(); //��������Ʈ�鿡 ���� ����
    int currentWaypoint = 0; //���� ��������Ʈ�� ���°����, �ϳ��� ���������� +1
    public override void Attack()
    {
        //������ �Ѵ�? ������ ?
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
        _hp -= dmg;
    }

    /// <summary>
    /// ����� ��ȯ (������ ��ȭ �߻� ���)
    /// </summary>
    /// <returns></returns>
    public float GetReward()
    {
        return _reward;
    }
    //Invoke�ΰ�? -> Enemy���� Invoke��(�Ķ���Ͱ� �־) -> UI���� UI�� Set�Ѵ�. �̰ǰ�?? //�׷� Enemy���� UI�� �˾ƾߵǴµ�? �³�?
    public void InvokeHPChange() //HP������ UI�� �˸� -> UI Manager��?
    {

    }

    //HP������ UI�� �˸� -> UI Manager��?
    //waypoint�޾� �̵� (Waypoint�� z�������� �̵��ϴ°���)

    private void Update()
    {
        //�׽�Ʈ��. �׳� �����Ǹ� ������ �̵��ǰ� �Ѵ� (� ��������� Ȯ�� ���ؼ�)
        transform.position += Vector3.forward * (Time.deltaTime * 3);
    }
}
