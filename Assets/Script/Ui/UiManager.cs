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
        // ��ư Ŭ�� �̺�Ʈ ���
        startButton.onClick.AddListener(OnStartButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);
    }
    // GameManager���� �� ��ȯ ��û
    private void OnStartButtonClicked()
    {        
       // gameManager.LoadGameScene();
    }
    private void OnExitButtonClicked()
    {
       // gameManager.QuitGame();
    }



}
    

