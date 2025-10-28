using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Piece : Unit
{
    [SerializeField,Range(0,2)] float _atteckCoolTime;

    private WaitForSeconds _attackDelay;
    private Coroutine _autoCoroutine;
    private void Awake()
    {
        _attackDelay = new WaitForSeconds(_atteckCoolTime);
    }

    /// <summary>
    /// Ȱ��ȭ �� �ڵ� ����
    /// </summary>
    private void OnEnable()
    {
        _autoCoroutine = StartCoroutine(AutoAttack());
    }


    /// <summary>
    /// ��Ȱ��ȭ�� ���� ����
    /// </summary>
    private void OnDisable()
    {
        StopCoroutine(_autoCoroutine);
    }

    /// <summary>
    /// �ڵ����ݿ� �ڷ�ƾ �Լ�
    /// </summary>
    /// <returns></returns>
    IEnumerator AutoAttack()
    {
        while(true)
        {
            Attack();
            yield return _attackDelay;
        }
    }

    public override void Attack()
    {
        //��Ÿ��� ���� ���� Ư���� �޶� ���� Ŭ�������� ������ �ʿ�
    }
    public override void TakenDamage(float Damage)
    {

    }


    /// <summary>
    /// UI���� �Ǹ� Ŭ���� ȣ��Ǵ� �޼���
    /// </summary>
    public void SelPiece()
    {
        //GoldManager�� ��� ȣ��
    }

    /// <summary>
    /// UI���� �⹰�� ���׷��̵� ��ư Ŭ���� ȣ��Ǵ� �޼���
    /// </summary>
    public void Upgrade()
    {

    }

    /// <summary>
    /// UI���� �⹰ ���� ��ư Ŭ���� ȣ��Ǵ� �޼���
    /// Ŭ���� Map Ŭ�������� ���õ� �ڸ��� ����ִ��� Ȯ�� �ؾ���
    /// </summary>
    public void SetPosition()
    {

    }


}
