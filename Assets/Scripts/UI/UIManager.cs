using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// �������� ���� �� HP ����, ���� ���� ������Ʈ
/// </summary>
public class UIManager : Singleton<UIManager>
{
    [SerializeField] GameObject _sellButtonPrefeb;

    protected override void Awake()
    {
        if (_instance == null)
        {
            _instance = this as UIManager;
            //UIManager�� �� ��ȯ�� �ʿ� ����
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
