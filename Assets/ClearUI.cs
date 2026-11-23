using UnityEngine;

/// <summary>
/// クリア画面を担当するUIコンポーネント
/// GameManager のイベントを購読し、クリアしたときにクリア画面を表示する。
/// </summary>
public class ClearUI : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject clearPanel;

    void OnEnable()
    {
        if (gameManager != null)
        {
            gameManager.OnGameCleared += ShowClearPanel;
        }
    }

    void OnDisable()
    {
        if (gameManager != null)
        {
            gameManager.OnGameCleared -= ShowClearPanel;
        }
    }

    private void ShowClearPanel()
    {
        if (clearPanel != null)
        {
            clearPanel.SetActive(true);
        }
    }
}
