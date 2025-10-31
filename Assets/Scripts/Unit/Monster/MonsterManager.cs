using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class MonsterManager : Singleton<MonsterManager>
{   
    private int _spawnCount = 5; //몬스터 몇마리 소환하는지
    private int _remainSpawnCount; //소환할때 몬스터가 몇마리 남아있는지 (wave종료되었는지 판단하기 위해 사용)
    private List<GoldManager.MonsterNameEnum> _currentStageMonstersInfo;
    [SerializeField] private List<GameObject> _monsterPrefabs; //몬스터들이 담긴 프리팹
    private Dictionary<GoldManager.MonsterNameEnum, GameObject> _monsterMap; //딕셔너리값으로 몬스터 찾기
    [SerializeField] GameObject _hpBarPrefab; //UI hp바 프리팹(Monster위에 표시하기 위해)
    [SerializeField] float _hpBarWidthSize = 300.0f; //UI hp바 가로크기 설정
    [SerializeField] float _hpBarHeightSize = 40.0f; //UI hp바 세로크기 설정
    [SerializeField] float _hpBarHeightGap = 1.0f; //UI hp바 아래 위 방향으로 위치 조절

    //▼이벤트
    public event Action<bool> _notifyAllMonsterSpawn;
    private IMonsterCount _notifyMonsterCount; //StageManager에서 사용. 몬스터 갯수 변경될때마다 알리는 이벤트
    private IMonsterWaveEnd _notifyWaveEnd;
    public event Action<List<Monster>> _notifiedMonsterMake; //BattleManager에서 사용. 몬스터 자체를 넘겨줌
    //코루틴용 private 필드
    private WaitForSeconds _delay; //StageManager에서 Set하면 설정되는 몬스터 생성 딜레이
    private Coroutine _coroutine; //코루틴 중복실행때문에 쓸까 하는데 안써도될듯? 좀 생각해봐야함
    List<Monster> _aliveMonsters; //살아있는 몬스터 배열 (스테이지에서 몇마리 살아있는지 알아야하기때문에)
    //▼웨이포인트용 필드
    private MonsterWayPoint _wayPointParent; //웨이포인트 정보를 자식으로 가지고있는 부모 게임 오브젝트
    private List<Transform> _wayPointChilds; //_wayPointParent에 있는 자식정보를 꺼내서 저장한 필드
    //▼몬스터 타겟
    //[SerializeField] private MonsterTarget _mosterAttackTarget;
    private MonsterTarget _monsterTarget;

    public void SubScribeMonsterCount(IMonsterCount subscriber)
    {
        _notifyMonsterCount = subscriber;
    }
    public void UnSubScribeMonsterCount()
    {
        _notifyMonsterCount = null;
    }
    //(안될듯)오브젝트풀로 생성하려 했으나.. 몬스터가 하나만 생성되는것이 아닌 여러개가 생성되기때문에 그러면 List를 Prefab갯수만큼 들고있어야함. 
    protected override void Awake()
    {
        base.Awake(); //싱글톤 체크
        _monsterMap = new Dictionary<GoldManager.MonsterNameEnum, GameObject>();
        

    }
    /// <summary>
    /// 부모(_wayPointParent)에 담겨있는 자식정보를 Set
    /// GetComponentsInChildren로 가져오고 난다음 자식만 남겨야한다
    /// </summary>
    private void SetWaypointChilds()
    {
        //GetComponentsInChildren는 부모까지 포함한 배열이므로 자식만 남겨야함
        _wayPointChilds = new List<Transform>();
        Transform[] trs = _wayPointParent.GetComponentsInChildren<Transform>(); //부모+자식정보가 trs에 저장
        foreach (var item in trs)
        {
            if (item != _wayPointParent.transform)
            {
                _wayPointChilds.Add(item); //_wayPointChilds에 자식만 남김
            }
        }
    }
    private void Start()
    {
        //인스턴스 생성 완료 후 동작 하도록 수정
        SetPositionByMonsterId(); //몬스터에 대한 정보를 enum값으로 ID형식으로 Set
        //ForSummonTest(); //테스트용 메서드
    }
    /// <summary>
    /// (삭제예정)테스트용 코드. StageManager에서 event받았을때 어떻게 동작될지 확인
    /// </summary>
    private void ForSummonTest() //(삭제예정)
    {
        int spawnCnt = 6;
        List<GoldManager.MonsterNameEnum> monsterIds = new List<GoldManager.MonsterNameEnum>();
        monsterIds.Add(GoldManager.MonsterNameEnum.Slime);
        monsterIds.Add(GoldManager.MonsterNameEnum.Slime);
        monsterIds.Add(GoldManager.MonsterNameEnum.Slime);
        monsterIds.Add(GoldManager.MonsterNameEnum.Turtle);
        monsterIds.Add(GoldManager.MonsterNameEnum.Mummy);
        monsterIds.Add(GoldManager.MonsterNameEnum.Ghost);
        int spawnDelay = 2;
        SetMonstersFromStageManager(spawnCnt, monsterIds, spawnDelay); //스테이지 매니저에서 불렀다치고 해보기
        StartMonsterRun(); //스테이지 매니저에서 불렀다치고 해보기
    }

    /// <summary>
    /// enum과 Dictionary를 이용해서 해당 몬스터에 대한 정보를 가지고있는 
    /// GameObject를 반환하도록하기위해 Dictionary를 Set
    /// </summary>
    private void SetPositionByMonsterId()
    {
        //Check 몬스터ID가 존재하는 위치찾기 (딕셔너리로)
        if (_monsterPrefabs == null) Debug.Log("Prefab Null");
        if (_monsterMap == null) Debug.Log("_monsterMap Null");
        for (int i = 0; i < _monsterPrefabs.Count; i++)
        {
            Monster monster = _monsterPrefabs[i].GetComponent<Monster>();
            if (monster != null)
            {
                if (!_monsterMap.ContainsKey(monster._monsterId))
                {
                    _monsterMap[monster._monsterId] = _monsterPrefabs[i];
                }
            }
        }
    }

    /// <summary>
    /// StageManager에서 사용하는 스테이지 정보를 Set해주는 메서드
    /// </summary>
    /// <param name="spawnCount">소환 개수</param>
    /// <param name="monstersInfo">생성할몬스터타입리스트</param>
    /// <param name="coolDown">생성 주기</param>
    public void SetMonstersFromStageManager(int spawnCnt, List<GoldManager.MonsterNameEnum> monstersInfo, int coolDown)
    {
        _spawnCount = spawnCnt;
        _currentStageMonstersInfo = new List<GoldManager.MonsterNameEnum>(monstersInfo); //깊은복사로 가져옴
        _delay = new WaitForSeconds(coolDown);
    }

    //(Get)스테이지 관리자에서 스테이지별 어느 몬스터를 어느 규모로 소환할지 받아와야함 (몇마리?, 어느몬스터?[인덱스넘겨줄거야?],쿨타임은?
    /// <summary>
    /// StageManager에서 사용하는 스테이지를 실행시키는 메서드
    /// SetMonstersFromStageManager 로 정보가 Set된 상태에서 실행한다
    /// </summary>
    public void StartMonsterRun()
    {
        StartCoroutine(DelayedStartMonsterRun()); //바로 실행하면 FindTag가 null이 떠서 지연으로 타이밍 맞춰야함
    }
    IEnumerator DelayedStartMonsterRun()
    {
        yield return null;
        _aliveMonsters = new List<Monster>(); //받으면 일단 초기화
        StartCoroutine(SummonMonsterCoroutine(_spawnCount, _currentStageMonstersInfo)); //실행
    }
    /// <summary>
    /// 코루틴에서 사용할 메서드 (몬스터 소환)
    /// </summary>
    /// <param name="spawnCount">소환 갯수</param>
    /// <param name="monsterType">소환될 몬스터 종류</param>
    /// <returns></returns>
    IEnumerator SummonMonsterCoroutine(int spawnCount, List<GoldManager.MonsterNameEnum> monstersInfo)
    {
        _remainSpawnCount = spawnCount; //남은 몬스터 체크해서 wave종료되었는지 파악
        _notifyAllMonsterSpawn.Invoke(false); //남은 몬스터 있음
        for (int index = 0; index < spawnCount; index++)
        {
            _remainSpawnCount--;
            //▼Monster 생성 (현재 선택된 몬스터 타입으로 Prefab에서 찾아서 설정
            GameObject makedMonster = CreateMonster(monstersInfo, index);
            //▼Monser에 wayPoint를 설정한다.
            Monster mon = makedMonster.GetComponent<Monster>();
            mon.SetWayPoints(_wayPointChilds);
            //▼Monster를 따라다니는 체력바도 생성;
            CreateMonsterFollowHpBar(makedMonster, mon,index);
            yield return _delay;
        }
        if(_remainSpawnCount == 0)
        {
            _notifyAllMonsterSpawn.Invoke(true); //끝남
        }
    }
    private GameObject CreateMonster(List<GoldManager.MonsterNameEnum> monstersInfo,int index)
    {
        GoldManager.MonsterNameEnum currentMonsterType = monstersInfo[index]; //현재 선택된 몬스터 타입
        GameObject monsterInstance = Instantiate(_monsterMap[currentMonsterType], _wayPointChilds[0].position, transform.rotation);
        monsterInstance.name += index; //이름 임시 변경
        Monster mon = monsterInstance.GetComponent<Monster>();
        mon._monsterDeadNotified += RemoveMonster; //몬스터가 죽을때 하는 deleate event 정의
        mon._monsterAttackAction += MonsterAttackTarget; //몬스터가 성벽 공격할때 delegate event
        AliveMonsterAdd(mon); //_aliveMonster에 추가
        return monsterInstance;
    }

    private void CreateMonsterFollowHpBar(GameObject makedMonster, Monster mon, int index)
    {
        Transform uiRootTransform = FindUiRoot();
        GameObject obj = Instantiate(_hpBarPrefab, makedMonster.transform.position, makedMonster.transform.rotation, uiRootTransform);
        obj.name += index;
        UIHpBarMonster uiHealthBar = obj.GetComponent<UIHpBarMonster>();
        uiHealthBar.SetGapPos(_hpBarHeightGap);
        //▼uiHealthBar의 크기 변경
        RectTransform hpBarRectTransform = obj.GetComponent<RectTransform>();
        hpBarRectTransform.sizeDelta = new Vector2(_hpBarWidthSize, _hpBarHeightSize);
        uiHealthBar.SetUIPos(mon);
        
        mon.SetHpBarObject(obj); // _aliveMonsters[index].SetHpBarObject(obj);
    }

    /// <summary>
    /// 몬스터가 웨이포인트 끝지점에 도달했을때 공격하는 메서드
    /// BattleManager에서 처리하도록 타겟과 공격력을 보내주기만 한다. 
    /// </summary>
    /// <param name="attackValue"></param>
    private void MonsterAttackTarget(int attackValue) //몬스터가 성벽을 공격. 배틀매니저한테 보냄
    {
        //BattleManager한테 보냄 (아래 주석내용 다른곳에서 작업 완료된 이후 할수있음)
        int defence = _monsterTarget.DefensePoint; //이거 protected라 접근불가. 프로퍼티 작업 필요함
        int hp = (int)_monsterTarget.Hp; //이거 protected라 접근불가. 프로퍼티 작업 필요함
        int resultDamage = BattleManager.Instance.MonsterAttack(attackValue, defence, hp);
        _monsterTarget.TakenDamage(resultDamage);
    }
    
    /// <summary>
    /// StageManager에서 현재 남아있는 몬스터 갯수를 알기위해 사용하는 함수
    /// </summary>
    /// <returns></returns>
    public int ReturnCurrentMonsterCount()
    {
        return _aliveMonsters.Count;
    }

    /// <summary>
    /// Monster가 삭제될때 호출되는 함수
    /// </summary>
    /// <param name="monster"></param>
    private void RemoveMonster(Monster monster) //수정필요, 지우고 이벤트 처리하는부분 이상함
    {
        //▼삭제 전, 어떻게 죽었는지 확인 후, 처리
        if(monster._isKilledByPlayer) //플레이어에 의해 죽었다면?
        {
            ByPlayerKilled();
        }
        else //목표지점까지 도달해서 성벽에 자폭했다면?
        {
            BySelfKilled();
        }


        //▼이벤트와 리스트에서 삭제
        monster._monsterDeadNotified -= RemoveMonster; //몬스터에 들어있는 이벤트 제거 (몬스터는 RemoveMonster 끝난후 스스로 Destroy함)
        monster._monsterAttackAction -= MonsterAttackTarget; //몬스터가 성벽 공격할때 delegate event
        AliveMonsterRemove(monster);
        //▼남은 몬스터의 갯수를 StageManager로 전달
        int remainMonster = ReturnCurrentMonsterCount();
        //_notifiedMonsterCount.Invoke(remainMonster); //현재 남은 몬스터의 정보를 StageManager에 쏴준다(없어질때마다)
        _notifyMonsterCount?.NotifieyRemainMonsterCount(remainMonster);
        //▼만약 몬스터갯수가 0이고, 더이상 소환할 몬스터가 없으면 wave종료를 StageManager에 쏴준다
        _notifyWaveEnd?.MonsterWaveEnd();
    }
    /// <summary>
    /// _aliveMonsters 추가할때는 무조건 이벤트 실행되어야해서 추가
    /// </summary>
    /// <param name="mon"></param>
    private void AliveMonsterAdd(Monster mon)
    {
        _aliveMonsters.Add(mon); //관리하기 위해 리스트에 추가
        _notifiedMonsterMake?.Invoke(_aliveMonsters);
    }
    /// <summary>
    /// _aliveMonsters 삭제할때는 무조건 이벤트 실행되어야해서 추가
    /// </summary>
    /// <param name="mon"></param>
    private void AliveMonsterRemove(Monster mon)
    {
        _aliveMonsters.Remove(mon); //리스트에서도 삭제한다
        _notifiedMonsterMake?.Invoke(_aliveMonsters);
    }

    private void ByPlayerKilled()
    {
        //플레이어가 죽였을때 행동 저장. 돈을 증가시킨다 등
    }
    private void BySelfKilled()
    {
        //몬스터가 끝지점까지 가서 스스로 파괴했을때 행동 저장. 성벽HP가 감소한다
    }

    private Transform FindUiRoot()//캔버스에서 UIRoot라는 태그를 가진 위치에 생성하기 위해 사용
    {
        //태그로 찾기 -> 씬에서 "UIRoot" 태그를 붙여두면 가장 빠름
        GameObject tagged = GameObject.FindWithTag("UIRoot");
        if (tagged != null)
            return tagged.transform;

        Debug.Log("태그 안잡힘. 몬스터 생성안되었습니다");
        return null;
    }

    //▼ 아래는 각각의 스크립트들이 Awake될때 자동으로 MonsterManager에 정보를 주도록 동적으로 설정
    public void SetMonsterTargetInfo(MonsterTarget target)
    {
        _monsterTarget = target;
    }
    public void UnSetMonsterTargetInfo()
    {
        _monsterTarget = null;
    }
    public void SetWayPointInfo(MonsterWayPoint wayPoint)
    {
        _wayPointParent = wayPoint;
        SetWaypointChilds();
    }
    public void UnSetWayPointInfo()
    {
        _wayPointParent = null;
    }
}
