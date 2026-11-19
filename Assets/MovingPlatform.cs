using UnityEngine;

/// <summary>
/// 指定された方向にオブジェクトを自動で往復移動させるコンポーネント。
/// 自身および子オブジェクト全体を移動させる役割を持つ。
/// レベルデザインにおいて、プレイヤーにタイミングを計らせるギミックとして使用。
/// </summary>

public class MovingPlatform : MonoBehaviour
{
    [Header("移動の設定")]
    [SerializeField] private float speed = 2.0f; // 往復する移動速度
    [SerializeField] private float distance = 5.0f; // 往復する最大距離
    [SerializeField] private Vector3 moveDirection = Vector3.right; // 移動する方向（ベクトル）

    private Vector3 startPosition; //ゲーム開始時の初期位置

    void Start()
    {
        // 往復の基準点とするため、開始時の座標をキャッシュ
        startPosition = transform.position;
    }

    void Update()
    {
        // Mathf.PingPongを用いて、0からdistanceの間を往復する値を計算
        float currentOffset = Mathf.PingPong(Time.time * speed, distance);
        // 初期位置に対して、指定された方向と移動量を掛け合わせて新しい位置を計算
        transform.position = startPosition + (moveDirection.normalized * currentOffset);
    }
}