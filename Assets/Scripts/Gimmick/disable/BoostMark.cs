using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostMark : MonoBehaviour
{
    public enum MarkState
    {
        Waiting,
        Act1,
        Act2,
        Ending
    }
    public class MarkProperties
    {
        public MarkState markState;                                  // UIのステート情報
        public int Cnt;                                              // カウンター
        public bool Usable;                                          // プレイヤー利用可能フラグ

        // コンストラクタ
        public MarkProperties()
        {
            markState = MarkState.Waiting;
            Cnt = 0;
            Usable = false;
        }
    }
    private int waitAppear = 10;                                     // 表示
    private int waitExpand = 120;                                    // 拡大

    /******************************************************************************************************************************************************************/
    // グローバル変数
    /******************************************************************************************************************************************************************/

    // パブリック変数
    public static int MaxMarkUI = 10;                                               // UIの最大数(maxMeteorの2倍は使用中)
    public MarkProperties[] MarkProp = new MarkProperties[MaxMarkUI];               // UIの設定
    public ObjectPool BoostMarkPool;

    // プライベート変数 
    [SerializeField] private MeteorMovement meteorMovement;                         // 噴石の情報を取得

    private MeteorMovement.MeteorProperties[] meteorProperties;                     // 噴石設定の情報を取得
    private Transform MainCam;                                                      // カメラの情報
    private Transform WorldSpaceCanvas;                                             // WorldSpaceCanvasの情報



    // Start is called before the first frame update
    void Start()
    {

        MainCam = Camera.main.transform;                                                // カメラの情報を取得

        WorldSpaceCanvas = GameObject.FindGameObjectWithTag("WorldSpace").transform;    // UIの親情報を取得

        for (int i = 0; i < MaxMarkUI; i++)
        {
            BoostMarkPool.GetList()[i].transform.SetParent(WorldSpaceCanvas); // 親子関係を設定
            BoostMarkPool.GetList()[i].transform.localScale = new Vector3(5, 5, 5); // サイズを設定(初期2,2,2)
            MarkProp[i] = new MarkProperties();
        }

    }

    // Update is called once per frame
    void Update()
    {
        // 取得できない場合、実行しない（エラーログ対処のため）
        if (meteorMovement == null)
        {
            return;
        }

        // 噴石の設定情報を取得
        int maxMeteor = MeteorMovement.MaxMeteor;
        meteorProperties = new MeteorMovement.MeteorProperties[maxMeteor];

        for (int i = 0; i < maxMeteor; i++)
        {
            meteorProperties[i] = meteorMovement.GetMeteorProperties(i);
        }
        // 噴石オブジェクトの情報を取得
        List<GameObject> instantiatedMeteors = meteorMovement.GetInstantiatedMeteors();



        if (instantiatedMeteors.Count >= maxMeteor)
        {
            for (int i = 0; i < maxMeteor; i++)
            {
                // 一定距離近づいたら、表示させる
                float distance = Vector3.Distance(MainCam.transform.position, instantiatedMeteors[i].transform.position);
                if (meteorProperties[i].Use && distance <= 40.0f)
                {
                    float val = 1.0f / waitExpand * 4;
                    switch (MarkProp[i].markState)
                    {
                        case MarkState.Waiting:
                            MarkProp[i].Cnt++;
                            if (MarkProp[i].Cnt > waitAppear)
                            {
                                MarkProp[i].markState = MarkState.Act1;
                                MarkProp[i + maxMeteor].markState = MarkState.Act1;
                                // 設定の初期化
                                MarkProp[i].Cnt = 0;
                            }
                            break;
                        case MarkState.Act1:
                            MarkProp[i].Usable = true;
                            BoostMarkPool.GetList()[i].SetActive(true); // 表示する
                            BoostMarkPool.GetList()[i].transform.localScale -= new Vector3(val, val, val);
                            //BoostMarkPool.GetList()[i].SetActive(true); // 表示する
                            //BoostMarkPool.GetList()[i].transform.localScale -= new Vector3(val, val, val);

                            MarkProp[i].Cnt++;
                            if (MarkProp[i].Cnt > waitExpand)
                            {
                                MarkProp[i].markState = MarkState.Act2;
                                // 設定の初期化
                                MarkProp[i].Cnt = 0;
                                BoostMarkPool.GetList()[i].transform.localScale = new Vector3(5, 5, 5);

                                //BoostMarkPool.GetList()[i].transform.localScale = new Vector3(5, 5, 5);
                            }

                            // アニメーションなしのマーク
                            BoostMarkPool.GetList()[i + maxMeteor].SetActive(true); // 表示する
                            BoostMarkPool.GetList()[i + maxMeteor].transform.localScale = new Vector3(1, 1, 1);
                            break;
                        case MarkState.Act2:
                            BoostMarkPool.GetList()[i].transform.localScale -= new Vector3(val, val, val);

                            MarkProp[i].Cnt++;
                            if (MarkProp[i].Cnt > waitExpand)
                            {
                                MarkProp[i].markState = MarkState.Ending;
                                BoostMarkPool.GetList()[i].SetActive(false); // 非表示にする
                                // 設定の初期化
                                MarkProp[i].Cnt = 0;
                            }
                            break;
                        case MarkState.Ending:

                            break;
                    }


                    // オブジェクトの前方にUIを表示させるため、positionを調整
                    Vector3 targetDirection;
                    {
                        targetDirection = BoostMarkPool.GetList()[i].transform.position - MainCam.transform.position;
                        //targetDirection.y = 0; // Y方向は無視する

                        if (targetDirection.magnitude > 0.01f) // 小さな値より大きい場合
                        {
                            targetDirection.Normalize(); // 正規化して方向ベクトルを得る
                            targetDirection += targetDirection * 2.0f;  // どれくらい前方に表示させるか、ここで調整(2.0f)
                        }
                    }

                    BoostMarkPool.GetList()[i].transform.position = instantiatedMeteors[i].transform.position - targetDirection;
                    BoostMarkPool.GetList()[i].transform.rotation = Quaternion.LookRotation(BoostMarkPool.GetList()[i].transform.position - MainCam.transform.position);
                    // アニメーションなしのマーク
                    BoostMarkPool.GetList()[i + maxMeteor].transform.position = instantiatedMeteors[i].transform.position - targetDirection;
                    BoostMarkPool.GetList()[i + maxMeteor].transform.rotation = Quaternion.LookRotation(BoostMarkPool.GetList()[i].transform.position - MainCam.transform.position);
                }
                else
                {
                    BoostMarkPool.GetList()[i].SetActive(false); // 非表示にする
                    BoostMarkPool.GetList()[i].transform.localScale = new Vector3(5, 5, 5);
                    MarkProp[i].markState = MarkState.Waiting;
                    MarkProp[i].Usable = false;

                    // アニメーションなしのマーク
                    BoostMarkPool.GetList()[i + maxMeteor].SetActive(false); // 表示する
                    BoostMarkPool.GetList()[i + maxMeteor].transform.localScale = new Vector3(5, 5, 5);
                    MarkProp[i + maxMeteor].markState = MarkState.Waiting;
                }
            }
        }
        else
        {
            Debug.LogWarning("Not enough instantiated Meteors.");
        }

    }


    // ブーストマーク利用可能フラグゲッター（プレイヤーブースト用）
    public (bool, Vector3) ObjectBoostUsable()
    {
        bool usable = false;
        Vector3 pos = new Vector3(0, 0, 0);


        Vector3 cameraForward = Camera.main.transform.forward; // カメラの前方ベクトルを取得
        float closestAngle = float.MaxValue; // 最も小さい角度を見つけるための初期値


        for (int i = 0; i < MaxMarkUI; i++)
        {
            if (MarkProp[i].Usable)
            {
                // オブジェクトへのベクトルを取得
                Vector3 directionToObj = (BoostMarkPool.GetList()[i].transform.position - Camera.main.transform.position).normalized;

                // 角度を計算
                float angle = Vector3.Angle(cameraForward, directionToObj);

                // 最小の角度を持つオブジェクトを見つける
                if (angle < closestAngle)
                {
                    closestAngle = angle;
                    pos = BoostMarkPool.GetList()[i].transform.position;
                }

                usable = true;
            }
        }

        return (usable, pos);
    }
}