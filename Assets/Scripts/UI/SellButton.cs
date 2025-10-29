using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SellButton : MonoBehaviour
{
    // ���õ� �⹰
    [field:SerializeField] public GameObject SelectedPeice {  get; set; }
    Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        //Ȱ��ȭ �� Ŭ���� �⹰�� �̸��� �����ͼ� �Ǹ� �Լ��� ���
        //_button.onClick.AddListener();
    }
    /// <summary>
    /// �⹰�� Ŭ������ �� �Ǹ� ��ư�� ��ġ�� �������ִ� �޼���
    /// </summary>
    /// <param name="position">UI�� �⹰���� ���� ���� ��ġ</param>
    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

}
