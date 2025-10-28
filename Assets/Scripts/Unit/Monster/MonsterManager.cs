using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : Singleton<MonsterManager>
{
    //####해야할일
    //(Set)배틀 매니저에 공격밭을 타겟을 넘겨줌.
    //####
    /*
    //아래 정보 필요없음 (StageManager에서 어짜피 받아서 쓰기때문에 설정할 이유가 없다)
    [SerializeField] private int _spawnCount = 5; //몬스터 몇마리 소환하는지
    [SerializeField] private float _spawnCoolDown = 1.0f; //몬스터 소환 쿨타임
    */
    [SerializeField] private List<GameObject> _monsterPrefabs; //몬스터들이 담긴 프리팹
    private Dictionary<GoldManager.MonsterNameEnum, GameObject> _monsterMap; //딕셔너리값으로 몬스터 찾기
    [SerializeField] GameObject _uiHpBarPrefab; //UI바 프리팹(Monster위에 표시하기 위해)
    public event Action<int> _notifiedMonsterCount;
    //코루틴용 private 필드
    private WaitForSeconds _delay;
    private Coroutine _coroutine; //코루틴 중복실행때문에 쓸까 하는데 안써도될듯? 좀 생각해봐야함
    List<Monster> _aliveMonsters; //살아있는 몬스터 배열 (스테이지에서 몇마리 살아있는지 알아야하기때문에)
    //(안될듯)오브젝트풀로 생성하려 했으나.. 몬스터가 하나만 생성되는것이 아닌 여러개가 생성되기때문에 그러면 List를 Prefab갯수만큼 들고있어야함. 
    protected override void Awake()
    {
        base.Awake(); //싱글톤 체크

        _monsterMap = new Dictionary<GoldManager.MonsterNameEnum, GameObject>();
        SetPositionByMonsterId(); //몬스터에 대한 정보를 enum값으로 ID형식으로 Set
    }
    private void Start()
    {
        ForSummonTest(); //테스트용 메서드
    }
    /// <summary>
    /// (삭제예정)테스트용 코드. StageManager에서 event받았을때 어떻게 동작될지 확인
    /// </summary>
    private void ForSummonTest() //(삭제예정)
    {
        int spawnCnt = 5;
        List<GoldManager.MonsterNameEnum> monsterIds = new List<GoldManager.MonsterNameEnum>();
        monsterIds.Add(GoldManager.MonsterNameEnum.Slime);
        monsterIds.Add(GoldManager.MonsterNameEnum.Slime);
        monsterIds.Add(GoldManager.MonsterNameEnum.Slime);
        monsterIds.Add(GoldManager.MonsterNameEnum.Turtle);
        monsterIds.Add(GoldManager.MonsterNameEnum.Box);
        int spawnDelay = 2;
        SummonMonsters(spawnCnt, monsterIds, spawnDelay); //스테이지 매니저에서 불렀다치고 해보기
    }

    /// <summary>
    /// enum과 Dictionary를 이용해서 해당 몬스터에 대한 정보를 가지고있는 
    /// GameObject를 반환하도록하기위해 Dictionary를 Set
    /// </summary>
    private void SetPositionByMonsterId()
    {
        //Check 몬스터ID가 존재하는 위치찾기 (딕셔너리로)
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
    //(Get)스테이지 관리자에서 스테이지별 어느 몬스터를 어느 규모로 소환할지 받아와야함 (몇마리?, 어느몬스터?[인덱스넘겨줄거야?],쿨타임은?
    public void SummonMonsters(int spawnCount, List<GoldManager.MonsterNameEnum> monstersInfo, int coolDown) //둘다 해보죠? 1. List
    {
        //Debug.Log("SummonMonsters 들어옴");
        _delay = new WaitForSeconds(coolDown);
        _aliveMonsters = new List<Monster>(); //받으면 일단 초기화
        //count만큼, List에 들어있는 종류만큼, coolDown만큼 지연을 두며 실행
        StartCoroutine(SummonMonsterCoroutine(spawnCount, monstersInfo));
    }
    /// <summary>
    /// 코루틴에서 사용할 메서드 (몬스터 소환)
    /// </summary>
    /// <param name="spawnCount">소환 갯수</param>
    /// <param name="monsterType">소환될 몬스터 종류</param>
    /// <returns></returns>
    IEnumerator SummonMonsterCoroutine(int spawnCount, List<GoldManager.MonsterNameEnum> monstersInfo)
    {
       // Debug.Log("SummonMonsterCoroutine 들어옴");
        for (int index = 0; index < spawnCount; index++)
        {
            //▼Monster 생성 (현재 선택된 몬스터 타입으로 Prefab에서 찾아서 설정
            GoldManager.MonsterNameEnum currentMonsterType = monstersInfo[index]; //현재 선택된 몬스터 타입
            GameObject makedMonster = Instantiate(_monsterMap[currentMonsterType], transform.position, transform.rotation);
            makedMonster.name += index; //이름 임시 변경
            Monster mon = makedMonster.GetComponent<Monster>();
            mon._monsterDeadNotified += RemoveMonster; //몬스터가 죽을때 하는 deleate event 정의
            _aliveMonsters.Add(mon); //관리하기 위해 리스트에 추가
            //▼Monster를 따라다니는 체력바도 생성;
            int maxHp = (int)mon._Hp;
            Transform uiRootTransform = FindUiRoot();
            GameObject obj = Instantiate(_uiHpBarPrefab, makedMonster.transform.position, makedMonster.transform.rotation, uiRootTransform);
            obj.name += index;
            UIHpBarMonster uiHealthBar = obj.GetComponent<UIHpBarMonster>();
            uiHealthBar.SetUIPos(mon);


            yield return _delay;
        }
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
        //▼이벤트와 리스트에서 삭제
        monster._monsterDeadNotified -= RemoveMonster; //몬스터에 들어있는 이벤트 제거 (몬스터는 RemoveMonster 끝난후 스스로 Destroy함)
        _aliveMonsters.Remove(monster); //리스트에서도 삭제한다
        //▼남은 몬스터의 갯수를 StageManager로 전달
        int remainMonster = ReturnCurrentMonsterCount(); 
        _notifiedMonsterCount.Invoke(remainMonster); //현재 남은 몬스터의 정보를 StageManager에 쏴준다(없어질때마다)
    }
    private Transform FindUiRoot()//캔버스에서 UIRoot라는 태그를 가진 위치에 생성하기 위해 사용
    {
        //태그로 찾기 -> 씬에서 "UIRoot" 태그를 붙여두면 가장 빠름
        GameObject tagged = GameObject.FindWithTag("UIRoot");
        if (tagged != null)
            return tagged.transform;

        return null;
    }
}
