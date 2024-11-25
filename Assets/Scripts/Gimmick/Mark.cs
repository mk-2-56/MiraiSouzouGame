using UnityEngine;

public class Mark : MonoBehaviour
{
    public enum MarkState
    {
        Waiting,    // マークが待機中
        Active,     // マークが表示中（追従中）
        Ending      // マークが終了状態（非表示になる準備）
    }

    [Header("Settings")]
    [SerializeField] private Vector3 offset = Vector3.up * 2f; // ターゲットからのオフセット位置
    [SerializeField] private float waitAppear = 0.5f;         // 表示までの待機時間

    private MarkState markState = MarkState.Waiting;          // 現在の状態
    private Transform targetObject;                          // 追従するターゲット
    private Camera mainCamera;                               // メインカメラ
    private float stateTimer = 0f;                           // ステート用のタイマー
    private RectTransform rectTransform;                     // マークの RectTransform
    private RectTransform canvasRectTransform;               // 親 Canvas の RectTransform

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize(Transform target, Camera camera)
    {
        targetObject = target;
        mainCamera = camera;
        stateTimer = 0f;
        markState = MarkState.Waiting;

        // RectTransform を取得
        rectTransform = GetComponent<RectTransform>();
        canvasRectTransform = rectTransform.parent.GetComponent<RectTransform>();

        gameObject.SetActive(true); // マークをアクティブ化
    }

    public void UpdateMark()
    {
        if (targetObject == null || mainCamera == null)
        {
            ResetMark();
            return;
        }

        // ターゲットに追従し、カメラ方向に向ける
        FollowTarget();

        // ステートごとの処理
        stateTimer += Time.deltaTime;
        switch (markState)
        {
            case MarkState.Waiting:
                HandleWaitingState();
                break;

            case MarkState.Active:
                HandleActiveState();
                break;

            case MarkState.Ending:
                ResetMark();
                break;
        }
    }

    /// <summary>
    /// ターゲットを追従
    /// </summary>
    private void FollowTarget()
    {
        // ターゲットの上のワールド座標を取得
        Vector3 worldPosition = targetObject.position + offset;

        // ワールド座標をスクリーン座標に変換
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(worldPosition);

        // スクリーン座標を Canvas 内のローカル座標に変換
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRectTransform, screenPosition, mainCamera, out Vector2 localPosition))
        {
            rectTransform.anchoredPosition = localPosition; // マークの位置を更新
        }
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
    /// アクティブな状態の処理（特別な演出などをここで追加可能）
    /// </summary>
    private void HandleActiveState()
    {
        // 必要に応じて、マークのアクティブ状態での処理を記述
    }

    /// <summary>
    /// マークをリセット（非アクティブ化）
    /// </summary>
    public void ResetMark()
    {
        targetObject = null;
        markState = MarkState.Waiting;
        stateTimer = 0f;
        gameObject.SetActive(false); // プールに戻す際に非アクティブ化
    }

    /// <summary>
    /// マークが使用可能かどうかを判定
    /// </summary>
    /// <returns>マークが使用可能かどうか</returns>
    public bool IsUsable()
    {
        return markState == MarkState.Active;
    }
}