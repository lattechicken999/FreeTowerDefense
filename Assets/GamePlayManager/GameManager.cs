using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool _isGameStart;

    //GameManager.instance
    static GameManager instance;
    //�̱������� �ۼ��غ���


    /// <summary>
    /// �����÷��� ���۽� UI�� �޼���ȣ��
    /// </summary>
    public void GameStartUI()
    {
        //UI��� ȣ��
    }

    /// <summary>
    /// �����÷��� ���۽� UI�� �޼���ȣ��
    /// </summary>
    public void GameStartStageManager()
    {
        //UI��� ȣ��
    }

    //��ư�� ������ �� ������ ����� �����ϼž� ��, ���Ⱑ �̻�ξ� �۾��� ��
    public void NextStage()
    {
        Debug.Log("������������ �̵�");
        SceneManager.LoadScene("�ΰ��� ��");
    }

    public void setisGameover()
    {
        _isGameStart = true;
    }

    //1. �̸����� ã��

    //2. �±׷� ã��
    //3. �巡�׷� ã��
}
