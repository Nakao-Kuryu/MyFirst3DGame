using UnityEngine;

/// <summary>
/// ステージ最下部に配置し、落下してきたプレイヤーを検知してリトライさせるコンポーネント。
/// </summary>
public class DeadZone : MonoBehaviour
{
    // GameManager をキャッシュしておく変数
    private GameManager gameManager;

    void Start()
    {
        // シーン内から GameManager を1つ探す
        gameManager = FindAnyObjectByType<GameManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        // 落下してきたオブジェクトがプレイヤー（PlayerControllerを持っている）かチェック
        if (other.TryGetComponent<PlayerController>(out _))
        {
            // GameManager にリトライ処理を依頼
            if (gameManager != null)
            {
                gameManager.RestartStage();
            }
        }
    }
}