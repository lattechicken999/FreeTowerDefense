using System.Collections;
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
    [SerializeField] LayerMask _pieceLayerMask;
    [SerializeField] LayerMask _uiLayerMask;

    //메인메뉴로 돌아가기
    [SerializeField] private Button _returnMenuButton;
    //게임 클리어, 실패 버튼저장용
    [SerializeField] private GameObject _clearButton;
    [SerializeField] private GameObject _failButton;

    [SerializeField] private GameObject _monsterTarget;

    private GameObject _selectedUnit;
    private Camera _camera;
    private Canvas _sellUiInst;
    private Transform _sellUiButtonTransform;
    private Button _sellUiButton;
    private Ray ray;
    private int _targetWallet;
    private int _curWallet;
    private WaitForSeconds _waitSeconds;
    private Coroutine _walletUpdateCuroutine;


    public void NotifyChangeGold(int leftWallet)
    {
        //UI 가 변경됨
        //_WalletInfo.text = leftWallet.ToString();
        _targetWallet = leftWallet;
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
            ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (_selectedUnit == null)
            {
                if (Physics.Raycast(ray, out RaycastHit hit, 100f,_pieceLayerMask))
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

    /// <summary>
    /// 1단위씩 오르고 내리게 수정
    /// </summary>
    IEnumerator SlowRigingWallet()
    {
        while (true)
        {
            if (_curWallet > _targetWallet)
            {
                _curWallet--;
                _WalletInfo.text = (_curWallet).ToString();
            }
            else if (_curWallet < _targetWallet)
            {
                _curWallet++;
                _WalletInfo.text = (_curWallet).ToString();
            }
            yield return _waitSeconds;
        }
    }
    public void SetGameClear()
    {
        _clearButton.SetActive(true);
        _clearButton.GetComponent<Button>().onClick.AddListener(GameManager.Instance.MainMenuButton);
    }
    public void SetGameFail()
    {
        _failButton.SetActive(true);
        _failButton.GetComponent<Button>().onClick.AddListener(GameManager.Instance.MainMenuButton);
    }
    public void SetMonsterTargetHpUi(float hpPersent)
    {
        _monsterTargetHpImage.fillAmount = hpPersent;
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
        _curWallet = 0;
        _targetWallet = 0;
        
    }

    private void Start()
    {
        //골드메니저에 ICashObserver 구독 필요
        GoldManager.Instance.RegistrationObserver(this);
        //스테이지 인포 구독 필요
        StageManager.Instance.SubscribeStageInfo(this);
        //버튼이벤트 연결
        _buyUiWarrior?.onClick.AddListener(() => PlaceablePointsCheck.Instance.CommandChackPlaceable(UnitEnum.Warrior));
        _buyUiWizard?.onClick.AddListener(() => PlaceablePointsCheck.Instance.CommandChackPlaceable(UnitEnum.Wizard));
        _returnMenuButton?.onClick.AddListener(GameManager.Instance.MainMenuButton);

        NotifyChangeGold(GoldManager.Instance.Wallet);
        //지갑 업데이트 용 코루틴
        _waitSeconds = new WaitForSeconds(0.02f);
        _walletUpdateCuroutine = StartCoroutine(SlowRigingWallet());

        //_clearButton = transform.Find("GameClearButton").gameObject;
        //_failButton = transform.Find("GameFailButton").gameObject;
        _clearButton.SetActive(false);
        _failButton.SetActive(false);

        _monsterTarget.GetComponent<MonsterTarget>()._monsterTargetHpChanged += SetMonsterTargetHpUi;
    }
    private void Update()
    {
        GetSelectUnit();
        SlowRigingWallet();
    }
    private void OnDisable()
    {
        //골드메니저 구독 해제 필요
        GoldManager.Instance.UnregisterObserver(this);
        _sellUiInst = null;

        //코루틴 해제
        StopCoroutine(_walletUpdateCuroutine);
    }
}