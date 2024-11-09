using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePlayer : MonoBehaviour
{
    public float moveSpeed = 5f; // 移動速度
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        // 入力を取得
        float horizontal = Input.GetAxis("Horizontal"); // A/D または 左/右キー
        float vertical = Input.GetAxis("Vertical");     // W/S または 上/下キー

        // カメラの前方と右方向を基準にする
        Vector3 forward = mainCamera.transform.forward;
        Vector3 right = mainCamera.transform.right;

        // 水平面における移動方向に制限
        forward.y = 0;
        right.y = 0;

        // 正規化して方向ベクトルを得る
        forward.Normalize();
        right.Normalize();

        // 入力に基づいて移動方向を決定
        Vector3 moveDirection = (forward * vertical + right * horizontal).normalized;

        // 移動を適用
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }
}
