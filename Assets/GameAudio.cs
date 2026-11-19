using UnityEngine;

/// <summary>
/// ゲーム中の効果音を担当するコンポーネント
/// GameManager のイベントを購読し、コイン獲得時とクリア時に音を鳴らす。
/// </summary>
public class GameAudio : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private AudioSource audioSource;

    [Header("効果音")]
    [SerializeField] private AudioClip coinSound; // コイン獲得時の効果音
    [SerializeField] private AudioClip clearSound; // クリア時のファンファーレ

    void OnEnable()
    {
        if (gameManager != null)
        {
            gameManager.OnCoinCountChanged += PlayCoinSound;
            gameManager.OnGameCleared += PlayClearSound;
        }
    }

    void OnDisable()
    {
        if (gameManager != null)
        {
            gameManager.OnCoinCountChanged -= PlayCoinSound;
            gameManager.OnGameCleared -= PlayClearSound;
        }
    }

    // OnCoinCountChanged は Action<int> なので、int を受け取る形にする必要がある
    private void PlayCoinSound(int coinCount)
    {
        if (audioSource != null && coinSound != null)
        {
            audioSource.PlayOneShot(coinSound);
        }
    }

    private void PlayClearSound()
    {
        if (audioSource != null && clearSound != null)
        {
            audioSource.PlayOneShot(clearSound);
        }
    }
}
