using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorKinematicFalser : MonoBehaviour
{
    private Vector3[] originalPositions; // 元の位置を保存する配列
    private Quaternion[] originalRotations; // 元の回転を保存する配列
    private Vector3[] originalVelocities; // 元の速度を保存する配列
    private Vector3[] originalAngularVelocities; // 元の角速度を保存する配列

    void Start()
    {
        // 子オブジェクトの元の位置を記録
        Rigidbody[] childRbs = GetComponentsInChildren<Rigidbody>();
        originalPositions = new Vector3[childRbs.Length];
        originalRotations = new Quaternion[childRbs.Length];
        originalVelocities = new Vector3[childRbs.Length];
        originalAngularVelocities = new Vector3[childRbs.Length];

        for (int i = 0; i < childRbs.Length; i++)
        {
            originalPositions[i] = childRbs[i].transform.position;
            originalRotations[i] = childRbs[i].transform.rotation; // 回転を記録
            originalVelocities[i] = childRbs[i].velocity; // 速度を記録
            originalAngularVelocities[i] = childRbs[i].angularVelocity; // 角速度を記録
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // プレイヤーと衝突したかを確認
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Collision is ok");
            // 子オブジェクトのすべてのRigidbodyを取得
            Rigidbody[] childRbs = GetComponentsInChildren<Rigidbody>();

            foreach (Rigidbody childRb in childRbs)
            {
                Debug.Log("foreach is ok");
                // isKinematicをfalseにする
                if (childRb != null)
                {
                    childRb.isKinematic = false;

                    // 飛び散らせる
                    var random = new System.Random();
                    var min = -3;
                    var max = 3;
                    var vect = new Vector3(random.Next(min, max), random.Next(0, max), random.Next(min, max));

                    childRb.AddForce(vect, ForceMode.Impulse);
                    childRb.AddTorque(vect, ForceMode.Impulse);
                }
            }
        }
    }

    void Update()
    {
        // 非表示の場合、isKinematicをtrueに戻し、元の位置に戻す
        if (!gameObject.activeInHierarchy)
        {
            ResetKinematic();
        }
    }

    public void ResetKinematic()
    {
        Debug.Log("reset is ok");
        Rigidbody[] childRbs = GetComponentsInChildren<Rigidbody>();
        for (int i = 0; i < childRbs.Length; i++)
        {
            if (childRbs[i] != null)
            {
                // 元の位置に戻す
                childRbs[i].transform.position = originalPositions[i];
                // 回転を元の回転に戻す
                childRbs[i].transform.rotation = originalRotations[i];
                // 加えた力をリセットする
                childRbs[i].velocity = originalVelocities[i]; // 元の速度に戻す
                childRbs[i].angularVelocity = originalAngularVelocities[i]; // 元の角速度に戻す

                // isKinematicをtrueに戻す
                childRbs[i].isKinematic = true;
            }
        }
    }
}
