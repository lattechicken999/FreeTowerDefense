using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePlayManager : MonoBehaviour
{

    private bool _isGameStart;

    //NewBehaviourScript.instance
    static GamePlayManager instance;

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
