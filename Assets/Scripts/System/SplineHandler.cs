using UnityEngine;

public class PlayerSplineHandler : MonoBehaviour
{
    private CC.Basic playerController;    // プレイヤーの通常操作スクリプト
    private SplineFollower splineFollower; // スプライン移動スクリプト

    private void Start()
    {
        playerController = GetComponentInParent<CC.Basic>(); // プレイヤー操作スクリプトの取得
    }

    private void OnTriggerEnter(Collider other)
    {
        // スプラインオブジェクトに触れた場合
        if (other.CompareTag("EventCheckPoint"))
        {
            Debug.Log("aa");
            // スプラインオブジェクトから SplineFollower コンポーネントを取得
            splineFollower = other.GetComponent<SplineFollower>();
            if (splineFollower != null)
            {
                // プレイヤーの操作を無効にし、スプライン移動を開始
/*                playerController.SetMovement(false);
*/                playerController.enabled = false;
                splineFollower.StartSplineMovement(this.transform);
            }
        }
    }

    // スプライン移動終了後に呼ばれるメソッド
    public void OnSplineMovementEnd()
    {
        playerController.enabled = true;
/*        playerController.SetMovement(true); // プレイヤーの操作を再び有効化
*/        splineFollower = null;               // スプラインフォロワーをリセット
    }
}