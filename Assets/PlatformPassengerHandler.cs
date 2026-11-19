using UnityEngine;

/// <summary>
/// 動く床の子オブジェクトにアタッチし、乗ってきたオブジェクトの親子関係を制御する。
/// スケール不均等による変形バグを防ぐため、親のRootオブジェクトを乗客の親として設定する。
/// </summary>
public class PlatformPassengerHandler : MonoBehaviour
{
    private Transform rootTransform;

    void Start()
    {
        rootTransform = transform.parent;

        if (rootTransform == null)
        {
            Debug.LogError($"{gameObject.name} の親オブジェクト (Root) が見つかりません。階層構造を確認してください。");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // 接触したオブジェクトが「Rigidbody」を持っている場合
        if (collision.gameObject.TryGetComponent<Rigidbody>(out _))
        {
            // プレイヤーの親を自分（MovingPlatform）ではなく、
            // 親（rootTransform）に指定する
            collision.transform.SetParent(rootTransform);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // 床から離れたオブジェクトの親が、Rootだった場合は親子関係を解除する。
        if (collision.transform.parent == rootTransform)
        {
            collision.transform.SetParent(null);
        }
    }
}