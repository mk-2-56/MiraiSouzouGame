using UnityEngine;
using UnityEngine.Splines;

public class SplineFollower : MonoBehaviour
{
    public SplineContainer splineContainer;
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float progress = 0.0f;

    // Update is called once per frame
    void Update()
    {
        // スプラインに沿った進行度を更新
        progress += speed * Time.deltaTime / splineContainer.CalculateLength();
        progress = Mathf.Clamp01(progress);  // 進行度を0?1に制限

        // スプライン上の現在の位置を取得してプレイヤーを移動
        Vector3 position = splineContainer.EvaluatePosition(progress);
        transform.position = position;

        // オブジェクトの向きもスプラインに沿わせる場合
        //Quaternion rotation = splineContainer.EvaluateOr(progress);
        //transform.rotation = rotation;
    }
}
