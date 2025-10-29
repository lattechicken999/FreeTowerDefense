using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �������� ���� �� HP ����, ���� ���� ������Ʈ
/// ���� �ǸŽ� �Ǹ� UI ���
/// </summary>
public class UIManager : Singleton<UIManager>, ICashObserver
{
    //���� Ÿ�� HP �� UI
    [SerializeField] private Image _monsterTargetHpImage;
    //�������� ���� ǥ�ÿ� (�� ��������, ���� ����, �帥 �ð�)
    [SerializeField] private TextMeshProUGUI _stageInfo;
    //���� ��� ���� Ȯ�� ��
    [SerializeField] private TextMeshProUGUI _WalletInfo;

    protected override void Awake()
    {
        if (_instance == null)
        {
            _instance = this as UIManager;
            //UIManager�� �� ��ȯ�� �ʿ� ����
            //DontDestroyOnLoad(gameObject);
            init();
        }
        else
        {
            if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    private void Start()
    {
        //���޴����� ICashObserver ���� �ʿ�
    }

    private void OnDestroy()
    {
        //���޴��� ���� ���� �ʿ�
    }

    public void NotifyChangeGold(int leftWallet)
    {
        //UI �� �����
        _WalletInfo.text = leftWallet.ToString();
    }
}
