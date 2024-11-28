using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PubbleTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    public ObjectFactory ObjectFactory;
    [SerializeField] int numberOfPebbles = 10;
    [SerializeField] float explosionForce = 10;
    // Update is called once per frame
    void Update()
    {//これテスト用
        //if (Input.GetKeyDown(KeyCode.Return))
        //{
        //    //隕石がプレイヤーとぶつかった後小石生成関数（パーティクルも追加予定
        //    TriggerExplosionEffect();
        //}
    }

    void TriggerExplosionEffect()
    {
        for (int i = 0; i < numberOfPebbles; i++)
        {
            Debug.Log(transform.position.ToString());
            // 小石を生成
            GameObject pebble = ObjectFactory.generateObj(
                ObjectType.objType_Pebble,
                transform.position, // 隕石の位置を基準に生成
                Random.rotation     // ランダムな回転
            );

            Rigidbody rb = pebble.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = pebble.AddComponent<Rigidbody>();
            }

            // ランダムな方向ベクトルを生成
            Vector3 randomDirection = Random.onUnitSphere; // 半径1の球面上のランダムな位置を取得
            rb.AddForce(randomDirection * explosionForce, ForceMode.Impulse); // ランダム方向に力を加える
        }
    }
}
