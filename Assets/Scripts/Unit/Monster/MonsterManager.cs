using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum MonsterId   //StageManager에서 생성해줄때 인덱스로하면 고치기힘듬. enum으로 설정
{
    slime,
    turtle,
    flyMonster,
    ghost,
    _End
}
public class MonsterManager : MonoBehaviour
{
    //매니저는 싱글톤으로 관리
    private static MonsterManager _instance;
    public static MonsterManager Instance
    {
        get
        {
            if (_instance == null) 
            {
                _instance = FindObjectOfType<MonsterManager>();
                if (_instance == null)
                {
                    _instance = new MonsterManager();
                }
            }
            return _instance;
        }
    }

    //####해야할일
    //(Set)스테이지 관리자에서 몬스터가 다 죽었다고 이벤트 발생되면? ?? 어떤 작업을 해야함
    //(Set)배틀 매니저에 공격밭을 타겟을 넘겨줌?
    //몬스터 자료구조
    //####
    /*
    //아래 정보 필요없음 (StageManager에서 어짜피 받아서 쓰기때문에 설정할 이유가 없다)
    [SerializeField] private int _spawnCount = 5; //몬스터 몇마리 소환하는지
    [SerializeField] private float _spawnCoolDown = 1.0f; //몬스터 소환 쿨타임
    */
    [SerializeField] private List<GameObject> _monsterPrefabs; //몬스터들이 담긴 프리팹
    private Dictionary<MonsterId, GameObject> _monsterMap; //딕셔너리값으로 몬스터 찾기
    //코루틴용 private 필드
    private WaitForSeconds _delay;
    private Coroutine _coroutine; //코루틴 중복실행때문에 쓸까 하는데 안써도될듯? 좀 생각해봐야함
    //(안될듯)오브젝트풀로 생성하려 했으나.. 몬스터가 하나만 생성되는것이 아닌 여러개가 생성되기때문에 그러면 List를 Prefab갯수만큼 들고있어야함. 
    private void Awake()
    {
        _monsterMap = new Dictionary<MonsterId, GameObject>();
        InitSIngleton(); //싱글톤 체크
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
        List<MonsterId> monsterIds = new List<MonsterId>();
        monsterIds.Add(MonsterId.slime);
        monsterIds.Add(MonsterId.turtle);
        monsterIds.Add(MonsterId.flyMonster);
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
    /// <summary>
    /// 싱글톤으로 유지하기위해 체크
    /// </summary>
    private void InitSIngleton()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(_instance);
    }

    //(Get)스테이지 관리자에서 스테이지별 어느 몬스터를 어느 규모로 소환할지 받아와야함 (몇마리?, 어느몬스터?[인덱스넘겨줄거야?],쿨타임은?
    public void SummonMonsters(int spawnCount, List<MonsterId> monsterType, int coolDown) //둘다 해보죠? 1. List
    {
        Debug.Log("SummonMonsters 들어옴");
        //아래거를 코루틴으로........
        _delay = new WaitForSeconds(coolDown);
        //count만큼, List에 들어있는 종류만큼, coolDown만큼 지연을 두며 실행
        StartCoroutine(SummonMonsterCoroutine(spawnCount, monsterType));
    }
    /// <summary>
    /// 코루틴에서 사용할 메서드 (몬스터 소환)
    /// </summary>
    /// <param name="spawnCount">소환 갯수</param>
    /// <param name="monsterType">소환될 몬스터 종류</param>
    /// <returns></returns>
    IEnumerator SummonMonsterCoroutine(int spawnCount, List<MonsterId> monsterType)
    {
        Debug.Log("SummonMonsterCoroutine 들어옴");
        int typeCount = monsterType.Count;
        for (int index = 0; index < spawnCount; index++)
        {
            Debug.Log("SummonMonsterCoroutine- for문 동작");
            Debug.Log(typeCount);
            int rndNum = Random.Range(0, typeCount);
            GameObject pickMonster = _monsterMap[monsterType[rndNum]]; //랜덤으로 선택된 게임 오브젝트
            Instantiate(pickMonster, transform.position, transform.rotation);
            yield return _delay;
        }
    }
}
