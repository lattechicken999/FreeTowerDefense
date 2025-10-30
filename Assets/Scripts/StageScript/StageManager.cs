using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : Singleton<StageManager>
{
    public IStageInfo _notifyStageInfoForUI; //UIManager 에서 스테이지 정보 얻어올수 있게 추가

    private int _stageNum = 1;
    public int StageNum
    {
        get { return _stageNum; }
        set { _stageNum = value; }
    }

    private int _sponNum;
    private int _sponDelay;
    //몬스터 타입
    //스테이지 넘버랑 결합된 몬스터 타입별 소환갯수
    //몬스터 규모(배열??)
    //몬스터 종류(슬라임? 거북이??)

    private float _unitCastleHp;
    public float UnitCastleHp
    {
        get { return _unitCastleHp; }
        set { _unitCastleHp = value; }
    }

    /// <summary>
    /// 알림 받을 객체 지정
    /// </summary>
    /// <param name="subscriber"></param>
    public void SubscribeStageInfo(IStageInfo subscriber)
    {
        _notifyStageInfoForUI = subscriber;
    }

    /// <summary>
    /// 알림받을 객체 해제
    /// </summary>
    public void UnsubscriberStageInfo()
    {
        _notifyStageInfoForUI = null;
    }
    /// <summary>
    /// 스테이지 시작과 성공 실패
    /// </summary>
    //만약 몬스터 규모가 0이 되면
    public void StageStart()
    {
        string msg = $"현재 스테이지: {StageNum}";
        _notifyStageInfoForUI?.NotifyStageInfo(msg); //UI에 스테이지 정보 알림

        if (UnitCastleHp > 0)    //만약 성 체력이 0초과면
        {
            StageSuccess(2);
        }
        else
        {
            StageFail();
        }
    }

    /// <summary>
    /// 스테이지 실패해서 메뉴로
    /// </summary>
    public void StageFail()
    {
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// 스테이지 성공해서 다음 스테이지로
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
