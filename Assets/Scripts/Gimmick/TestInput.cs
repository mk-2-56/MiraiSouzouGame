using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TestInput : MonoBehaviour
{
    // Start is called before the first frame update
    public ObjectFactory ObjectFactory;
    [SerializeField] int numberOfPebbles = 10;
    [SerializeField] float explosionForce = 10;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {

            TriggerExplosionEffect();
/*            ObjectFactory.generateObj(ObjectType.objType_Pebble);
*/        }
    }

    void TriggerExplosionEffect()
    {
        for (int i = 0; i < numberOfPebbles; i++)
        {
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
