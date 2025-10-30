using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �⹰ ��ġ���� �⹰�� ��ġ �������� �˷��ִ� ���
/// </summary>
public partial class PlaceablePointsCheck : Singleton<PlaceablePointsCheck>
{

    private Dictionary<Transform,int> _childsDict;

    //��ġ �������� �����ϴ� ���� �迭
    private List<bool> _placeableStates;

    //�������� �������� Ȯ���ϴ� ���� ��
    private bool _isPlaceState;

    //���� ī�޶��
    private Camera _camera;

    //��ġ ������ ��ҿ� ����Ʈ ǥ�ÿ�
    private GameObject _effect;

    //���콺�� �÷��� ���� ��ġ ������ ���̶��, ������ �ε��� ����
    private int _selectedPlaceablPoint;

    //���� ��û�� Ÿ�� ���� �뵵
    private UnitEnum _pieceType;

    //place �� �����ɽ�Ʈ �ɼ� �ֵ��� ���̾� ����Ʈ
    [SerializeField] private LayerMask _placeLayerMask;
    //��ġ ������ ��Ұ� ������ ǥ���ϴ� ����Ʈ
    [SerializeField] private GameObject _placeEffect;
    //��ġ ����Ʈ ��ġ ��¦ ���� �ø��� �� ��
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


        //���� ��� �ڽ� ��������
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
                //���õ� ���� ������ �׳� ����
                return;
            }
            //�⹰ ��ġ �Ұ����ϰ� �ٲ�
            _placeableStates[_selectedPlaceablPoint] = false;
            //�⹰ ���� ����� �����ʿ��� ����

            //����Ʈ ����
            _effect.SetActive(false);
            ComplateChackPlaceable();
        }
    }
}



public partial class PlaceablePointsCheck : Singleton<PlaceablePointsCheck>
{
    private void Start()
    {
        _effect.SetActive(false);
    }
    private void Update()
    {
        if (_isPlaceState) 
        {
            CheckPlaceable();
            SendToCreatePieceCommand();
        }
    }
}
