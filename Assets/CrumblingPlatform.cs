using System;
using UnityEngine;

/// <summary>
/// プレイヤーが乗ると一定時間後に点滅して崩れ（消滅）、安全を確認してから復活する床ギミック。
/// </summary>
public class CrumblingPlatform : MonoBehaviour
{
    [Header("崩れるまでの設定")]
    [SerializeField] private float delayBeforeCrumble = 1.5f; // 乗ってから崩れるまでの秒数
    [SerializeField] private float respawnTime = 3.0f; // 復活するまでの秒数

    [Header("復活時の重なりチェック")]
    [SerializeField] private LayerMask playerLayer; // 重なりを調べる対象のレイヤー（Player）

    // 内部キャッシュおよび状態管理用の変数
    private Collider platformCollider;
    private MeshRenderer meshRenderer;
    private bool isTriggered = false;

    void Start()
    {
        // 処理負荷軽減のため、コンポーネントをキャッシュ
        platformCollider = GetComponent<Collider>();
        meshRenderer = GetComponent<MeshRenderer>();

        if (platformCollider == null || meshRenderer == null)
        {
            Debug.LogError($"{gameObject.name} に必要なコンポーネントが見つかりません。");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // 接触したオブジェクトがプレイヤーかつ、まだギミックが発動していない場合
        if (!isTriggered && collision.gameObject.TryGetComponent<PlayerController>(out _))
        {
            isTriggered = true;
            _ = CrumbleSequenceAsync();
        }
    }

    /// <summary>
    /// 点滅警告、消滅、プレイヤーとの重複チェックを伴う安全な復活を制御する非同期処理。
    /// </summary>
    private async Awaitable CrumbleSequenceAsync()
    {
        try
        {
            float timer = 0f;
            float blinkInterval = 0.1f; // 点滅の間隔

            // 指定された時間が経過するまで、見た目を反転させて高速点滅させる
            while (timer < delayBeforeCrumble)
            {
                if (meshRenderer != null)
                {
                    meshRenderer.enabled = !meshRenderer.enabled;
                }

                await Awaitable.WaitForSecondsAsync(blinkInterval, destroyCancellationToken);
                timer += blinkInterval;
            }

            // 床の当たり判定と見た目を完全に無効化する
            if (platformCollider != null) platformCollider.enabled = false;
            if (meshRenderer != null) meshRenderer.enabled = false;

            // 設定された復活時間を待つ
            await Awaitable.WaitForSecondsAsync(respawnTime, destroyCancellationToken);

            // 床のあった位置に、プレイヤーが重なっている間は復活を遅らせる
            while (Physics.CheckBox(transform.position, transform.localScale / 2f, transform.rotation, playerLayer))
            {
                // 毎フレームごとにチェックを行う
                await Awaitable.NextFrameAsync(destroyCancellationToken);
            }

            // プレイヤーがいない状態になったので、床を復活させる
            if (platformCollider != null) platformCollider.enabled = true;
            if (meshRenderer != null) meshRenderer.enabled = true;
        
            // 次の接触を検知できるようにフラグをリセット
            isTriggered = false;
        }
        catch (OperationCanceledException)
        {
            // 床が破棄された（シーンの読み直しなど）ため、処理を中断してなにもせず終了する
        }
    }
}