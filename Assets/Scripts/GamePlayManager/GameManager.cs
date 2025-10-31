using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private bool _isGameStart;
    private Vector3 _initCameraPosition;
    private Quaternion _initCameraRotation;
   
    protected override void init()
    {

    }
    /// <summary>
    /// ���ѹ��� ������ �ش�� ȣ��
    /// </summary>
    public void MoveScene(string sceneNum)
    {
        SceneManager.LoadScene(sceneNum);
    }

    /// <summary>
    /// ���� ���� ��ư�� ������ ���������� ȣ��
    /// </summary>
    public void GameStartButton()
    {
        _isGameStart = true;
        SceneManager.LoadScene("InGame");

        StageManager.Instance.StageStart(); //��ŸƮ ��ư ������ ����ǵ��� Test�ڵ�
    }

    /// <summary>
    /// ���� ���� ��ư�� ������ ��������� ȣ��
    /// </summary>
    public void GameEndButton()
    {
        Application.Quit();
    }

    /// <summary>
    /// ���� �޴� ��ư�� ������ ���Ӹ޴��� ȣ��
    /// </summary>
    public void MainMenuButton()
    {
        _isGameStart = false;
        SceneManager.LoadScene("MainMenu");
    }

}
