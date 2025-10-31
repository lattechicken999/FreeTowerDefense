using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 기물 배치에서 기물이 배치 가능한지 알려주는 기능
/// </summary>
public partial class PlaceablePointsCheck : Singleton<PlaceablePointsCheck>
{

    private Dictionary<Transform,int> _childsDict;

    //배치 가능한지 저장하는 상태 배열
    private List<bool> _placeableStates;

    //선택중인 상태인지 확인하는 상태 값
    private bool _isPlaceState;

    //메인 카메라용
    private Camera _camera;

    //배치 가능한 장소에 이펙트 표시용
    private GameObject _effect;

    //마우스를 올려둔 곳이 배치 가능한 곳이라면, 가져올 인덱스 정보
    private int _selectedPlaceablPoint;

    //선택된 곳의 위치 정보
    private Transform _selectedPointTransform;

    //생성 요청한 타입 저장 용도
    private UnitEnum _pieceType;

    //place 만 레이케스트 될수 있도록 레이어 마스트
    [SerializeField] private LayerMask _placeLayerMask;
    //배치 가능한 장소가 맞으면 표시하는 이펙트
    [SerializeField] private GameObject _placeEffect;
    //배치 이펙트 위치 살짝 위로 올리기 용 갭
    [SerializeField] private Vector3 _effectGap;

    protected override void Awake()
    {
        if (_instance == null)
        {
            _instance = this as PlaceablePointsCheck;
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

    protected override void init()
    {
        _childsDict = new Dictionary<Transform, int>();
        _placeableStates = new List<bool>();
        _camera = Camera.main;
        _effect = Instantiate(_placeEffect);
        _selectedPlaceablPoint = -1;
        _selectedPointTransform = null;

        //하위 모든 자식 가져오기
        for (int i =0;i < transform.childCount;i++)
        {
            _childsDict.Add(transform.GetChild(i), i);
            _placeableStates.Add(true);
        }

    }

    public void CommandChackPlaceable(UnitEnum type)
    {
        _isPlaceState = true;
        _pieceType = type;
    }
    private void ComplateChackPlaceable()
    {
        _isPlaceState = false;
        _pieceType = UnitEnum._None;

    }
    private void CheckPlaceable()
    {

        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, _placeLayerMask))
        {
            if(_placeableStates[_childsDict[hit.transform]])
            {
                _effect.transform.position = hit.transform.position + _effectGap;
                _effect.SetActive(true);
                _selectedPlaceablPoint = _childsDict[hit.transform];
                _selectedPointTransform = hit.transform;
            }
            else
            {
                _effect.SetActive(false);
            }
        }
        else
        {
            _effect.SetActive(false);
            _selectedPlaceablPoint = -1;
        }
    }

    private void SendToCreatePieceCommand()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(_selectedPlaceablPoint < 0 )
            {
                //선택된 곳이 없으면 그냥 종료
                return;
            }
            //기물 배치 불가능하게 바꿈
            _placeableStates[_selectedPlaceablPoint] = false;
            //기물 생성 명령을 스포너에게 보냄
            switch(_pieceType)
            {
                case UnitEnum.Warrior:
                    _placeableStates[_selectedPlaceablPoint] = !PieceSpawner.Instance.CreateWarrior(_selectedPointTransform.position);
                    break;
                case UnitEnum.Wizard:
                    _placeableStates[_selectedPlaceablPoint] = !PieceSpawner.Instance.CreateWizard(_selectedPointTransform.position);
                    break;
                default:
                    break;
            }
            

            //이펙트 종료
            _effect.SetActive(false);
            ComplateChackPlaceable();
        }
    }
    public void SellingComplate()
    {
        _placeableStates[_childsDict[_selectedPointTransform]] = true;
        _selectedPointTransform = null;
    }
}



public partial class PlaceablePointsCheck : Singleton<PlaceablePointsCheck>
{
    private void Start()
    {
        _effect.SetActive(false);
    }

    private void LateUpdate()
    {
        //오브젝트 생성과 동시에 Sell 버튼 체크가 활성화 되어 생성되는 로직 순서를 맨 뒤로 미룸
        if (_isPlaceState)
        {
            CheckPlaceable();
            SendToCreatePieceCommand();
        }
    }
}
