using System;
using UnityEngine;

/// <summary>
/// ゲーム全体の進行、コイン枚数の管理、およびクリア判定を統括するマネージャーコンポーネント。
/// </summary>
public class GameManager : MonoBehaviour
{
    [Header("ゲームクリアの設定")]
    [SerializeField] private int targetCoins = 3; // クリアに必要な総コイン枚数

    private int coinCount = 0; // 現在のコイン枚数
    private bool isCleared = false; // クリア済みかどうかの変数

    // コイン枚数が変わったときのイベント（新しい枚数を渡す）
    public event Action<int> OnCoinCountChanged;

    // クリアしたときのイベント（渡すものなし）
    public event Action OnGameCleared;
    
    /// <summary>
    /// CoinItemから呼ばれ、コイン枚数を1増やして変化を通知する。目標枚数に達したらクリアを通知する。
    /// </summary>
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
}