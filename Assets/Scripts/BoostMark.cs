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

    // *********************************************************************************************************
    // グローバル変数

    // パブリック変数
    public static int MaxMarkUI = 10;                                               // UIの最大数(maxMeteorの2倍は使用中)
    public MarkProperties[] MarkProp = new MarkProperties[MaxMarkUI];               // UIの設定

    // プライベート変数 
    [SerializeField] private MeteorMovement meteorMovement;                         // 噴石の情報を取得
    [SerializeField] private GameObject PrefabUIObj_Cir;                            // UIオブジェクトの取得
    private List<GameObject> MarkUI_Cir = new List<GameObject>();                   // UIオブジェクトリスト
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
            GameObject obj = Instantiate(PrefabUIObj_Cir); // オブジェクトを生成する
            MarkUI_Cir.Add(obj); // リストに追加
            MarkUI_Cir[i].SetActive(false); // 非表示にする
            MarkUI_Cir[i].transform.SetParent(WorldSpaceCanvas); // 親子関係を設定
            MarkUI_Cir[i].transform.localScale = new Vector3(5, 5, 5); // サイズを設定(初期2,2,2)

            MarkProp[i] = new MarkProperties();
        }
        if (PrefabUIObj_Cir == null)
        {
            Debug.LogError("PrefabUI is not assigned!");
            return;
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
                            MarkUI_Cir[i].SetActive(true); // 表示する
                            MarkUI_Cir[i].transform.localScale -= new Vector3(val, val, val);

                            MarkProp[i].Cnt++;
                            if (MarkProp[i].Cnt > waitExpand)
                            {
                                MarkProp[i].markState = MarkState.Act2;
                                // 設定の初期化
                                MarkProp[i].Cnt = 0;
                                MarkUI_Cir[i].transform.localScale = new Vector3(5, 5, 5);
                            }

                            // アニメーションなしのマーク
                            MarkUI_Cir[i + maxMeteor].SetActive(true); // 表示する
                            MarkUI_Cir[i + maxMeteor].transform.localScale = new Vector3(1, 1, 1);
                            break;
                        case MarkState.Act2:
                            MarkUI_Cir[i].transform.localScale -= new Vector3(val, val, val);

                            MarkProp[i].Cnt++;
                            if (MarkProp[i].Cnt > waitExpand)
                            {
                                MarkProp[i].markState = MarkState.Ending;
                                MarkUI_Cir[i].SetActive(false); // 非表示にする
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
                        targetDirection = MarkUI_Cir[i].transform.position - MainCam.transform.position;
                        //targetDirection.y = 0; // Y方向は無視する

                        if (targetDirection.magnitude > 0.01f) // 小さな値より大きい場合
                        {
                            targetDirection.Normalize(); // 正規化して方向ベクトルを得る
                            targetDirection += targetDirection * 2.0f;  // どれくらい前方に表示させるか、ここで調整(2.0f)
                        }
                    }

                    MarkUI_Cir[i].transform.position = instantiatedMeteors[i].transform.position - targetDirection;
                    MarkUI_Cir[i].transform.rotation = Quaternion.LookRotation(MarkUI_Cir[i].transform.position - MainCam.transform.position);
                    // アニメーションなしのマーク
                    MarkUI_Cir[i + maxMeteor].transform.position = instantiatedMeteors[i].transform.position - targetDirection;
                    MarkUI_Cir[i + maxMeteor].transform.rotation = Quaternion.LookRotation(MarkUI_Cir[i].transform.position - MainCam.transform.position);
                }
                else
                {
                    MarkUI_Cir[i].SetActive(false); // 非表示にする
                    MarkUI_Cir[i].transform.localScale = new Vector3(5, 5, 5);
                    MarkProp[i].markState = MarkState.Waiting;
                    MarkProp[i].Usable = false;

                    // アニメーションなしのマーク
                    MarkUI_Cir[i + maxMeteor].SetActive(false); // 表示する
                    MarkUI_Cir[i + maxMeteor].transform.localScale = new Vector3(5, 5, 5);
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

        // カメラの前方ベクトルを取得
        Vector3 cameraForward = Camera.main.transform.forward;
        float closestAngle = float.MaxValue; // 最も小さい角度を見つけるための初期値
        Vector3 pos = new Vector3(0, 0, 0);

        for (int i = 0; i < MaxMarkUI; i++)
        {
            if (MarkProp[i].Usable)
            {
                // オブジェクトへのベクトルを取得
                Vector3 directionToObj = (MarkUI_Cir[i].transform.position - Camera.main.transform.position).normalized;

                // 角度を計算
                float angle = Vector3.Angle(cameraForward, directionToObj);

                // 最小の角度を持つオブジェクトを見つける
                if (angle < closestAngle)
                {
                    closestAngle = angle;
                    pos = MarkUI_Cir[i].transform.position;
                }

                usable = true;
            }
        }

        return (usable, pos);
    }
}




//public class Example : MonoBehaviour
//{
//    void Start()
//    {
//        var result = GetBoolAndVector();
//        bool myBool = result.Item1;
//        Vector3 myVector = result.Item2;

//        Debug.Log($"Bool: {myBool}, Vector: {myVector}");
//    }

//    (bool, Vector3) GetBoolAndVector()
//    {
//        bool someCondition = true; // 任意の条件
//        Vector3 someVector = new Vector3(1, 2, 3); // 任意のVector3

//        return (someCondition, someVector);
//    }
//}