using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    private Button _startButton;
    private void Awake()
    {
        _startButton = GetComponent<Button>();
    }
    // Start is called before the first frame update
    void Start()
    {
        _startButton.onClick.AddListener(GameManager.Instance.GameStartButton);
    }
}
