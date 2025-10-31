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

    protected virtual int GetPrice() => 0;
    protected GoldManager.UnitNameEnum _unitName;

    private void Awake()
    {
        _attackDelay = new WaitForSeconds(_atteckCoolTime);
    }

    /// <summary>
    /// 활성화 시 자동 공격
    /// </summary>
    private void OnEnable()
    {
        if(_autoCoroutine == null)
            _autoCoroutine = StartCoroutine(AutoAttack());
    }


    /// <summary>
    /// 비활성화시 공격 중지
    /// </summary>
    private void OnDisable()
    {
        StopCoroutine(_autoCoroutine);
    }

    /// <summary>
    /// 자동공격용 코루틴 함수
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
        //사거리나 유닛 공격 특성이 달라 하위 클래스에서 재정의 필요
    }
    public override void TakenDamage(float Damage)
    {

    }


    /// <summary>
    /// UI에서 판매 클릭시 호출되는 메서드
    /// </summary>
    public void SellPiece()
    {
        /*
        if (GoldManager.Instance != null)
        {
            GoldManager.Instance.UnitSell(_unitName);
        }
        
        
        Destroy(gameObject);
        */
        // 골드 지급
        GoldManager goldManager = FindObjectOfType<GoldManager>();
        if (goldManager != null)
        {
            goldManager.UnitSell(_unitName);
        }
       

        // 유닛 제거
        Destroy(gameObject);
    }

    /// <summary>
    /// UI에서 기물의 업그레이드 버튼 클릭시 호출되는 메서드
    /// </summary>
    public void Upgrade()
    {

    }

    /// <summary>
    /// 기물이 진짜 생성 되었을 때 호출되는 메서드
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
