using UnityEngine;

/// <summary>
/// クリア表示を担当するUIコンポーネント
/// GameManager のイベントを購読し、クリアしたときだけ表示を更新する。
/// </summary>
public class ClearUI : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject clearTextUI;

    void OnEnable()
    {
        if (gameManager != null)
        {
            gameManager.OnGameCleared += ShowClearText;
        }
    }

    void OnDisable()
    {
        if (gameManager != null)
        {
            gameManager.OnGameCleared -= ShowClearText;
        }
    }

    private void ShowClearText()
    {
        clearTextUI.SetActive(true);
    }
}
