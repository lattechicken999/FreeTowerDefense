using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �������� ���� �� HP ����, ���� ���� ������Ʈ
/// ���� �ǸŽ� �Ǹ� UI ���
/// </summary>
public partial class UIManager : Singleton<UIManager>, ICashObserver,IStageInfo
{
    //���� Ÿ�� HP �� UI
    [SerializeField] private Image _monsterTargetHpImage;
    //�������� ���� ǥ�ÿ� (�� ��������, ���� ����, �帥 �ð�)
    [SerializeField] private TextMeshProUGUI _stageInfo;
    //���� ��� ���� Ȯ�� ��
    [SerializeField] private TextMeshProUGUI _WalletInfo;
    [SerializeField] private Canvas _sellUiPrefeb;

    //���� ���� ��ư
    [SerializeField] private Button _buyUiWarrior;
    [SerializeField] private Button _buyUiWizard;

    //�⹰ ���̾� (����ĳ��Ʈ�� ���)
    [SerializeField] LayerMask _layerMask;

    //���θ޴��� ���ư���
    [SerializeField] private Button _returnMenuButton;

    private GameObject _selectedUnit;
    private Camera _camera;
    private Canvas _sellUiInst;
    private Transform _sellUiButtonTransform;
    private Button _sellUiButton;

    public void NotifyChangeGold(int leftWallet)
    {
        //UI �� �����
        _WalletInfo.text = leftWallet.ToString();
    }

    public void NotifyStageInfo(string Text)
    {
        _stageInfo.text = Text;
    }


    //���̷� ���� ���� �� Sell UI ��ư�� ���̰� �ϴ� ��� �ʿ�
    private void GetSelectUnit()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_selectedUnit == null)
            {
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, 100f,_layerMask))
                {
                    _selectedUnit = hit.transform.gameObject;
                    
                    _sellUiInst.enabled = true;

                    _sellUiButtonTransform.position = _camera.WorldToScreenPoint(_selectedUnit.transform.position);

                    //��Ŭ�� �̺�Ʈ�� ���� �Ǹ� ��� ����
                    //������ ��ϵ� ������ �ִٸ� ����
                    _sellUiButton.onClick.RemoveAllListeners();
                    _sellUiButton.onClick.AddListener(_selectedUnit.transform.GetComponent<Piece>().SellPiece);
                }
            }
            else
            {
                _selectedUnit = null;
                //_sellUiInst.enabled = false;
            }
        }
    }

    /// <summary>
    /// sell �� UI ĵ���� �̸� ������ �α�
    /// </summary>
    private void InitSellUiCanvas()
    {
        _sellUiInst = Instantiate(_sellUiPrefeb);
        _sellUiInst.enabled = false;
        _sellUiButtonTransform = _sellUiInst.transform.GetChild(0);
        _sellUiButton = _sellUiButtonTransform.GetComponent<Button>();
    }
}

public partial class UIManager : Singleton<UIManager>, ICashObserver
{
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
        InitSellUiCanvas();
        _camera = Camera.main;

        //���޴����� ICashObserver ���� �ʿ�
        GoldManager.Instance.RegistrationObserver(this);
        //�������� ���� ���� �ʿ�
        StageManager.Instance.SubscribeStageInfo(this);
    }

    private void Start()
    {
        //��ư�̺�Ʈ ����
        _buyUiWarrior?.onClick.AddListener(() => PlaceablePointsCheck.Instance.CommandChackPlaceable(UnitEnum.Warrior));
        _buyUiWizard?.onClick.AddListener(() => PlaceablePointsCheck.Instance.CommandChackPlaceable(UnitEnum.Wizard));
        _returnMenuButton?.onClick.AddListener(GameManager.Instance.MainMenuButton);

        NotifyChangeGold(GoldManager.Instance.Wallet);
    }
    private void Update()
    {

        GetSelectUnit();


    }
    private void OnDisable()
    {
        //���޴��� ���� ���� �ʿ�
        GoldManager.Instance.UnregisterObserver(this);
        _sellUiInst = null;
    }
}