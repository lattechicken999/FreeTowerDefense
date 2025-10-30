using UnityEngine;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour
{
    private Button _exitButton;
    private void Awake()
    {
        _exitButton = GetComponent<Button>();
    }
    // Start is called before the first frame update
    void Start()
    {
        _exitButton.onClick.AddListener(GameManager.Instance.GameEndButton);
    }

}
