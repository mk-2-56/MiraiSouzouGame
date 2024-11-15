using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class SplineFollower : MonoBehaviour
{
    public SplineContainer splineContainer;
    public float speed = 5.0f;

    private List<Transform> targets = new List<Transform>();      // 複数のプレイヤーのターゲット
    private List<float> progresses = new List<float>();           // 各プレイヤーの進行度
    private List<PlayerSplineHandler> playerHandlers = new List<PlayerSplineHandler>(); // プレイヤーのスプラインハンドラ

    private bool isSpline = false;  //移動中？

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i] != null)
            {
                FollowSpline(i);
            }
        }
    }
    private void FollowSpline(int index)
    {
        // スプラインに沿ってプレイヤーを移動
        progresses[index] += speed * Time.deltaTime / splineContainer.CalculateLength();
        progresses[index] = Mathf.Clamp01(progresses[index]); // 進行度を0～1にクランプ

        // スプライン上の位置を計算し、ターゲットに適用
        Vector3 currentPosition = splineContainer.EvaluatePosition(progresses[index]);
        targets[index].position = currentPosition;

        // 向きの設定
        float lookAheadDistance = 0.01f;
        float nextProgress = Mathf.Clamp01(progresses[index] + lookAheadDistance);
        Vector3 nextPosition = splineContainer.EvaluatePosition(nextProgress);
        Vector3 direction = (nextPosition - currentPosition).normalized;

        if (direction != Vector3.zero)
        {
            targets[index].rotation = Quaternion.LookRotation(direction); // ターゲットの向きを進行方向に設定
        }

        // 終点に到達したら移動を停止
        if (progresses[index] >= 1f)
        {
            EndSplineMovement(index);
        }
    }

    public void StartSplineMovement(Transform newTarget)
    {
        targets.Add(newTarget); // 移動対象のターゲットを設定
        progresses.Add(0f);
        playerHandlers.Add(newTarget.GetComponent<PlayerSplineHandler>());
/*        isSpline = true;
*/    }

    public void EndSplineMovement(int index)
    {
        // 指定のターゲットのスプライン移動を終了し、リストから削除
        if (playerHandlers[index] != null)
        {
            playerHandlers[index].OnSplineMovementEnd(); // スプライン移動終了を通知
        }
        targets.RemoveAt(index);//この関数強すぎ
        progresses.RemoveAt(index);
        playerHandlers.RemoveAt(index);
        // 他のスクリプトで通常移動に戻す処理を行う
    }

}
