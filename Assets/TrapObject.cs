using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ステージ上の障害物（罠）の挙動を制御するコンポーネント
/// プレイヤーとの接触を検知した際、即座に現在のシーンを再読込（リトライ）。
/// </summary>
public class TrapObject : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        // 接触したオブジェクトがプレイヤー（PlayerController）であるか判定
        if (other.TryGetComponent<PlayerController>(out _))
        {
            // 現在アクティブなシーンの名前を取得し、即座にロード（リペア/リトライ処理）
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}