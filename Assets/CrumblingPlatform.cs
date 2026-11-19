using System.Collections;
using UnityEngine;

/// <summary>
/// プレイヤーが乗ると一定時間後に点滅して崩れ（消滅）、安全を確認してかた復活する床ギミック。
/// </summary>
public class CrumblingPlatform : MonoBehaviour
{
    [Header("崩れるまでの設定")]
    [SerializeField] private float delayBeforeCrumble = 1.5f; // 乗ってから崩れるまでの秒数
    [SerializeField] private float respawnTime = 3.0f; // 復活するまでの秒数

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
        Debug.Log($"床に何か（{collision.gameObject.name}）がぶつかりました！");
        // 接触したオブジェクトがプレイヤーかつ、まだギミックが発動していない場合
        if (!isTriggered && collision.gameObject.TryGetComponent<TestScript>(out _))
        {
            isTriggered = true;
            StartCoroutine(CrumbleSequence());
        }
    }

    /// <summary>
    /// 点滅警告、消滅、プレイヤーとの重複チェックを伴う安全な復活を制御するコルーチン。
    /// </summary>
    private IEnumerator CrumbleSequence()
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

            yield return new WaitForSeconds(blinkInterval);
            timer += blinkInterval;
        }

        // 消滅処理の直前に、見た目を一瞬だけ確実にONに戻す
        if (meshRenderer != null) meshRenderer.enabled = true;

        // 床の当たり判定と見た目を完全に無効化する
        if (platformCollider != null) platformCollider.enabled = false;
        if (meshRenderer != null) meshRenderer.enabled = false;

        // 設定された復活時間を待つ
        yield return new WaitForSeconds(respawnTime);

        // 床のあった位置に、プレイヤーが重なっている間は復活を遅らせる
        while (Physics.CheckBox(transform.position, transform.localScale / 2f, transform.rotation))
        {
            // 毎フレームごとにチェックを行う
            yield return null;
        }

        // プレイヤーがいない状態になったので、床を復活させる
        if (platformCollider != null) platformCollider.enabled = true;
        if (meshRenderer != null) meshRenderer.enabled = true;
        
        // 次の接触を検知できるようにフラグをリセット
        isTriggered = false;
    }
}