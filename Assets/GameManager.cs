using System;
using UnityEngine;

/// <summary>
/// ゲーム全体の進行、クリア判定、およびオーディオ再生を統括するマネージャーコンポーネント。
/// </summary>
public class GameManager : MonoBehaviour
{
    [Header("ゲームクリアの設定")]
    [SerializeField] private int targetCoins = 3; // クリアに必要な総コイン枚数
    private int coinCount = 0; // 現在のコイン枚数

    private bool isCleared = false; // クリアを判定する変数

    [SerializeField] private GameObject clearTextUI; // クリア時に表示するUIオブジェクト

    [Header("オーディオの設定")]
    [SerializeField] private AudioClip clearSound; // クリア時のファンファーレ音
    [SerializeField] private AudioClip coinSound; // コイン獲得時の効果音

    // 内部キャッシュ用のコンポーネント
    public AudioSource audioSource;

    // コイン枚数が変わったときのイベント（新しい枚数を渡す）
    public event Action<int> OnCoinCountChanged;

    // クリアしたときのイベント（渡すものなし）
    public event Action OnGameCleared;

    void Start()
    {
        // 処理負荷軽減と安全性確保のため、同一オブジェクトからAudioSourceをキャッシュ
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError($"{gameObject.name} に AudioSource コンポーネントがついていません！追加してください。");
        }
    }
    

    public void AddCoin()
    {
        coinCount++;

        // コイン枚数が変わったときのイベントを発行
        OnCoinCountChanged?.Invoke(coinCount);

        // コインが目標枚数に達していて、一度もクリアしていなかったら
        if (coinCount >= targetCoins && !isCleared)
        {
            isCleared = true;
            OnGameCleared?.Invoke();
        }
    }
    /// <summary>
    /// コインの獲得枚数を評価し、規定枚数に達していればクリア演出を実行する。
    /// </summary>
    /// <param name="currentCoins">現在のプレイヤーのコイン所持枚数</param>
    public void CheckClear(int currentCoins)
    {
        // コインを獲得したため、効果音を再生
        PlayCoinSound();

        // クリア条件の判定
        if (currentCoins >= targetCoins)
        {
            if (clearTextUI != null)
            {
                clearTextUI.SetActive(true);
            }
            if (audioSource != null && clearSound != null)
            {
                audioSource.PlayOneShot(clearSound);
            }
        }
    }

    /// <summary>
    /// コイン獲得時のSEを単発再生する内部関数
    /// </summary>
    public void PlayCoinSound()
    {
        if (audioSource != null && coinSound != null)
        {
            audioSource.PlayOneShot(coinSound);
        }   
    }

    
}