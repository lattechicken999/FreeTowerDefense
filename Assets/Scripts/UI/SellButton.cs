using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SellButton : MonoBehaviour
{
    private Canvas _canvas;
    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
    }
    /// <summary>
    /// 기물을 클릭했을 때 판매 버튼의 위치를 조정해주는 메서드
    /// </summary>
    /// <param name="position">UI를 기물위에 띄우기 위한 위치</param>
    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }
    public void ComplateSelling()
    {
        _canvas.enabled = false;
    }
}
