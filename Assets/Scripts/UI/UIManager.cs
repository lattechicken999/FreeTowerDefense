using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 스테이지 정보 및 HP 정보, 지갑 정보 업데이트
/// </summary>
public class UIManager : Singleton<UIManager>
{
    [SerializeField] GameObject _sellButtonPrefeb;

    protected override void Awake()
    {
        if (_instance == null)
        {
            _instance = this as UIManager;
            //UIManager는 씬 전환시 필요 없음
            //DontDestroyOnLoad(gameObject);
            init();
        }
        else
        {
            if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

}
