using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private bool _isGameStart;

    /// <summary>
    /// 씬넘버를 받으면 해당씬 호출
    /// </summary>
    public void MoveScene(string sceneNum)
    {
        SceneManager.LoadScene(sceneNum);
    }

    /// <summary>
    /// 게임 시작 버튼을 누르면 스테이지씬 호출
    /// </summary>
    public void GameStartButton()
    {
        SceneManager.LoadScene("Stage");
    }

    /// <summary>
    /// 게임 종료 버튼을 누르면 게임종료씬 호출
    /// </summary>
    public void GameEndButton()
    {
        SceneManager.LoadScene("GameEnd");
    }
}
