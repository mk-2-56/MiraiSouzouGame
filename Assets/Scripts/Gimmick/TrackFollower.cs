using UnityEngine;
using Cinemachine;
using System.Collections.Generic;
using AU;

public class TrackFollower : MonoBehaviour
{
    [SerializeField] private CinemachineSmoothPath trackPath; // トラック
    [SerializeField] private float trackSpeed = 5.0f;         // 移動速度
    private Dictionary<GameObject, CinemachineDollyCart> activeCarts = new Dictionary<GameObject, CinemachineDollyCart>();
    private Dictionary<GameObject, Quaternion> initialRotations = new Dictionary<GameObject, Quaternion>();


    private void Update()
    {
        // 全てのDollyCartをチェック
        List<GameObject> cartsToRemove = new List<GameObject>();

        foreach (var entry in activeCarts)
        {
            GameObject player = entry.Key;
            CinemachineDollyCart dollyCart = entry.Value;

            // 終点に到達したか判定
            if (dollyCart.m_Position >= dollyCart.m_Path.PathLength)
            {
                dollyCart.m_Speed = 0; // DollyCartを停止
                Destroy(dollyCart);    // DollyCartコンポーネントを削除
                cartsToRemove.Add(player); // 削除予定のプレイヤーを記録
                UnityEngine.Debug.Log($"{player.name} reached the end of the track.");
            }
            else
            {
                // プレイヤーの回転を維持
                if (initialRotations.ContainsKey(player))
                {
                    player.transform.rotation = initialRotations[player];
                }
            }
        }

        // 終点到達したプレイヤーを辞書から削除
        foreach (var player in cartsToRemove)
        {
            activeCarts.Remove(player);
        }

        /* if (dollyCart.m_Position >= dollyCart.m_Path.PathLength)
         {
             GameObject player = other.gameObject;

             // DollyCartを停止し、プレイヤーから削除
             CinemachineDollyCart dollyCart = activeCarts[player];
             dollyCart.m_Speed = 0;
             Destroy(dollyCart);

             activeCarts.Remove(player);
         }*/

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !activeCarts.ContainsKey(other.gameObject))
        {
            GameObject player = other.gameObject;

            // プレイヤーの初期回転を保存
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

            UnityEngine.Debug.Log($"{player.name} started moving along the track!");
        }
    }

}