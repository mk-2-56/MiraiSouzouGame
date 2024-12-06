using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleporter : MonoBehaviour
{
    // 移動先の位置をリストで管理
    [SerializeField]private List<Vector3> targetPositions = new List<Vector3>();
    private int currentTargetIndex = 0; // 現在の移動先インデックス

    void Update()
    {
        // Tボタンが押された場合
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (targetPositions.Count == 0)
            {
                Debug.LogWarning("Target positions list is empty.");
                return;
            }

            // Playerタグのついたオブジェクトを探す
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                // 現在のターゲット位置に移動
                player.transform.position = targetPositions[currentTargetIndex];
                Debug.Log($"Player moved to {targetPositions[currentTargetIndex]}");

                // 次のターゲット位置に進む（ループする）
                currentTargetIndex = (currentTargetIndex + 1) % targetPositions.Count;
            }
            else
            {
                Debug.LogWarning("No object with Player tag found.");
            }
        }
    }
}
