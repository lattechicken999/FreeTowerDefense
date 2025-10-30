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
    public IStageInfo _notifyStageInfoForUI; //UIManager ���� �������� ���� ���ü� �ְ� �߰�

    private int _stageNum = 1;
    public int StageNum
    {
        get { return _stageNum; }
        set { _stageNum = value; }
    }

    private int _sponNum;
    private int _sponDelay;
    //���� Ÿ��
    //�������� �ѹ��� ���յ� ���� Ÿ�Ժ� ��ȯ����
    //���� �Ը�(�迭??)
    //���� ����(������? �ź���??)

    private float _unitCastleHp;
    public float UnitCastleHp
    {
        get { return _unitCastleHp; }
        set { _unitCastleHp = value; }
    }

    protected override void Awake()
    {
        base.Awake(); //�θ� Awakeȣ�� (�̱��� ��ü���� Ȯ�� ����)

        //�������ڸ��� �������� 1���� ��������, ���� �Ŵ����� �ش� ���� Set
        _stageMonsterList = GetStageData(_stageNum);
        MonsterManager.Instance.SubScribeMonsterCount(this);
    }

    /// <summary>
    /// �˸� ���� ��ü ����
    /// </summary>
    /// <param name="subscriber"></param>
    public void SubscribeStageInfo(IStageInfo subscriber)
    {
        _notifyStageInfoForUI = subscriber;
    }

    /// <summary>
    /// �˸����� ��ü ����
    /// </summary>
    public void UnsubscriberStageInfo()
    {
        _notifyStageInfoForUI = null;
    }
    /// <summary>
    /// �������� ���۰� ���� ����
    /// </summary>
    //���� ���� �Ը� 0�� �Ǹ�
    public void StageStart()
    {
        
        MonsterManager.Instance?.StartMonsterRun();
    }
    private void Start()
    {
        SetMonsterManagerMonsterList(); //���� �Ŵ����� �ֱ�
        StageStart();
    }

    /// <summary>
    /// �������� �����ؼ� �޴���
    /// </summary>
    public void StageFail()
    {
        SceneManager.LoadScene("MainMenu");
    }
    /// <summary>
    /// �������� �����ؼ� ���� ����������
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
    /// ScriptableObject�� ����ִ� �������� Index�� �´°��� ã�Ƽ� ������
    /// </summary>
    /// <param name="stageIndex">�������� ��ȣ</param>
    /// <returns>���������� ���� Ÿ�� ����Ʈ ��ȯ</returns>
    public List<GoldManager.MonsterNameEnum> GetStageData(int stageIndex)
    {
        foreach(var item in _stageDataList)
        {
            if(item.stageNumber == stageIndex)
            {
                _sponDelay = item.spawnDelay;
                return item.monsterInfoList; //ã���� ã���������� ����
            }
        }
        return new List<GoldManager.MonsterNameEnum>(); //��ã���� ���� �����ؼ� ����
    }
    public void SetMonsterManagerMonsterList()
    {
        _sponNum = _stageMonsterList.Count;
        MonsterManager._instance?.SetMonstersFromStageManager(_sponNum, _stageMonsterList, _sponDelay);
    }

    public void NotifieyRemainMonsterCount(int count)
    {
        string msg = $"���� ��������: {StageNum}, ���� ����: {count}";
        _notifyStageInfoForUI?.NotifyStageInfo(msg); //UI�� �������� ���� �˸�
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
        
        if (UnitCastleHp > 0)    //���� �� ü���� 0�ʰ���
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
