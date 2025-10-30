using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : Singleton<StageManager>, IMonsterCount, IMonsterWaveEnd
{
    [SerializeField] private List<StageDataSO> _stageDataList;
    private List<GoldManager.MonsterNameEnum> _stageMonsterList;
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

    protected override void Awake()
    {
        base.Awake(); //부모 Awake호출 (싱글톤 객체인지 확인 위해)

        //시작하자마자 스테이지 1번값 가져오고, 몬스터 매니저에 해당 정보 Set
        _stageMonsterList = GetStageData(_stageNum);
        MonsterManager.Instance.SubScribeMonsterCount(this);
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
        
        MonsterManager.Instance?.StartMonsterRun();
    }
    private void Start()
    {
        SetMonsterManagerMonsterList(); //몬스터 매니저에 넣기
        StageStart();
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

    /// <summary>
    /// ScriptableObject에 담겨있는 스테이지 Index와 맞는것을 찾아서 가져옴
    /// </summary>
    /// <param name="stageIndex">스테이지 번호</param>
    /// <returns>스테이지의 몬스터 타입 리스트 반환</returns>
    public List<GoldManager.MonsterNameEnum> GetStageData(int stageIndex)
    {
        foreach(var item in _stageDataList)
        {
            if(item.stageNumber == stageIndex)
            {
                _sponDelay = item.spawnDelay;
                return item.monsterInfoList; //찾으면 찾은스테이지 보냄
            }
        }
        return new List<GoldManager.MonsterNameEnum>(); //못찾으면 새로 생성해서 보냄
    }
    public void SetMonsterManagerMonsterList()
    {
        _sponNum = _stageMonsterList.Count;
        MonsterManager._instance?.SetMonstersFromStageManager(_sponNum, _stageMonsterList, _sponDelay);
    }

    public void NotifieyRemainMonsterCount(int count)
    {
        string msg = $"현재 스테이지: {StageNum}, 남은 몬스터: {count}";
        _notifyStageInfoForUI?.NotifyStageInfo(msg); //UI에 스테이지 정보 알림
    }

    public void RegisterStageFailEvent(MonsterTarget monsterTarget)
    {
        monsterTarget._gameFailNotify += StageManager.Instance.StageFail;
    }
    public void UnRegisterStageFailEvent(MonsterTarget monsterTarget)
    {
        monsterTarget._gameFailNotify -= StageManager.Instance.StageFail;
    }
    private void CheckStageClear()
    {
        /*
        
        if (UnitCastleHp > 0)    //만약 성 체력이 0초과면
        {
            StageSuccess(2);
        }
        else
        {
            StageFail();
        }
        */
    }

    public void MonsterWaveEnd()
    {
        
    }
}
