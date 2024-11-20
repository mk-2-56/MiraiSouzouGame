using UnityEngine;

public class Mark : MonoBehaviour
{
    public enum MarkState
    {
        Waiting,    // マークが待機中
        Active,     // マークが表示中（アニメーション中）
        Expanding,  // アニメーション終了後
        Ending      // マークが終了状態（非表示になる準備）
    }

    [Header("References")]
    [SerializeField] private GameObject mark;   // 三角マークの GameObject（プレハブの子）

    [Header("Settings")]
    [SerializeField] private float waitAppear = 0.5f;   // 表示までの待機時間
    [SerializeField] private float expandDuration = 1.5f; // 拡大アニメーションの時間
    [SerializeField] private Vector3 initialScale = new Vector3(2f, 2f, 2f); // 初期スケール
    [SerializeField] private Vector3 targetScale = new Vector3(1f, 1f, 1f);   // 最終スケール
    [SerializeField] private Vector3 offset = Vector3.up * 2f; // ターゲットからのオフセット位置

    private MarkState markState = MarkState.Waiting;    // 現在の状態
    private Transform targetObject;                    // 追従するターゲット
    private Camera mainCamera;                         // プレイヤーカメラ
    private float stateTimer = 0f;                     // ステートの経過時間

    /// <summary>
    /// マークを初期化
    /// </summary>
    /// <param name="target">追従するターゲット</param>
    /// <param name="camera">追従するカメラ</param>
    public void Initialize(Transform target, Camera camera)
    {
        targetObject = target;
        mainCamera = camera;
        markState = MarkState.Waiting;
        stateTimer = 0f;
        transform.localScale = initialScale;
        gameObject.SetActive(true);
    }

    /// <summary>
    /// マークの状態を更新
    /// </summary>
    public void UpdateMark()
    {
        if (targetObject == null || mainCamera == null)
        {
            ResetMark();
            return;
        }

        // ターゲットに追従しカメラ方向を向く
        FollowTarget();

        // 現在のステートに応じて処理を切り替え
        stateTimer += Time.deltaTime;
        switch (markState)
        {
            case MarkState.Waiting:
                HandleWaitingState();
                break;

            case MarkState.Active:
                HandleActiveState();
                break;

            case MarkState.Expanding:
                HandleExpandingState();
                break;

            case MarkState.Ending:
                HandleEndingState();
                break;
        }
    }

    /// <summary>
    /// ターゲットを追従しカメラ方向を向く
    /// </summary>
    private void FollowTarget()
    {
        transform.position = targetObject.position + offset;
        transform.LookAt(mainCamera.transform);
    }

    /// <summary>
    /// 待機中のステート処理
    /// </summary>
    private void HandleWaitingState()
    {
        if (stateTimer >= waitAppear)
        {
            stateTimer = 0f;
            markState = MarkState.Active;
        }
    }

    /// <summary>
    /// アクティブ状態の処理（拡大アニメーション）
    /// </summary>
    private void HandleActiveState()
    {
        transform.localScale = Vector3.Lerp(initialScale, targetScale, stateTimer / expandDuration);
        if (stateTimer >= expandDuration)
        {
            stateTimer = 0f;
            markState = MarkState.Expanding;
        }
    }

    /// <summary>
    /// 拡大後のステート処理
    /// </summary>
    private void HandleExpandingState()
    {
        // 将来的に必要な追加処理をここに記述
        // 例: 他のエフェクトや状態管理
    }

    /// <summary>
    /// 終了時のステート処理
    /// </summary>
    private void HandleEndingState()
    {
        ResetMark();
    }

    /// <summary>
    /// マークをリセット（非アクティブ化）
    /// </summary>
    public void ResetMark()
    {
        targetObject = null;
        markState = MarkState.Waiting;
        stateTimer = 0f;
        gameObject.SetActive(false);
    }

    /// <summary>
    /// マークが使用可能かを判定
    /// </summary>
    /// <returns>マークが使用可能かどうか</returns>
    public bool IsUsable()
    {
        return markState == MarkState.Active;
    }
}