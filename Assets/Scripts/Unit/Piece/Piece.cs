using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Piece : Unit
{
    [SerializeField,Range(0,2)] float _atteckCoolTime;
    
    [SerializeField] UnitEnum _unitName;

    private WaitForSeconds _attackDelay;
    private Coroutine _autoCoroutine;
    private bool _isPlaced = false;

    protected virtual int GetPrice() => 0;
    protected GoldManager.UnitNameEnum _unitName;

    private void Awake()
    {
        _attackDelay = new WaitForSeconds(_atteckCoolTime);
    }

    /// <summary>
    /// Ȱ��ȭ �� �ڵ� ����
    /// </summary>
    private void OnEnable()
    {
        if(_autoCoroutine == null)
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
        while (true)
        {
            if (_isPlaced)
            {
                Attack();
            }

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
    public void SellPiece()
    {
       
        GoldManager goldManager = GoldManager.Instance;
        if (goldManager != null)
        {
            goldManager.UnitSell(_unitName);
        }
       

        // ���� ����
        Destroy(gameObject);
    }

    /// <summary>
    /// UI���� �⹰�� ���׷��̵� ��ư Ŭ���� ȣ��Ǵ� �޼���
    /// </summary>
    public void Upgrade()
    {

    }

    /// <summary>
    /// �⹰�� ��¥ ���� �Ǿ��� �� ȣ��Ǵ� �޼���
    /// </summary>
    //public void InifalPiece()
    //{
    //    _isPlaced = true;

    //    if (_autoCoroutine == null)
    //    {
    //        _autoCoroutine = StartCoroutine(AutoAttack());
    //    }
    //}


}
