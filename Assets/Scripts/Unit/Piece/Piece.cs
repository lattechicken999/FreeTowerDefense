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
    /// 활성화 시 자동 공격
    /// </summary>
    private void OnEnable()
    {
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
        while(true)
        {
            Attack();
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
    public void SelPiece()
    {
        //GoldManager의 기능 호출
    }

    /// <summary>
    /// UI에서 기물의 업그레이드 버튼 클릭시 호출되는 메서드
    /// </summary>
    public void Upgrade()
    {

    }

    /// <summary>
    /// UI에서 기물 구매 버튼 클릭시 호출되는 메서드
    /// 클릭시 Map 클래스에서 선택된 자리가 비어있는지 확인 해야함
    /// </summary>
    public void SetPosition()
    {

    }


}
