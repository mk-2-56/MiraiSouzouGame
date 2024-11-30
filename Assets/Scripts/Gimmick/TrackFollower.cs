using UnityEngine;
using Cinemachine;
using System.Collections.Generic;
using AU;

public class TrackFollower : MonoBehaviour
{
    [SerializeField] private CinemachineSmoothPath trackPath; // トラック
    [SerializeField] private float trackSpeed = 5.0f;         // 移動速度
    private Dictionary<GameObject, CinemachineDollyCart> activeCarts = new Dictionary<GameObject, CinemachineDollyCart>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !activeCarts.ContainsKey(other.gameObject))
        {
            GameObject player = other.gameObject;

            // プレイヤーにDollyCartを追加
            CinemachineDollyCart dollyCart = player.AddComponent<CinemachineDollyCart>();
            dollyCart.m_Path = trackPath;        // トラックを設定
            dollyCart.m_Position = 0;            // トラックの開始位置
            dollyCart.m_Speed = trackSpeed;      // トラック上の移動速度
            dollyCart.m_UpdateMethod = CinemachineDollyCart.UpdateMethod.FixedUpdate;

            activeCarts[player] = dollyCart;

/*            UnityEngine.Debug.Log($"{player.name} started moving along the track!");
*/        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && activeCarts.ContainsKey(other.gameObject))
        {
            GameObject player = other.gameObject;

            // DollyCartを停止し、プレイヤーから削除
            CinemachineDollyCart dollyCart = activeCarts[player];
            dollyCart.m_Speed = 0;
            Destroy(dollyCart);

            activeCarts.Remove(player);

/*            Debug.Log($"{player.name} stopped moving along the track!");
*/        }
    }
}