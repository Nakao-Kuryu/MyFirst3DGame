using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ステージ最下部に配置し、落下してきたプレイヤーを検知してリトライさせるコンポーネント。
/// </summary>
public class DeadZone : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        // 落下してきたオブジェクトがプレイヤー（TestScriptを持っている）かチェック
        if (other.TryGetComponent<TestScript>(out _))
        {
            Debug.Log("プレイヤーがステージから落下しました。初期位置に戻します。");

            // 現在アクティブなシーンの名前を取得し、即座にロード（リペア/リトライ処理）
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}