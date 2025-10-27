using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum MonsterId   //StageManager���� �������ٶ� �ε������ϸ� ��ġ������. enum���� ����
{
    slime,
    turtle,
    flyMonster,
    ghost,
    _End
}
public class MonsterManager : MonoBehaviour
{
    //�Ŵ����� �̱������� ����
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

    //####�ؾ�����
    //(Set)�������� �����ڿ��� ���Ͱ� �� �׾��ٰ� �̺�Ʈ �߻��Ǹ�? ?? � �۾��� �ؾ���
    //(Set)��Ʋ �Ŵ����� ���ݹ��� Ÿ���� �Ѱ���?
    //���� �ڷᱸ��
    //####
    /*
    //�Ʒ� ���� �ʿ���� (StageManager���� ��¥�� �޾Ƽ� ���⶧���� ������ ������ ����)
    [SerializeField] private int _spawnCount = 5; //���� ��� ��ȯ�ϴ���
    [SerializeField] private float _spawnCoolDown = 1.0f; //���� ��ȯ ��Ÿ��
    */
    [SerializeField] private List<GameObject> _monsterPrefabs; //���͵��� ��� ������
    private Dictionary<MonsterId, GameObject> _monsterMap; //��ųʸ������� ���� ã��
    //�ڷ�ƾ�� private �ʵ�
    private WaitForSeconds _delay;
    private Coroutine _coroutine; //�ڷ�ƾ �ߺ����ට���� ���� �ϴµ� �Ƚᵵ�ɵ�? �� �����غ�����
    //(�ȵɵ�)������ƮǮ�� �����Ϸ� ������.. ���Ͱ� �ϳ��� �����Ǵ°��� �ƴ� �������� �����Ǳ⶧���� �׷��� List�� Prefab������ŭ ����־����. 
    private void Awake()
    {
        _monsterMap = new Dictionary<MonsterId, GameObject>();
        InitSIngleton(); //�̱��� üũ
        SetPositionByMonsterId(); //���Ϳ� ���� ������ enum������ ID�������� Set
    }
    private void Start()
    {
        ForSummonTest(); //�׽�Ʈ�� �޼���
    }
    /// <summary>
    /// (��������)�׽�Ʈ�� �ڵ�. StageManager���� event�޾����� ��� ���۵��� Ȯ��
    /// </summary>
    private void ForSummonTest() //(��������)
    {
        int spawnCnt = 5;
        List<MonsterId> monsterIds = new List<MonsterId>();
        monsterIds.Add(MonsterId.slime);
        monsterIds.Add(MonsterId.turtle);
        monsterIds.Add(MonsterId.flyMonster);
        int spawnDelay = 2;
        SummonMonsters(spawnCnt, monsterIds, spawnDelay); //�������� �Ŵ������� �ҷ���ġ�� �غ���
    }

    /// <summary>
    /// enum�� Dictionary�� �̿��ؼ� �ش� ���Ϳ� ���� ������ �������ִ� 
    /// GameObject�� ��ȯ�ϵ����ϱ����� Dictionary�� Set
    /// </summary>
    private void SetPositionByMonsterId()
    {
        //Check ����ID�� �����ϴ� ��ġã�� (��ųʸ���)
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
    /// �̱������� �����ϱ����� üũ
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

    //(Get)�������� �����ڿ��� ���������� ��� ���͸� ��� �Ը�� ��ȯ���� �޾ƿ;��� (���?, �������?[�ε����Ѱ��ٰž�?],��Ÿ����?
    public void SummonMonsters(int spawnCount, List<MonsterId> monsterType, int coolDown) //�Ѵ� �غ���? 1. List
    {
        Debug.Log("SummonMonsters ����");
        //�Ʒ��Ÿ� �ڷ�ƾ����........
        _delay = new WaitForSeconds(coolDown);
        //count��ŭ, List�� ����ִ� ������ŭ, coolDown��ŭ ������ �θ� ����
        StartCoroutine(SummonMonsterCoroutine(spawnCount, monsterType));
    }
    /// <summary>
    /// �ڷ�ƾ���� ����� �޼��� (���� ��ȯ)
    /// </summary>
    /// <param name="spawnCount">��ȯ ����</param>
    /// <param name="monsterType">��ȯ�� ���� ����</param>
    /// <returns></returns>
    IEnumerator SummonMonsterCoroutine(int spawnCount, List<MonsterId> monsterType)
    {
        Debug.Log("SummonMonsterCoroutine ����");
        int typeCount = monsterType.Count;
        for (int index = 0; index < spawnCount; index++)
        {
            Debug.Log("SummonMonsterCoroutine- for�� ����");
            Debug.Log(typeCount);
            int rndNum = Random.Range(0, typeCount);
            GameObject pickMonster = _monsterMap[monsterType[rndNum]]; //�������� ���õ� ���� ������Ʈ
            Instantiate(pickMonster, transform.position, transform.rotation);
            yield return _delay;
        }
    }
}
