using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    
    [SerializeField] private Button startButton;
    [SerializeField] private Button exitButton;
    // [SerializeField] private GameManager gameManager;

    private void Start()
    {
        // 버튼 클릭 이벤트 등록
        startButton.onClick.AddListener(OnStartButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);
    }
    // GameManager에게 씬 전환 요청
    private void OnStartButtonClicked()
    {        
       // gameManager.LoadGameScene();
    }
    private void OnExitButtonClicked()
    {
       // gameManager.QuitGame();
    }



}
    

