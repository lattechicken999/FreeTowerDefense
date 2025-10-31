using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : Singleton<StageManager>, IMonsterCount, IMonsterWaveEnd
{
    [SerializeField] private List<StageDataSO> _stageDataList;
    [SerializeField] private int _startNextStageDelay = 3;
    private MonsterTarget _monsterTarget;
    private List<GoldManager.MonsterNameEnum> _stageMonsterList;
    public IStageInfo _notifyStageInfoForUI; //UIManager ���� �������� ���� ���ü� �ְ� �߰�
    private bool _isStageClear;
    private WaitForSeconds _stageStartDelay;

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
        _stageStartDelay = new WaitForSeconds(_startNextStageDelay);
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
    /// monsterTarget���� ����ϴ� ���� ���� �̺�Ʈ
    /// </summary>
    public void SubscribeMonsterTargetEvent(MonsterTarget monsterTarget)
    {
        monsterTarget._gameFailNotify += StageFail;
    }
    /// <summary>
    /// monsterTarget���� ����ϴ� ���� ���� �̺�Ʈ
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
    /// �������� ���۰� ���� ����
    /// </summary>
    //���� ���� �Ը� 0�� �Ǹ�
    public void StageStart()
    {
        _isStageClear = false;
        MonsterManager.Instance?.StartMonsterRun();
    }
    private void Start()
    {
        MonsterManager.Instance.SubScribeMonsterCount(this);
        MonsterManager.Instance._notifyAllMonsterSpawn += AllMonsterDefeat;
        SetStageData();
        //StageStart();
    }
    private void SetStageData()
    {
        _stageMonsterList = GetStageData(StageNum);
        SetMonsterManagerMonsterList(); //���� �Ŵ����� �ֱ�

    }
    private void OnDestroy()
    {
        if(Instance == this)
        {
            Debug.Log("StageManager Singleton �ν��Ͻ����� OnDestroyȣ���");
            MonsterManager.Instance.UnSubScribeMonsterCount();
            MonsterManager.Instance._notifyAllMonsterSpawn -= AllMonsterDefeat;
        }
    }
    private void AllMonsterDefeat(bool isClear)
    {
        _isStageClear = isClear; //�������� Ŭ���� ����
    }

    /// <summary>
    /// �������� �����ؼ� �޴���
    /// </summary>
    public void StageFail()
    {
        Debug.Log("StageFail��ȣ �߻�");
        SceneManager.LoadScene("MainMenu");
    }
    /// <summary>
    /// �������� �����ؼ� ���� ����������
    /// </summary>
    public void StageSuccess()
    {
        StageNum++;
        if (StageNum <= _stageDataList.Count)
        {
            StartCoroutine(StageStartWithCoroutine()); //�������ش��� �������� ����
        }
        else
        {
            Debug.Log($"����������ȣ {StageNum}���� �ִ� �������� ���� �ʰ��Ͽ� �������� �̵�");
            SceneManager.LoadScene("MainMenu");
        }
    }
    IEnumerator StageStartWithCoroutine()
    {
        SetStageData();
        Debug.Log("����ð�:"+DateTime.Now);
        yield return _stageStartDelay;
        Debug.Log("����Ľð�:"+DateTime.Now);
        StageStart();
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
        
        if(count == 0 && _isStageClear && _monsterTarget.Hp != 0)
        {
            Debug.Log("�������� Ŭ���� ��ȣ ����");
            StageSuccess();
        }
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
