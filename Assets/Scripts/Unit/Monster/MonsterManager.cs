using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class MonsterManager : Singleton<MonsterManager>
{   
    private int _spawnCount = 5; //���� ��� ��ȯ�ϴ���
    private int _remainSpawnCount; //��ȯ�Ҷ� ���Ͱ� ��� �����ִ��� (wave����Ǿ����� �Ǵ��ϱ� ���� ���)
    private List<GoldManager.MonsterNameEnum> _currentStageMonstersInfo;
    [SerializeField] private List<GameObject> _monsterPrefabs; //���͵��� ��� ������
    private Dictionary<GoldManager.MonsterNameEnum, GameObject> _monsterMap; //��ųʸ������� ���� ã��
    [SerializeField] GameObject _hpBarPrefab; //UI hp�� ������(Monster���� ǥ���ϱ� ����)
    [SerializeField] float _hpBarWidthSize = 300.0f; //UI hp�� ����ũ�� ����
    [SerializeField] float _hpBarHeightSize = 40.0f; //UI hp�� ����ũ�� ����
    [SerializeField] float _hpBarHeightGap = 1.0f; //UI hp�� �Ʒ� �� �������� ��ġ ����

    //���̺�Ʈ
    public event Action<bool> _notifyAllMonsterSpawn;
    private IMonsterCount _notifyMonsterCount; //StageManager���� ���. ���� ���� ����ɶ����� �˸��� �̺�Ʈ
    private IMonsterWaveEnd _notifyWaveEnd;
    public event Action<List<Monster>> _notifiedMonsterMake; //BattleManager���� ���. ���� ��ü�� �Ѱ���
    //�ڷ�ƾ�� private �ʵ�
    private WaitForSeconds _delay; //StageManager���� Set�ϸ� �����Ǵ� ���� ���� ������
    private Coroutine _coroutine; //�ڷ�ƾ �ߺ����ට���� ���� �ϴµ� �Ƚᵵ�ɵ�? �� �����غ�����
    List<Monster> _aliveMonsters; //����ִ� ���� �迭 (������������ ��� ����ִ��� �˾ƾ��ϱ⶧����)
    //���������Ʈ�� �ʵ�
    private MonsterWayPoint _wayPointParent; //��������Ʈ ������ �ڽ����� �������ִ� �θ� ���� ������Ʈ
    private List<Transform> _wayPointChilds; //_wayPointParent�� �ִ� �ڽ������� ������ ������ �ʵ�
    //����� Ÿ��
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
    //(�ȵɵ�)������ƮǮ�� �����Ϸ� ������.. ���Ͱ� �ϳ��� �����Ǵ°��� �ƴ� �������� �����Ǳ⶧���� �׷��� List�� Prefab������ŭ ����־����. 
    protected override void Awake()
    {
        base.Awake(); //�̱��� üũ
        _monsterMap = new Dictionary<GoldManager.MonsterNameEnum, GameObject>();
        

    }
    /// <summary>
    /// �θ�(_wayPointParent)�� ����ִ� �ڽ������� Set
    /// GetComponentsInChildren�� �������� ������ �ڽĸ� ���ܾ��Ѵ�
    /// </summary>
    private void SetWaypointChilds()
    {
        //GetComponentsInChildren�� �θ���� ������ �迭�̹Ƿ� �ڽĸ� ���ܾ���
        _wayPointChilds = new List<Transform>();
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
        //�ν��Ͻ� ���� �Ϸ� �� ���� �ϵ��� ����
        SetPositionByMonsterId(); //���Ϳ� ���� ������ enum������ ID�������� Set
        //ForSummonTest(); //�׽�Ʈ�� �޼���
    }
    /// <summary>
    /// (��������)�׽�Ʈ�� �ڵ�. StageManager���� event�޾����� ��� ���۵��� Ȯ��
    /// </summary>
    private void ForSummonTest() //(��������)
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
    /// StageManager���� ����ϴ� �������� ������ Set���ִ� �޼���
    /// </summary>
    /// <param name="spawnCount">��ȯ ����</param>
    /// <param name="monstersInfo">�����Ҹ���Ÿ�Ը���Ʈ</param>
    /// <param name="coolDown">���� �ֱ�</param>
    public void SetMonstersFromStageManager(int spawnCnt, List<GoldManager.MonsterNameEnum> monstersInfo, int coolDown)
    {
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
        StartCoroutine(DelayedStartMonsterRun()); //�ٷ� �����ϸ� FindTag�� null�� ���� �������� Ÿ�̹� �������
    }
    IEnumerator DelayedStartMonsterRun()
    {
        yield return null;
        _aliveMonsters = new List<Monster>(); //������ �ϴ� �ʱ�ȭ
        StartCoroutine(SummonMonsterCoroutine(_spawnCount, _currentStageMonstersInfo)); //����
    }
    /// <summary>
    /// �ڷ�ƾ���� ����� �޼��� (���� ��ȯ)
    /// </summary>
    /// <param name="spawnCount">��ȯ ����</param>
    /// <param name="monsterType">��ȯ�� ���� ����</param>
    /// <returns></returns>
    IEnumerator SummonMonsterCoroutine(int spawnCount, List<GoldManager.MonsterNameEnum> monstersInfo)
    {
        _remainSpawnCount = spawnCount; //���� ���� üũ�ؼ� wave����Ǿ����� �ľ�
        _notifyAllMonsterSpawn.Invoke(false); //���� ���� ����
        for (int index = 0; index < spawnCount; index++)
        {
            _remainSpawnCount--;
            //��Monster ���� (���� ���õ� ���� Ÿ������ Prefab���� ã�Ƽ� ����
            GameObject makedMonster = CreateMonster(monstersInfo, index);
            //��Monser�� wayPoint�� �����Ѵ�.
            Monster mon = makedMonster.GetComponent<Monster>();
            mon.SetWayPoints(_wayPointChilds);
            //��Monster�� ����ٴϴ� ü�¹ٵ� ����;
            CreateMonsterFollowHpBar(makedMonster, mon,index);
            yield return _delay;
        }
        if(_remainSpawnCount == 0)
        {
            _notifyAllMonsterSpawn.Invoke(true); //����
        }
    }
    private GameObject CreateMonster(List<GoldManager.MonsterNameEnum> monstersInfo,int index)
    {
        GoldManager.MonsterNameEnum currentMonsterType = monstersInfo[index]; //���� ���õ� ���� Ÿ��
        GameObject monsterInstance = Instantiate(_monsterMap[currentMonsterType], _wayPointChilds[0].position, transform.rotation);
        monsterInstance.name += index; //�̸� �ӽ� ����
        Monster mon = monsterInstance.GetComponent<Monster>();
        mon._monsterDeadNotified += RemoveMonster; //���Ͱ� ������ �ϴ� deleate event ����
        mon._monsterAttackAction += MonsterAttackTarget; //���Ͱ� ���� �����Ҷ� delegate event
        AliveMonsterAdd(mon); //_aliveMonster�� �߰�
        return monsterInstance;
    }

    private void CreateMonsterFollowHpBar(GameObject makedMonster, Monster mon, int index)
    {
        Transform uiRootTransform = FindUiRoot();
        GameObject obj = Instantiate(_hpBarPrefab, makedMonster.transform.position, makedMonster.transform.rotation, uiRootTransform);
        obj.name += index;
        UIHpBarMonster uiHealthBar = obj.GetComponent<UIHpBarMonster>();
        uiHealthBar.SetGapPos(_hpBarHeightGap);
        //��uiHealthBar�� ũ�� ����
        RectTransform hpBarRectTransform = obj.GetComponent<RectTransform>();
        hpBarRectTransform.sizeDelta = new Vector2(_hpBarWidthSize, _hpBarHeightSize);
        uiHealthBar.SetUIPos(mon);
        
        mon.SetHpBarObject(obj); // _aliveMonsters[index].SetHpBarObject(obj);
    }

    /// <summary>
    /// ���Ͱ� ��������Ʈ �������� ���������� �����ϴ� �޼���
    /// BattleManager���� ó���ϵ��� Ÿ�ٰ� ���ݷ��� �����ֱ⸸ �Ѵ�. 
    /// </summary>
    /// <param name="attackValue"></param>
    private void MonsterAttackTarget(int attackValue) //���Ͱ� ������ ����. ��Ʋ�Ŵ������� ����
    {
        //BattleManager���� ���� (�Ʒ� �ּ����� �ٸ������� �۾� �Ϸ�� ���� �Ҽ�����)
        int defence = _monsterTarget.DefensePoint; //�̰� protected�� ���ٺҰ�. ������Ƽ �۾� �ʿ���
        int hp = (int)_monsterTarget.Hp; //�̰� protected�� ���ٺҰ�. ������Ƽ �۾� �ʿ���
        int resultDamage = BattleManager.Instance.MonsterAttack(attackValue, defence, hp);
        _monsterTarget.TakenDamage(resultDamage);
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
        //����� ��, ��� �׾����� Ȯ�� ��, ó��
        if(monster._isKilledByPlayer) //�÷��̾ ���� �׾��ٸ�?
        {
            ByPlayerKilled();
        }
        else //��ǥ�������� �����ؼ� ������ �����ߴٸ�?
        {
            BySelfKilled();
        }


        //���̺�Ʈ�� ����Ʈ���� ����
        monster._monsterDeadNotified -= RemoveMonster; //���Ϳ� ����ִ� �̺�Ʈ ���� (���ʹ� RemoveMonster ������ ������ Destroy��)
        monster._monsterAttackAction -= MonsterAttackTarget; //���Ͱ� ���� �����Ҷ� delegate event
        AliveMonsterRemove(monster);
        //�峲�� ������ ������ StageManager�� ����
        int remainMonster = ReturnCurrentMonsterCount();
        //_notifiedMonsterCount.Invoke(remainMonster); //���� ���� ������ ������ StageManager�� ���ش�(������������)
        _notifyMonsterCount?.NotifieyRemainMonsterCount(remainMonster);
        //�常�� ���Ͱ����� 0�̰�, ���̻� ��ȯ�� ���Ͱ� ������ wave���Ḧ StageManager�� ���ش�
        _notifyWaveEnd?.MonsterWaveEnd();
    }
    /// <summary>
    /// _aliveMonsters �߰��Ҷ��� ������ �̺�Ʈ ����Ǿ���ؼ� �߰�
    /// </summary>
    /// <param name="mon"></param>
    private void AliveMonsterAdd(Monster mon)
    {
        _aliveMonsters.Add(mon); //�����ϱ� ���� ����Ʈ�� �߰�
        _notifiedMonsterMake?.Invoke(_aliveMonsters);
    }
    /// <summary>
    /// _aliveMonsters �����Ҷ��� ������ �̺�Ʈ ����Ǿ���ؼ� �߰�
    /// </summary>
    /// <param name="mon"></param>
    private void AliveMonsterRemove(Monster mon)
    {
        _aliveMonsters.Remove(mon); //����Ʈ������ �����Ѵ�
        _notifiedMonsterMake?.Invoke(_aliveMonsters);
    }

    private void ByPlayerKilled()
    {
        //�÷��̾ �׿����� �ൿ ����. ���� ������Ų�� ��
    }
    private void BySelfKilled()
    {
        //���Ͱ� ���������� ���� ������ �ı������� �ൿ ����. ����HP�� �����Ѵ�
    }

    private Transform FindUiRoot()//ĵ�������� UIRoot��� �±׸� ���� ��ġ�� �����ϱ� ���� ���
    {
        //�±׷� ã�� -> ������ "UIRoot" �±׸� �ٿ��θ� ���� ����
        GameObject tagged = GameObject.FindWithTag("UIRoot");
        if (tagged != null)
            return tagged.transform;

        Debug.Log("�±� ������. ���� �����ȵǾ����ϴ�");
        return null;
    }

    //�� �Ʒ��� ������ ��ũ��Ʈ���� Awake�ɶ� �ڵ����� MonsterManager�� ������ �ֵ��� �������� ����
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
