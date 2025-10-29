using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : Singleton<MonsterManager>
{
    //####�ؾ�����
    //(Set)��Ʋ �Ŵ����� ���ݹ��� Ÿ���� �Ѱ���.
    //####
    
    private int _spawnCount = 5; //���� ��� ��ȯ�ϴ���
    private List<GoldManager.MonsterNameEnum> _currentStageMonstersInfo;
    [SerializeField] private List<GameObject> _monsterPrefabs; //���͵��� ��� ������
    private Dictionary<GoldManager.MonsterNameEnum, GameObject> _monsterMap; //��ųʸ������� ���� ã��
    [SerializeField] GameObject _uiHpBarPrefab; //UI�� ������(Monster���� ǥ���ϱ� ����)
    public event Action<int> _notifiedMonsterCount; //������ ������ ��ȭ������ �˸�
    //�ڷ�ƾ�� private �ʵ�
    private WaitForSeconds _delay; //StageManager���� Set�ϸ� �����Ǵ� ���� ���� ������
    private Coroutine _coroutine; //�ڷ�ƾ �ߺ����ට���� ���� �ϴµ� �Ƚᵵ�ɵ�? �� �����غ�����
    List<Monster> _aliveMonsters; //����ִ� ���� �迭 (������������ ��� ����ִ��� �˾ƾ��ϱ⶧����)
    //���������Ʈ�� �ʵ�
    [SerializeField] private GameObject _wayPointParent; //��������Ʈ ������ �ڽ����� �������ִ� �θ� ���� ������Ʈ
    private List<Transform> _wayPointChilds = new List<Transform>(); //_wayPointParent�� �ִ� �ڽ������� ������ ������ �ʵ�
    //(�ȵɵ�)������ƮǮ�� �����Ϸ� ������.. ���Ͱ� �ϳ��� �����Ǵ°��� �ƴ� �������� �����Ǳ⶧���� �׷��� List�� Prefab������ŭ ����־����. 
    protected override void Awake()
    {
        base.Awake(); //�̱��� üũ

        _monsterMap = new Dictionary<GoldManager.MonsterNameEnum, GameObject>();
        SetPositionByMonsterId(); //���Ϳ� ���� ������ enum������ ID�������� Set
        SetWaypointChilds();
    }
    /// <summary>
    /// �θ�(_wayPointParent)�� ����ִ� �ڽ������� Set
    /// GetComponentsInChildren�� �������� ������ �ڽĸ� ���ܾ��Ѵ�
    /// </summary>
    private void SetWaypointChilds()
    {
        //GetComponentsInChildren�� �θ���� ������ �迭�̹Ƿ� �ڽĸ� ���ܾ���
        Transform[] trs = _wayPointParent.GetComponentsInChildren<Transform>(); //�θ�+�ڽ������� trs�� ����
        foreach (var item in trs)
        {
            if (item != _wayPointParent.transform)
            {
                _wayPointChilds.Add(item); //_wayPointChilds�� �ڽĸ� ����
            }
        }
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
        List<GoldManager.MonsterNameEnum> monsterIds = new List<GoldManager.MonsterNameEnum>();
        monsterIds.Add(GoldManager.MonsterNameEnum.Slime);
        monsterIds.Add(GoldManager.MonsterNameEnum.Slime);
        monsterIds.Add(GoldManager.MonsterNameEnum.Slime);
        monsterIds.Add(GoldManager.MonsterNameEnum.Turtle);
        monsterIds.Add(GoldManager.MonsterNameEnum.Box);
        int spawnDelay = 2;
        SetMonstersFromStageManager(spawnCnt, monsterIds, spawnDelay); //�������� �Ŵ������� �ҷ���ġ�� �غ���
        StartMonsterRun(); //�������� �Ŵ������� �ҷ���ġ�� �غ���
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
    /// StageManager���� ����ϴ� �������� ������ Set���ִ� �޼���
    /// </summary>
    /// <param name="spawnCount">��ȯ ����</param>
    /// <param name="monstersInfo">�����Ҹ���Ÿ�Ը���Ʈ</param>
    /// <param name="coolDown">���� �ֱ�</param>
    public void SetMonstersFromStageManager(int spawnCnt, List<GoldManager.MonsterNameEnum> monstersInfo, int coolDown)
    {
        //Debug.Log("SetMonstersFromStageManager ����");
        _spawnCount = spawnCnt;
        _currentStageMonstersInfo = new List<GoldManager.MonsterNameEnum>(monstersInfo); //��������� ������
        _delay = new WaitForSeconds(coolDown);
    }

    //(Get)�������� �����ڿ��� ���������� ��� ���͸� ��� �Ը�� ��ȯ���� �޾ƿ;��� (���?, �������?[�ε����Ѱ��ٰž�?],��Ÿ����?
    /// <summary>
    /// StageManager���� ����ϴ� ���������� �����Ű�� �޼���
    /// SetMonstersFromStageManager �� ������ Set�� ���¿��� �����Ѵ�
    /// </summary>
    public void StartMonsterRun()
    {
        //Debug.Log("StartMonsterRun ����");
        _aliveMonsters = new List<Monster>(); //������ �ϴ� �ʱ�ȭ
        StartCoroutine(SummonMonsterCoroutine(_spawnCount, _currentStageMonstersInfo));
    }
    /// <summary>
    /// �ڷ�ƾ���� ����� �޼��� (���� ��ȯ)
    /// </summary>
    /// <param name="spawnCount">��ȯ ����</param>
    /// <param name="monsterType">��ȯ�� ���� ����</param>
    /// <returns></returns>
    IEnumerator SummonMonsterCoroutine(int spawnCount, List<GoldManager.MonsterNameEnum> monstersInfo)
    {
       // Debug.Log("SummonMonsterCoroutine ����");
        for (int index = 0; index < spawnCount; index++)
        {
            //��Monster ���� (���� ���õ� ���� Ÿ������ Prefab���� ã�Ƽ� ����
            GoldManager.MonsterNameEnum currentMonsterType = monstersInfo[index]; //���� ���õ� ���� Ÿ��
            GameObject makedMonster = Instantiate(_monsterMap[currentMonsterType], transform.position, transform.rotation);
            makedMonster.name += index; //�̸� �ӽ� ����
            Monster mon = makedMonster.GetComponent<Monster>();
            mon._monsterDeadNotified += RemoveMonster; //���Ͱ� ������ �ϴ� deleate event ����
            _aliveMonsters.Add(mon); //�����ϱ� ���� ����Ʈ�� �߰�

            //��Monser�� wayPoint�� �����Ѵ�.
            mon.SetWayPoints(_wayPointChilds);

            //��Monster�� ����ٴϴ� ü�¹ٵ� ����;
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
    /// StageManager���� ���� �����ִ� ���� ������ �˱����� ����ϴ� �Լ�
    /// </summary>
    /// <returns></returns>
    public int ReturnCurrentMonsterCount()
    {
        return _aliveMonsters.Count;
    }

    /// <summary>
    /// Monster�� �����ɶ� ȣ��Ǵ� �Լ�
    /// </summary>
    /// <param name="monster"></param>
    private void RemoveMonster(Monster monster) //�����ʿ�, ����� �̺�Ʈ ó���ϴºκ� �̻���
    {
        //���̺�Ʈ�� ����Ʈ���� ����
        monster._monsterDeadNotified -= RemoveMonster; //���Ϳ� ����ִ� �̺�Ʈ ���� (���ʹ� RemoveMonster ������ ������ Destroy��)
        _aliveMonsters.Remove(monster); //����Ʈ������ �����Ѵ�
        //�峲�� ������ ������ StageManager�� ����
        int remainMonster = ReturnCurrentMonsterCount(); 
        _notifiedMonsterCount.Invoke(remainMonster); //���� ���� ������ ������ StageManager�� ���ش�(������������)
    }
    private Transform FindUiRoot()//ĵ�������� UIRoot��� �±׸� ���� ��ġ�� �����ϱ� ���� ���
    {
        //�±׷� ã�� -> ������ "UIRoot" �±׸� �ٿ��θ� ���� ����
        GameObject tagged = GameObject.FindWithTag("UIRoot");
        if (tagged != null)
            return tagged.transform;

        return null;
    }
}
