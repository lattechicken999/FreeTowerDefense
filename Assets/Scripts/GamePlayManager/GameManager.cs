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
        _isGameStart = true;
        SceneManager.LoadScene("InGame");

        StageManager.Instance.StageStart(); //스타트 버튼 누르면 실행되도록 Test코드
    }

    /// <summary>
    /// 게임 종료 버튼을 누르면 게임종료씬 호출
    /// </summary>
    public void GameEndButton()
    {
        Application.Quit();
    }

    /// <summary>
    /// 게임 메뉴 버튼을 누르면 게임메뉴씬 호출
    /// </summary>
    public void MainMenuButton()
    {
        _isGameStart = false;
        SceneManager.LoadScene("MainMenu");
    }

}
