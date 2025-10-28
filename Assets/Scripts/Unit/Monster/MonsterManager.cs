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
public class MonsterManager : Singleton<MonsterManager>
{
    //####�ؾ�����
    //(Set)��Ʋ �Ŵ����� ���ݹ��� Ÿ���� �Ѱ���.
    //####
    /*
    //�Ʒ� ���� �ʿ���� (StageManager���� ��¥�� �޾Ƽ� ���⶧���� ������ ������ ����)
    [SerializeField] private int _spawnCount = 5; //���� ��� ��ȯ�ϴ���
    [SerializeField] private float _spawnCoolDown = 1.0f; //���� ��ȯ ��Ÿ��
    */
    [SerializeField] private List<GameObject> _monsterPrefabs; //���͵��� ��� ������
    private Dictionary<MonsterId, GameObject> _monsterMap; //��ųʸ������� ���� ã��
    [SerializeField] GameObject _uiHpBarPrefab;
    //�ڷ�ƾ�� private �ʵ�
    private WaitForSeconds _delay;
    private Coroutine _coroutine; //�ڷ�ƾ �ߺ����ට���� ���� �ϴµ� �Ƚᵵ�ɵ�? �� �����غ�����
    List<Monster> _aliveMonsters; //����ִ� ���� �迭 (������������ ��� ����ִ��� �˾ƾ��ϱ⶧����)
    //(�ȵɵ�)������ƮǮ�� �����Ϸ� ������.. ���Ͱ� �ϳ��� �����Ǵ°��� �ƴ� �������� �����Ǳ⶧���� �׷��� List�� Prefab������ŭ ����־����. 
    protected override void Awake()
    {
        base.Awake(); //�̱��� üũ

        _monsterMap = new Dictionary<MonsterId, GameObject>();
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

    //(Get)�������� �����ڿ��� ���������� ��� ���͸� ��� �Ը�� ��ȯ���� �޾ƿ;��� (���?, �������?[�ε����Ѱ��ٰž�?],��Ÿ����?
    public void SummonMonsters(int spawnCount, List<MonsterId> monsterType, int coolDown) //�Ѵ� �غ���? 1. List
    {
        //Debug.Log("SummonMonsters ����");
        _delay = new WaitForSeconds(coolDown);
        _aliveMonsters = new List<Monster>(); //������ �ϴ� �ʱ�ȭ
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
       // Debug.Log("SummonMonsterCoroutine ����");
        int typeCount = monsterType.Count;
        for (int index = 0; index < spawnCount; index++)
        {
            //Monster ����
            int rndNum = Random.Range(0, typeCount);
            GameObject pickMonster = _monsterMap[monsterType[rndNum]]; //�������� ���õ� ���� ������Ʈ
            GameObject makedMonster = Instantiate(pickMonster, transform.position, transform.rotation);
            makedMonster.name += index;
            Monster mon = makedMonster.GetComponent<Monster>();
            mon._onDeath += RemoveMonster;
            _aliveMonsters.Add(mon); //�����ϱ� ���� ����Ʈ�� �߰�
            //Monster�� ����ٴϴ� ü�¹ٵ� ����;
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
    /// �̺�Ʈ �߻��ϸ� ����ó��, ��������
    /// </summary>
    /// <param name="monster"></param>
    private void RemoveMonster(Monster monster) //�����ʿ�, ����� �̺�Ʈ ó���ϴºκ� �̻���
    {
        //���������ʳ�? �̺�Ʈ�� �̻������ʳ�?
        monster._onDeath -= RemoveMonster;
        _aliveMonsters.Remove(monster);
        Debug.Log("������: ���簹�� " + _aliveMonsters.Count);
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
