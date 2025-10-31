using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : Singleton<StageManager>, IMonsterCount
{
    [SerializeField] private List<StageDataSO> _stageDataList; //스테이지 데이터 리스트 (ScriptableObject)
    [SerializeField] private int _startNextStageDelay = 3; //코루틴으로 사용하는 딜레이 
    private MonsterTarget _monsterTarget;   //몬스터가 도착지점에 도달해서 공격하는 대상
    private List<GoldManager.MonsterNameEnum> _stageMonsterList; //Stage에서 소환할 수 있는 몬스터 스크립트에서 불러옴
    public IStageInfo _notifyStageInfoForUI; //UIManager 에서 스테이지 정보 얻어올수 있게 추가
    private bool _isSpawnFinished; //스테이지 성공 여부
    private WaitForSeconds _stageStartDelay; //다음 스테이지 진행 시 지연시간 설정 위해서 선언

    private int _stageNum = 1; //현재 Stage 번호
    public int StageNum
    {
        get { return _stageNum; }
        set { _stageNum = value; }
    }

    private int _sponNum; //몬스터 소환 갯수
    private int _sponDelay; //몬스터 소환 지연시간
    private int _spawnRemainCount; // 현재 stage에 남은 몬스터 개수
    private int _deathMonsterCount = -1; // 현재 stage에서 죽은 몬스터 개수

    protected override void Awake()
    {
        base.Awake(); //부모 Awake호출 (싱글톤 객체인지 확인 위해)

        //시작하자마자 스테이지 1번값 가져오고, 몬스터 매니저에 해당 정보 Set
        _stageStartDelay = new WaitForSeconds(_startNextStageDelay);
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
    /// monsterTarget에서 사용하는 구독 설정 이벤트
    /// </summary>
    public void SubscribeMonsterTargetEvent(MonsterTarget monsterTarget)
    {
        monsterTarget._gameFailNotify += StageFail;
    }
    /// <summary>
    /// monsterTarget에서 사용하는 구독 설정 이벤트
    /// </summary>
    public void UnSubscribeMonsterTargetEvent(MonsterTarget monsterTarget)
    {
        monsterTarget._gameFailNotify -= StageFail;
    }
    public void SetMonsterTargetInfo(MonsterTarget target)
    {
        _monsterTarget = target;
    }

    /// <summary>
    /// 스테이지 시작과 성공 실패
    /// </summary>
    //만약 몬스터 규모가 0이 되면
    public void StageStart()
    {
        _isSpawnFinished = false;
        MonsterManager.Instance?.StartMonsterRun();
    }
    private void Start()
    {
        MonsterManager.Instance.SubScribeMonsterCount(this);
        MonsterManager.Instance._notifyAllMonsterSpawn += AllMonsterSpawned;
        SetStageData();
        //StageStart();
    }
    private void SetStageData()
    {
        _stageMonsterList = GetStageData(StageNum);
        SetMonsterManagerMonsterList(); //몬스터 매니저에 넣기

    }
    private void OnDisable()
    {
        if (Instance == this)
        {
            Debug.Log("StageManager Singleton 인스턴스에서 OnDestroy호출됨");
            MonsterManager.Instance?.UnSubScribeMonsterCount();
            MonsterManager.Instance._notifyAllMonsterSpawn -= AllMonsterSpawned;
        }
    }

    /// <summary>
    /// 몬스터 매니저에서 모든 몬스터가 소환된 상태인지 확인
    /// 모든 몬스터가 소환된 상태이고 && 남은 몬스터 개수가 0이면 Clear 판정 내리기 위해 선언
    /// </summary>
    /// <param name="isSpawned">소환된 상태인지</param>
    private void AllMonsterSpawned(bool isSpawned)
    {
        _isSpawnFinished = isSpawned; //스테이지 클리어 상태
        CheckStageClear();
    }
    private void GetDeathMonsterCount(int count)
    {

    }

    /// <summary>
    /// 스테이지 실패해서 메뉴로
    /// </summary>
    public void StageFail()
    {
        Debug.Log("StageFail신호 발생");
        //SceneManager.LoadScene("MainMenu");
        UIManager.Instance.SetGameFail();
    }
    /// <summary>
    /// 스테이지 성공해서 다음 스테이지로
    /// </summary>
    public void StageSuccess()
    {
        StageNum++;
        if (StageNum <= _stageDataList.Count)
        {
            StartCoroutine(StageStartWithCoroutine()); //딜레이준다음 스테이지 시작
        }
        else
        {
            Debug.Log($"스테이지번호 {StageNum}으로 최대 스테이지 갯수 초과하여 메인으로 이동");
            //SceneManager.LoadScene("MainMenu");
            UIManager.Instance.SetGameClear();
        }
    }
    /// <summary>
    /// 다음 스테이지로 진행하기 위한 지연 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator StageStartWithCoroutine()
    {
        SetStageData();
        Debug.Log("현재시각:"+DateTime.Now);
        yield return _stageStartDelay;
        Debug.Log("대기후시각:"+DateTime.Now);
        StageStart();
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

    /// <summary>
    /// 몬스터 매니저에 현재 스테이지의 소환할 몬스터 정보를 보냄
    /// </summary>
    public void SetMonsterManagerMonsterList()
    {
        _sponNum = _stageMonsterList.Count;
        MonsterManager._instance?.SetMonstersFromStageManager(_sponNum, _stageMonsterList, _sponDelay);
    }
    /// <summary>
    /// 해당 함수에서 현재 몬스터의 개수를 MonsterMager에서 받아옴
    /// 남은 몬스터와 스테이지 클리어 여부를 체크해서 클리어 여부를 체크한다
    /// </summary>
    /// <param name="count">남은 몬스터 개수</param>
    public void NotifieyRemainMonsterCount()
    {
        _deathMonsterCount++;

        string msg = $"Stage: {StageNum}, ReMain Monster: {_sponNum - _deathMonsterCount}";
        _notifyStageInfoForUI?.NotifyStageInfo(msg); //UI에 스테이지 정보 알림
        CheckStageClear();

    }
    private void CheckStageClear()
    {
        if (_spawnRemainCount == 0 && _isSpawnFinished && _monsterTarget.Hp != 0)
        {
            Debug.Log("스테이지 클리어 신호 동작");
            StageSuccess();
        }
    }
}
