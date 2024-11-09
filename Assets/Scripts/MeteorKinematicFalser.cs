using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorKinematicFalser : MonoBehaviour
{
    private Vector3[] originalPositions; // 元の位置を保存する配列
    private Quaternion[] originalRotations; // 元の回転を保存する配列
    private Vector3[] originalVelocities; // 元の速度を保存する配列
    private Vector3[] originalAngularVelocities; // 元の角速度を保存する配列
    
    private MeteorMovement meteorMovement; // MeteorMovementの参照を追加

    private bool started = false; // 開始時の初期化を終えてからResetKinematic()を実行させるため
    private int despawnCnt = 0; // 破壊後に消滅するまでのカウンター
    private int meteoNum = 0; // 消滅させる噴石の番号
    private bool cntDespawn = false;
    [SerializeField] private int despawnTime = 1500; // 衝突後、消滅するまでの時間


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

        // 名前でオブジェクトを見つける
        GameObject meteorSpawner = GameObject.Find("MeteorsSpawner");
        // MeteorMovementコンポーネントを取得
        meteorMovement = meteorSpawner.GetComponent<MeteorMovement>();

        started = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        // プレイヤーと衝突したかを確認
        if (collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Collision is ok");
            // 子オブジェクトのすべてのRigidbodyを取得
            Rigidbody[] childRbs = GetComponentsInChildren<Rigidbody>();

            foreach (Rigidbody childRb in childRbs)
            {
                // isKinematicをfalseにする
                if (childRb != null)
                {
                    childRb.isKinematic = false;

                    //// 力を加えてもっと飛び散らせる
                    //var random = new System.Random();
                    //var min = -8;
                    //var max = 8;
                    //var vect = new Vector3(random.Next(min, max), random.Next(0, max), random.Next(min, max));

                    //childRb.AddForce(vect, ForceMode.Impulse);
                    //childRb.AddTorque(vect, ForceMode.Impulse);
                }
            }

            // プレイヤーと衝突後、時間経過で消滅させる
            if (meteorMovement != null)
            {
                for (int i = 0; i < meteorMovement.MeteorProp.Length; i++)
                {
                    float distanceThreshold = 5.0f;// 位置が近いかどうかを判定する

                    if (Vector3.Distance(meteorMovement.MeteorProp[i].Pos, transform.position) < distanceThreshold)
                    {
                        meteoNum = i;
                        cntDespawn = true;
                        break;
                    }
                }
            }

            //// 当たり判定を消す
            //Collider[] childCol = GetComponentsInChildren<Collider>();
            //foreach (Collider childCo in childCol)
            //{
            //    if (childCo != null)
            //    {
            //        childCo.isTrigger = true;
            //    }
            //}
        }
    }

    void Update()
    {
        //デバッグ用
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ResetKinematic();
        }
        if (cntDespawn)
        {
            AfterBreakDespawnTimer();
        }
    }

    void OnEnable()
    {
        if (started)
        {
            ResetKinematic();   // 表示されたら、オブジェクト破壊で飛び散った破片をリセットする

            //// 消した当たり判定を復活させる
            //Collider[] childCol = GetComponentsInChildren<Collider>();
            //foreach (Collider childCo in childCol)
            //{
            //    if (childCo != null)
            //    {
            //        childCo.isTrigger = false;
            //    }
            //}
        }
    }

    public void ResetKinematic()
    {
        //Debug.Log("reset is ok");

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
                if (!childRbs[i].isKinematic)
                {
                    childRbs[i].velocity = originalVelocities[i]; // 元の速度に戻す
                    childRbs[i].angularVelocity = originalAngularVelocities[i]; // 元の角速度に戻す
                }

                // isKinematicをtrueに戻す
                childRbs[i].isKinematic = true;
            }
        }
    }

    private void AfterBreakDespawnTimer()
    {
        despawnCnt++;
        if (despawnCnt > despawnTime)
        {
            despawnCnt = 0;
            meteorMovement.MeteorProp[meteoNum].Use = false;
            cntDespawn = false;
        }
    }
}
