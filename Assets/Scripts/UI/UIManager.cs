using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �������� ���� �� HP ����, ���� ���� ������Ʈ
/// ���� �ǸŽ� �Ǹ� UI ���
/// </summary>
public partial class UIManager : Singleton<UIManager>, ICashObserver
{
    //���� Ÿ�� HP �� UI
    [SerializeField] private Image _monsterTargetHpImage;
    //�������� ���� ǥ�ÿ� (�� ��������, ���� ����, �帥 �ð�)
    [SerializeField] private TextMeshProUGUI _stageInfo;
    //���� ��� ���� Ȯ�� ��
    [SerializeField] private TextMeshProUGUI _WalletInfo;
    [SerializeField] private Canvas _sellUiPrefeb;

    //�⹰ ���̾� (����ĳ��Ʈ�� ���)
    [SerializeField] LayerMask _layerMask;

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
                    //_sellUiButton.onClick = hit.GetComponet()?.sell 
                }
            }
            else
            {
                _selectedUnit = null;
                _sellUiInst.enabled = false;
                _sellUiButton.onClick = null;
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
    }

    private void Start()
    {
        //���޴����� ICashObserver ���� �ʿ�
        GoldManager.Instance.RegistrationObserver(this);
    }
    private void Update()
    {

        GetSelectUnit();


    }
    private void OnDestroy()
    {
        //���޴��� ���� ���� �ʿ�
        GoldManager.Instance.UnregisterObserver(this);
        _sellUiInst = null;
    }
}