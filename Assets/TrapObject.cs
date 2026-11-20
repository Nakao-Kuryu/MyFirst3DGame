using UnityEngine;

/// <summary>
/// ステージ上の障害物（罠）の挙動を制御するコンポーネント。
/// プレイヤーとの接触を検知した際、リトライを依頼する。
/// </summary>
public class TrapObject : MonoBehaviour
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
        // 接触したオブジェクトがプレイヤー（PlayerController）であるか判定
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