using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 스테이지 정보 및 HP 정보, 지갑 정보 업데이트
/// 유닛 판매시 판매 UI 띄움
/// </summary>
public class UIManager : Singleton<UIManager>, ICashObserver
{
    //몬스터 타겟 HP 용 UI
    [SerializeField] private Image _monsterTargetHpImage;
    //스테이지 인포 표시용 (몇 스테이지, 남은 몬스터, 흐른 시간)
    [SerializeField] private TextMeshProUGUI _stageInfo;
    //남은 골드 갯수 확인 용
    [SerializeField] private TextMeshProUGUI _WalletInfo;

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
    }

    private void Start()
    {
        //골드메니저에 ICashObserver 구독 필요
    }

    private void OnDestroy()
    {
        //골드메니저 구독 해제 필요
    }

    public void NotifyChangeGold(int leftWallet)
    {
        //UI 가 변경됨
        _WalletInfo.text = leftWallet.ToString();
    }
}
