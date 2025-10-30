using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class UIHpBarMonster : MonoBehaviour
{
    [SerializeField] private float _gap = 1.0f;
    [SerializeField] Image _hpBarImage;
    Vector3 _hpBarPos;
    Camera _camera;
    Vector3 _gapPos;
    Monster _monsterInfo;


    private float _maxHp;
    private void Awake()
    {
        _camera = Camera.main;
        SetGapPos(_gap); //_gapPos ¼öÁ¤
        // _gapPos = Vector3.up * _gap;
    }
    public void SetUIPos(Monster monster)
    {
        //Debug.Log("SetUIPos");
        _monsterInfo = monster;
        _maxHp = _monsterInfo._Hp;
        _monsterInfo._hpValueChange += SetUIValue;
    }
    private void OnDestroy()
    {
        if(_monsterInfo != null)
        {
            _monsterInfo._hpValueChange -= SetUIValue;
        }
    }
    void Update()
    {
        //MoveHpVar();
    }

    private void MoveHpVar()
    {
        Vector3 movePos = _monsterInfo.transform.position + _gapPos;
        transform.position = _camera.WorldToScreenPoint(movePos);
    }

    private void SetUIValue(float currentHp)
    {
        Debug.Log("SetUIValue");
        float hpAmount = currentHp / _maxHp;
        _hpBarImage.fillAmount = hpAmount;
    }

    public void SetGapPos(float gapValue)
    {
        _gapPos = Vector3.forward * gapValue;
    }

}
