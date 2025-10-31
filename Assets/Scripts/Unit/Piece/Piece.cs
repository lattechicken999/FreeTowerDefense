using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Piece : Unit
{
    [SerializeField,Range(0,2)] float _atteckCoolTime;
    
    private WaitForSeconds _attackDelay;
    private Coroutine _autoCoroutine;
    private bool _isPlaced = false;
    private Ray _ray;

    protected GameObject targetMonster;
    protected virtual int GetPrice() => 0;
    protected UnitEnum _unitName;

    private void Awake()
    {
        _attackDelay = new WaitForSeconds(_atteckCoolTime);
        targetMonster = null;
    }

    protected virtual void Start()
    {
        GoldManager.Instance.UnitBuy(_unitName);
    }
    /// <summary>
    /// Ȱ��ȭ �� �ڵ� ����
    /// </summary>
    private void OnEnable()
    {
        
        _isPlaced = true;
        if (_autoCoroutine == null)
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

        GoldManager.Instance.UnitSell(_unitName);

        // ���� ����
        Destroy(gameObject);
    }

    private void Update()
    {
        if(targetMonster  != null)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation,
                                                                   Quaternion.LookRotation(targetMonster.transform.position - transform.position),
                                                                   Time.deltaTime * 2f);
        }
    }
}
