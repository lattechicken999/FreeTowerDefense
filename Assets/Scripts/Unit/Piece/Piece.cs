using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Piece : Unit
{
    [SerializeField,Range(0,2)] float _atteckCoolTime;

    WaitForSeconds _attackDelay;
    private void Awake()
    {
        _attackDelay = new WaitForSeconds(_atteckCoolTime);
    }

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
