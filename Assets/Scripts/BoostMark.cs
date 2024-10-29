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
        public bool Play;                                            // UI表示アニメスタートフラグ(未使用)

        // コンストラクタ
        public MarkProperties()
        {
            markState = MarkState.Waiting;
            Cnt = 0;
            Play = false;
        }
    }
    private int waitAppear = 10;                                     // 表示
    private int waitExpand = 180;                                    // 拡大

    // *********************************************************************************************************
    // グローバル変数

    // パブリック変数
    public static int MaxMarkUI = 16;                                               // UIの最大数
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

        List<GameObject> instantiatedMeteors = meteorMovement.GetInstantiatedMeteors();



        if (instantiatedMeteors.Count >= maxMeteor)
        {
            for (int i = 0; i < maxMeteor; i++)
            {
                float distance = Vector3.Distance(MainCam.transform.position, instantiatedMeteors[i].transform.position);
                if (meteorProperties[i].Use && distance <= 25.0f)
                {
                    float val = 1.0f / waitExpand * 4;
                    switch (MarkProp[i].markState)
                    {
                        case MarkState.Waiting:
                            MarkProp[i].Cnt++;
                            if (MarkProp[i].Cnt > waitAppear)
                            {
                                MarkProp[i].markState = MarkState.Act1;
                                // 設定の初期化
                                MarkProp[i].Cnt = 0;
                            }
                            break;
                        case MarkState.Act1:
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
                            break;
                        case MarkState.Act2:
                            MarkUI_Cir[i].transform.localScale -= new Vector3(val, val, val);

                            MarkProp[i].Cnt++;
                            if (MarkProp[i].Cnt > waitExpand)
                            {
                                MarkProp[i].markState = MarkState.Ending;
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
                            targetDirection += targetDirection * 0.01f;
                        }
                    }

                    MarkUI_Cir[i].SetActive(true); // 表示する
                    MarkUI_Cir[i].transform.position = instantiatedMeteors[i].transform.position - targetDirection;
                    MarkUI_Cir[i].transform.rotation = Quaternion.LookRotation(MarkUI_Cir[i].transform.position - MainCam.transform.position);
                }
                else
                {
                    MarkUI_Cir[i].SetActive(false); // 非表示にする
                    MarkUI_Cir[i].transform.localScale = new Vector3(5, 5, 5);
                    MarkProp[i].markState = MarkState.Waiting;
                }
            }
        }
        else
        {
            Debug.LogWarning("Not enough instantiated Meteors.");
        }

    }
}
