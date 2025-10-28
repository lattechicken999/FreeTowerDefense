using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private bool _isGameStart;

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
        SceneManager.LoadScene("Stage");
    }

    /// <summary>
    /// ���� ���� ��ư�� ������ ��������� ȣ��
    /// </summary>
    public void GameEndButton()
    {
        SceneManager.LoadScene("GameEnd");
    }
}
