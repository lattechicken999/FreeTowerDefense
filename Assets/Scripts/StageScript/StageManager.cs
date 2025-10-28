using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : Singleton<StageManager>
{
    
    private int _stageNum = 1;
    public int StageNum
    {
        get { return _stageNum; }
        set { _stageNum = value; }
    }

    private int _sponNum;
    private int _sponDelay;
    //���� Ÿ��
    //�������� �ѹ��� ���յ� ���� Ÿ�Ժ� ��ȯ����
    //���� �Ը�(�迭??)
    //���� ����(������? �ź���??)

    private float _unitCastleHp;
    public float UnitCastleHp
    {
        get { return _unitCastleHp; }
        set { _unitCastleHp = value; }
    }

    /// <summary>
    /// �������� ���۰� ���� ����
    /// </summary>
    //���� ���� �Ը� 0�� �Ǹ�
    public void StageStart()
    {
        if(UnitCastleHp > 0)    //���� �� ü���� 0�ʰ���
        {
            StageSuccess(2);
        }
        else
        {
            StageFail();
        }
    }

    /// <summary>
    /// �������� �����ؼ� �޴���
    /// </summary>
    public void StageFail()
    {
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// �������� �����ؼ� ���� ����������
    /// </summary>
    public void StageSuccess(int stage)
    {
        if (StageNum < stage)
        {
            StageNum++;
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
    
}
