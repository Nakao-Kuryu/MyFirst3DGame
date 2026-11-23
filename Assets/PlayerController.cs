using UnityEngine;

/// <summary>
/// プレイヤーの移動、ジャンプを制御するコンポーネント。
/// 入力検知はUpdate、物理演算はFixedUpdateに分離し、フレームレート非依存の挙動を保証。
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("移動・ジャンプの設定")]
    [SerializeField] private float speed = 5.0f; // 移動時に加える力の大きさ
    [SerializeField] private float jumpForce = 5.0f; // ジャンプ時の瞬間的な上方向への力

    [Header("接地判定の設定")]
    [SerializeField] private float groundNormalThreshold = 0.7f; // 面の法線のyがこれより大きければ床とみなす

    [Header("参照")]
    [SerializeField] private GameManager gameManager;

    // 内部キャッシュ用の変数
    private Rigidbody rb;
    private bool isGrounded = true;

    // 入力値をUpdateとFixedUpdate間で共有するための変数
    private float moveH;
    private float moveV;
    private bool isJumpRequested = false;

    // 操作を受け付けるかどうか（クリア後は false）
    private bool canControl = true;

    void OnEnable()
    {
        // クリアの通知を購読
        if (gameManager != null)
        {
            gameManager.OnGameCleared += OnGameCleared;
        }
    }

    void OnDisable()
    {
        // 購読を解除
        if (gameManager != null)
        {
            gameManager.OnGameCleared -= OnGameCleared;
        }
    }

    void Start()
    {
        // 処理負荷軽減のため、コンポーネントを初期化時にキャッシュ
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError($"{gameObject.name} に Rigidbody がついていません！追加してください。");
            return;
        }
        // 静止中もOnCollisionStayを受け取って接地判定するため、スリープさせない
        rb.sleepThreshold = 0;
    }

    void Update()
    {
        if (rb == null || !canControl) return; // 操作できないときは入力を読まない

        // フレーム単位の取りこぼしを防ぐため、Update内で取得
        moveH = Input.GetAxis("Horizontal");
        moveV = Input.GetAxis("Vertical");

        // ジャンプキーが押されたフラグを立てる
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            isJumpRequested = true;
        }
    }

    void FixedUpdate()
    {
        if (rb == null) return;

        Vector3 movement = new Vector3(moveH, 0.0f, moveV);
        rb.AddForce(movement * speed);

        // フラグが立っていた場合のみ物理力を加える
        if (isJumpRequested)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJumpRequested = false; // トリガーを消費
        }

        // 次の物理計算の接触判定を任せるため、一旦「空中」に戻す。
        isGrounded = false;
    }

    /// <summary>
    /// 何かに触れ続けている間、物理演算のたびに呼ばれる。
    /// 触れている面の中に上向きの面（床）があれば接地とみなす
    /// </summary>
    void OnCollisionStay(Collision collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector3 normal = collision.GetContact(i).normal;

            if (normal.y > groundNormalThreshold)
            {
                isGrounded = true;
                return;
            }
        }
    }

    /// <summary>
    /// GameManager からクリアの通知を受けたときに呼ばれ、プレイヤーの操作を止める。
    /// </summary>
    private void OnGameCleared()
    {
        canControl = false;
        moveH = 0;
        moveV = 0;
        isJumpRequested = false;

        // 移動の勢いを止める
        rb.linearVelocity = Vector3.zero;
    }
}
