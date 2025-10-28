using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{

    private bool _isGameStart;

    //NewBehaviourScript.instance
    static NewBehaviourScript instance;

    /// <summary>
    /// 게임플레이 시작시 UI로 메서드호출
    /// </summary>
    public void GameStartUI()
    {
        //UI기능 호출
    }

    /// <summary>
    /// 게임플레이 시작시 UI로 메서드호출
    /// </summary>
    public void GameStartStageManager()
    {
        //UI기능 호출
    }

    //버튼이 눌렸을 때 수행할 기능을 제작하셔야 함, 여기가 이상민씨 작업의 끝
    public void NextStage()
    {
        Debug.Log("다음스테이지 이동");
        SceneManager.LoadScene("인게임 씬");
    }

    public void setisGameover()
    {
        _isGameStart = true;
    }

    //1. 이름으로 찾기
    
    //2. 태그로 찾기
    //3. 드래그로 찾기

}
