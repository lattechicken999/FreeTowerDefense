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
    /// �⹰�� Ŭ������ �� �Ǹ� ��ư�� ��ġ�� �������ִ� �޼���
    /// </summary>
    /// <param name="position">UI�� �⹰���� ���� ���� ��ġ</param>
    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }
    public void ComplateSelling()
    {
        _canvas.enabled = false;
    }
}
