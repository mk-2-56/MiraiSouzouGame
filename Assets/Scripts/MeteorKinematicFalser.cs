using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorKinematicFalser : MonoBehaviour
{
    private Vector3[] originalPositions; // 元の位置を保存する配列
    private Quaternion[] originalRotations; // 元の回転を保存する配列

    private Rigidbody[] childRbs;

    void Start()
    {
        // 子オブジェクトの元の位置を記録
        childRbs = GetComponentsInChildren<Rigidbody>();
        originalPositions = new Vector3[childRbs.Length];
        originalRotations = new Quaternion[childRbs.Length];

        for (int i = 0; i < childRbs.Length; i++)
        {
            originalPositions[i] = new Vector3(0,0,0);
            originalRotations[i] = childRbs[i].transform.rotation; // 回転を記録
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // プレイヤーと衝突したかを確認
        if (collision.gameObject.CompareTag("Player"))
        {
            // 子オブジェクトのすべてのRigidbodyを取得
            //childRbs = GetComponentsInChildren<Rigidbody>();

            foreach (Rigidbody childRb in childRbs)
            {
                // isKinematicをfalseにする
                if (childRb != null)
                {
                    childRb.isKinematic = false;

                    // 飛び散らせる
                    //var random = new System.Random();
                    //var min = -3;
                    //var max = 3;
                    //var vect = new Vector3(random.Next(min, max), random.Next(0, max), random.Next(min, max));

                    //childRb.AddForce(vect, ForceMode.Impulse);
                    //childRb.AddTorque(vect, ForceMode.Impulse);
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

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ResetKinematic();
        }
    }

    public void ResetKinematic()
    {
        Debug.Log("reset is ok");

        //childRbs = GetComponentsInChildren<Rigidbody>();


        GameObject[] obj = GameObject.FindGameObjectsWithTag("MeteorBreakable");
        // 子オブジェクトのすべてのRigidbodyを取得
        Rigidbody[][] childRbs2 = new Rigidbody[obj.Length][];

        for (int i = 0; i < obj.Length; i++)
        {
            childRbs2[i] = obj[i].GetComponentsInChildren<Rigidbody>();

            for (int j = 0; j < childRbs.Length; j++)
            {
                if (childRbs[j] != null)
                {
                    // 元の位置に戻す
                    childRbs[j].transform.position = originalPositions[j];
                    // 回転を元の回転に戻す
                    childRbs[j].transform.rotation = originalRotations[j];
                    // 加えた力をリセットする
                    if (!childRbs[j].isKinematic)
                    {
                        childRbs[j].velocity = new Vector3(0, 0, 0); // 元の速度に戻す
                        childRbs[j].angularVelocity = new Vector3(0, 0, 0); // 元の角速度に戻す
                    }
                    // isKinematicをtrueに戻す
                    childRbs[j].isKinematic = true;
                }
            }
        }
    }
}
