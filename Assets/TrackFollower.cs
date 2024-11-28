using AU;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackFollower : MonoBehaviour
{
    // Start is called before the first frame update
    private uint followerIndex = 0;
    private bool isFollower = false;
    private 
    void Start()
    {
        followerIndex = 0;
        isFollower = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void StartSplineMovement()
    {
        //if (_dollyCarts.TryGetValue(playerId, out CinemachineDollyCart dollyCart))
        //{
        //    // プレイヤーの操作を無効化
        //    var playerController = _players[playerId].GetComponent<PlayerController>();
        //    if (playerController != null)
        //    {
        //        playerController.enabled = false;
        //    }

        //    // Dolly Cartを動作開始
        //    dollyCart.m_Speed = dollySpeed;

        //    // 移動中のチェックを開始
        //    StartCoroutine(CheckSplineEnd(playerId));
        //    Debug.Log($"プレイヤー{playerId}がスプライン移動を開始しました！");
        //}
    }
}
