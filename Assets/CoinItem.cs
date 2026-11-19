using UnityEngine;

/// <summary>
/// ステージ上に配置されるコインの挙動を制御するコンポーネント。
/// プレイヤーとの接触判定、スコア加算のトリガー、および自身の消滅処理を行う。
/// </summary>
public class CoinItem : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        // 接触したオブジェクトがプレイヤー（TestScript）を持っているかチェックし、
        // 取得できた場合はそのまま変数 player にキャッシュ
        if (other.TryGetComponent<TestScript>(out var player))
        {
            //プレイヤー側のスコア加算処理を呼び出す。
            player.AddCoin();

            // コイン自体をシーンから削除
            Destroy(gameObject);
        }
    }
}