using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SellButton : MonoBehaviour
{
    // 선택된 기물
    [field:SerializeField] public GameObject SelectedPeice {  get; set; }
    Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        //활성화 시 클릭한 기물의 이름을 가져와서 판매 함수에 등록
        //_button.onClick.AddListener();
    }
    /// <summary>
    /// 기물을 클릭했을 때 판매 버튼의 위치를 조정해주는 메서드
    /// </summary>
    /// <param name="position">UI를 기물위에 띄우기 위한 위치</param>
    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

}
