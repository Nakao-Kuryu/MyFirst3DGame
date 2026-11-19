using UnityEngine;

/// <summary>
/// ステージ上に配置されるコインの挙動を制御するコンポーネント。
/// プレイヤーとの接触判定、スコア加算のトリガー、および自身の消滅処理を行う。
/// </summary>
public class CoinItem : MonoBehaviour
{
    // GameManager をキャッシュしておく変数
    private GameManager gameManager;

    void Start()
    {
        // シーン内から GameManager を1つ探す
        // （FindObjectOfType は Unity 6 では非推奨なので、こちらを使う)
        gameManager = FindAnyObjectByType<GameManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        // 触れたのがプレイヤーかどうかだけ判定する（中身は使わないので out _）
        if (other.TryGetComponent<PlayerController>(out _))
        {
            // GameManager にコイン獲得を伝える（null なら呼ばない）
            if (gameManager != null)
            {
                gameManager.AddCoin();
            }

            // コイン自体をシーンから削除
            Destroy(gameObject);
        }
    }
}