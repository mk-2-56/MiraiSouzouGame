using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorsActivatePoint : MonoBehaviour
{
    private MeteorMovement meteor;
    private bool canActivate = true; // アクティベート可能かどうかのフラグ
    private float waitTime = 10f; // 待機時間
    private float timer = 0f; // タイマー用の変数


    void Start()
    {
        // 名前でオブジェクトを見つける
        GameObject meteorSpawner = GameObject.Find("MeteorsSpawner");

        // MeteorMovementコンポーネントを取得
        meteor = meteorSpawner.GetComponent<MeteorMovement>();

        if (meteorSpawner == null)
        {
            Debug.LogError("Error: MeteorsSpawner not found.");
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canActivate)
        {
            // PlayerControllerが見つかった場合、関数を呼び出す
            if (meteor != null)
            {
                meteor.ActivateMeteors();
                canActivate = false; // アクティベートを無効にする
            }
            else
            {
                Debug.Log("error");
            }

        }
    }

    private void Update()
    {
        // 待機中の場合、タイマーを進める
        if (!canActivate)
        {
            timer += Time.deltaTime; // タイマーを加算
            if (timer >= waitTime)
            {
                canActivate = true; // アクティベート可能に戻す
                timer = 0f; // タイマーをリセット
            }
        }
    }
}
