using UnityEngine;

/// <summary>
/// カメラをプレイヤーに対して滑らかに追従させるコンポーネント。
/// プレイヤーの移動完了後に追従させるため、LateUpdate を採用し画面のガタつきを防止。
/// </summary>
public class CameraFollow : MonoBehaviour
{
    [Header("追従の設定")]
    [SerializeField] private Transform target; // 追従対象のオブジェクト（プレイヤー）
    [SerializeField] private Vector3 offset; // ターゲットからの相対距離
    [SerializeField] private float smoothSpeed = 5.0f; // 亀田が追従する滑らかさ

    void LateUpdate()
    {
        // ターゲットが未設定（Null）の場合の安全対策
        if (target == null) return;

        // 目標とするカメラの座標を計算
        Vector3 desiredPosition = target.position + offset;

        // Vector3.Lerpを用いて、現在の位置から目標位置までを滑らかに補間
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // カメラの位置を更新
        transform.position = smoothedPosition;
        
    }
}