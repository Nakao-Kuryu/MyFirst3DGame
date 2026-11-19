using UnityEngine;
using TMPro;

/// <summary>
/// プレイヤーの移動、ジャンプ、およびスコア管理を制御するコンポーネント。
/// 入力検知はUpdate、物理演算はFixedUpdateに分離し、フレームレート非依存の挙動を保証。
/// </summary>
public class TestScript : MonoBehaviour
{
    [Header("移動・ジャンプの設定")]
    [SerializeField] private float speed = 5.0f; // 移動の空気抵抗や力加減
    [SerializeField] private float jumpForce = 5.0f; // ジャンプ時の瞬間的な上方向への力

    [Header("UIの参照")]
    [SerializeField] private TextMeshProUGUI scoreText; // コイン枚数を表示するUIテキスト

    // 内部キャッシュ用の変数
    private Rigidbody rb;
    private GameManager gameManager;
    private int coinCount = 0;
    private bool isGrounded = true;

    // 入力値をUpdateとFixedUpdate間で共有するための変数
    private float moveH;
    private float moveV;
    private bool isJumpRequested = false;


    void Start()
    {
        // 処理負荷軽減のため、コンポーネントとマネージャーを初期化時にキャッシュ
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError($"{gameObject.name} に Rigidbody がついていません！追加してください。");
        }
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if (rb == null) return;

        // フレーム単位の取りこぼしを防ぐため、Update内で取得
        moveH = Input.GetAxis("Horizontal");
        moveV = Input.GetAxis("Vertical");

        // ジャンプキーが押されたフラグを立てる
        if (Input.GetButtonDown("Jump") && (isGrounded == true))
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
            isGrounded = false;
            isJumpRequested = false; // トリガーを消費
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // 地面に接触した際、ジャンプ可能状態にリセット
        isGrounded = true;
    }

    /// <summary>
    /// コイン獲得時に外部（CoinItem）から呼び出されるイベント関数。
    /// スコアの更新とクリア条件のチェックを行う。
    /// </summary>
    public void AddCoin()
    {
        coinCount++;
        scoreText.text = "COINS: " + coinCount.ToString();

        if (gameManager != null)
        {
            gameManager.CheckClear(coinCount);
        }
    }   
}
