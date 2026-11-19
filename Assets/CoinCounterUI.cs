using TMPro;
using UnityEngine;

/// <summary>
/// コイン枚数の表示を担当するUIコンポーネント
/// GameManager のイベントを購読し、枚数が変わったときだけ表示を更新する。
/// </summary>
public class CoinCounterUI : MonoBehaviour
{
    // UIは1つだけなので、CoinItem と違って Inspector で割り当てる
    [SerializeField] private GameManager gameManager;
    [SerializeField] private TextMeshProUGUI scoreText;

    void OnEnable()
    {
        if (gameManager != null)
        {
            gameManager.OnCoinCountChanged += UpdateText;
        }
    }

    void OnDisable()
    {
        if (gameManager != null)
        {
            gameManager.OnCoinCountChanged -= UpdateText;
        }
    }

    private void UpdateText(int coinCount)
    {
        scoreText.text = "COINS: " + coinCount.ToString();
    }
}
