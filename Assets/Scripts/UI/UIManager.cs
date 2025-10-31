using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 스테이지 정보 및 HP 정보, 지갑 정보 업데이트
/// 유닛 판매시 판매 UI 띄움
/// </summary>
public partial class UIManager : Singleton<UIManager>, ICashObserver,IStageInfo
{
    //몬스터 타겟 HP 용 UI
    [SerializeField] private Image _monsterTargetHpImage;
    //스테이지 인포 표시용 (몇 스테이지, 남은 몬스터, 흐른 시간)
    [SerializeField] private TextMeshProUGUI _stageInfo;
    //남은 골드 갯수 확인 용
    [SerializeField] private TextMeshProUGUI _WalletInfo;
    [SerializeField] private Canvas _sellUiPrefeb;

    //유닉 구매 버튼
    [SerializeField] private Button _buyUiWarrior;
    [SerializeField] private Button _buyUiWizard;

    //기물 레이어 (레이캐스트에 사용)
    [SerializeField] LayerMask _layerMask;

    //메인메뉴로 돌아가기
    [SerializeField] private Button _returnMenuButton;

    private GameObject _selectedUnit;
    private Camera _camera;
    private Canvas _sellUiInst;
    private Transform _sellUiButtonTransform;
    private Button _sellUiButton;

    public void NotifyChangeGold(int leftWallet)
    {
        //UI 가 변경됨
        _WalletInfo.text = leftWallet.ToString();
    }

    public void NotifyStageInfo(string Text)
    {
        _stageInfo.text = Text;
    }


    //레이로 유닛 선택 시 Sell UI 버튼이 보이게 하는 기능 필요
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

                    //온클릭 이벤트에 유닛 판매 명령 전달
                    //이전에 등록된 리스너 있다면 삭제
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
    /// sell 용 UI 캔버스 미리 생성해 두기
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
            //UIManager는 씬 전환시 필요 없음
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

        //골드메니저에 ICashObserver 구독 필요
        GoldManager.Instance.RegistrationObserver(this);
        //스테이지 인포 구독 필요
        StageManager.Instance.SubscribeStageInfo(this);
    }

    private void Start()
    {
        //버튼이벤트 연결
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
        //골드메니저 구독 해제 필요
        GoldManager.Instance.UnregisterObserver(this);
        _sellUiInst = null;
    }
}