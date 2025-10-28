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
