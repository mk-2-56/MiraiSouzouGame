using UnityEngine;
using Cinemachine;
using System.Collections.Generic;
using UnityEngine.VFX;

public class TrackFollower : MonoBehaviour
{
    [SerializeField] private CinemachineSmoothPath trackPath; // トラック
    [SerializeField] private float trackSpeed;         // 移動速度
    private Dictionary<GameObject, CinemachineDollyCart> activeCarts = new Dictionary<GameObject, CinemachineDollyCart>();
    private Dictionary<GameObject, Quaternion> initialRotations = new Dictionary<GameObject, Quaternion>();
    private Dictionary<GameObject, Vector3> endDirs = new Dictionary<GameObject, Vector3>();
    private AudioSource curAucioSource = null;
    private void Update()
    {
        // 全てのDollyCartをチェック
        List<GameObject> cartsToRemove = new List<GameObject>();

        foreach (var entry in activeCarts)
        {
            GameObject player = entry.Key;
            CinemachineDollyCart dollyCart = entry.Value;

            // プレイヤーの向きを固定
            if (initialRotations.ContainsKey(player))
            {
                player.transform.rotation = initialRotations[player];
            }

            // 終点に到達したか判定
            if (dollyCart.m_Position >= dollyCart.m_Path.PathLength)
            {
                curAucioSource?.Stop();
                // 終了の方向を取得
                Vector3 endDir = dollyCart.m_Path.EvaluateTangentAtUnit(
                    dollyCart.m_Path.PathLength, // 最終位置
                    CinemachinePathBase.PositionUnits.Distance
                );

                endDir = endDir.normalized; // 正規化
                if (!endDirs.ContainsKey(player))
                {
                    endDirs[player] = endDir; // 終了時の進行方向を保存
                }

                dollyCart.m_Speed = 0;
                Destroy(dollyCart);
                cartsToRemove.Add(player);

                // Kinematicをオフに戻す（必要なら）
                Rigidbody rb = player.GetComponent<Rigidbody>();
                if (rb)
                {
                    rb.isKinematic = false; 
                }
            }

            // スプライン移動終了後も進行方向に沿って移動を継続
            if (endDirs.ContainsKey(player))
            {
                AddEndTrackVec(player, endDirs[player]);
            }
        }

        // 終点到達したの処理
        foreach (var player in cartsToRemove)
        {
            activeCarts.Remove(player);
            initialRotations.Remove(player);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !activeCarts.ContainsKey(other.gameObject))
        {
            SoundManager.Instance?.PlaySE(SESoundData.SE.SE_WindTrigger);

            GameObject player = other.gameObject;
            //Animation Set
            player.transform.Find("Facing/Cog/AnimationController").GetComponent<PlayerAnimationControl>().DispatchBigJump();
            
            player.transform.Find("Facing/Cog/EffectDispatcher/BigJumpWindEffect").GetComponent<VisualEffect>().Play() ;
            Rigidbody rb = player.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true; // 物理挙動を無効化
            }

            // プレイヤーの初期向きを記録
            if (!initialRotations.ContainsKey(player))
            {
                initialRotations[player] = player.transform.rotation;
            }

            // プレイヤーにDollyCartを追加
            CinemachineDollyCart dollyCart = player.AddComponent<CinemachineDollyCart>();
            dollyCart.m_Path = trackPath;        // トラックを設定
            dollyCart.m_Position = 0;           // トラックの開始位置
            dollyCart.m_Speed = trackSpeed;     // トラック上の移動速度
            dollyCart.m_UpdateMethod = CinemachineDollyCart.UpdateMethod.FixedUpdate;

            activeCarts[player] = dollyCart;

            curAucioSource = SoundManager.Instance?.PlaySE(SESoundData.SE.SE_WindOnBigJump);
            Debug.Log($"{player.name} started moving along the track!");
        }
    }

    // 終了の移動処理
    private void AddEndTrackVec(GameObject player, Vector3 direction)
    {
        float moveSpeed = trackSpeed; // 継続的な移動速度
        // プレイヤーを進行方向に移動
        player.GetComponent<Rigidbody>().velocity = direction * moveSpeed;
    }
}