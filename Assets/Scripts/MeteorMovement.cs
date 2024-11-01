using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorMovement : MonoBehaviour
{
    public struct MeteorProperties
    {
        public Vector3 Pos;
        public Vector3 SpawnPos;                                     // 噴石のスポーン地点
        public int Time;                                             // スポーンさせる時間
        public bool Use;                                             // 使用中フラグ
        public bool WillUse;                                         // 使用する予定フラグ
        public bool OnGround;                                        // 着地フラグ
        public int CntDespawn;                                       // 時間経過デスポーンのカウンター
    }
    /******************************************************************************************************************************************************************/
    // グローバル変数
    /******************************************************************************************************************************************************************/
    // パブリック変数
    public static int MaxMeteor = 5;                                                     // 噴石の最大数
    public MeteorProperties[] MeteorProp = new MeteorProperties[MaxMeteor];              // 噴石の設定

    // プライベート変数 
    [SerializeField] private GameObject PrefabObj;                                       // 噴石オブジェクトの取得
    private List<GameObject> Meteor = new List<GameObject>();                            // 噴石オブジェクトリスト
    private float Speed = 0.25f;                                                         // 落下速度
    private Vector3 Crater = new Vector3(0.0f, 0.0f, 0.0f);                              // 噴火口の中心座標
    private float Height = 50.0f;                                                        // 噴石のスポーンy座標、いくら上でスポーンさせるか、固定値
    [SerializeField] private int Interval = 300;                                         // 次の噴石の発生間隔
    private int SpawnTimer = 300;                                                        // スポーンタイマー
    // 初期値ゼロの変数
    private int MaxUsingMeteor = 0;                                                      // （プレイヤーが一定の位置にくると1判定）一回の判定で使用する噴石の最大数
    private bool AllStarted = false;                                                     // 一回の判定で使用する噴石をすべて出現させたかフラグ
    private int CntStartedMeteor = 0;                                                    // 出現させた噴石の数のカウント
    private int EventNum = 0;                                                            // 噴石イベントの発生回数のカウント
    private int AddNumforPosTank = 0;                                                    // MeteorsPosTank参照用カウント

    private Vector3[] MeteorsPosTank =                               // 噴石Positionの貯蔵庫
        {
            new Vector3(196.0f, 34.0f, 152.0f),
            new Vector3(220.0f, 36.0f, 151.0f),
            new Vector3(236.0f, 38.0f, 139.0f),

            new Vector3(330.0f, 45.0f, 131.0f),
            new Vector3(358.0f, 44.0f, 150.0f),
            new Vector3(372.0f, 46.0f, 153.0f),
            new Vector3(384.0f, 45.0f, 176.0f),

            new Vector3(0.0f, 0.0f, 90.0f),
            new Vector3(-5.0f, 0.0f, 100.0f),
            new Vector3(5.0f, 0.0f, 110.0f),
        };
    private int[] UseNumAtOnceTank =                                 // 1回のイベントでスポーンさせる噴石の数の貯蔵庫
        {
            3,
            4,
            3,
        };
    private int MaxEventNum = 3;                                     // 最大イベント数（上記のUseNumAtOnceTankに入力した数字の数）

    private bool SpawnMeteor = false;                                // 噴石発生フラグ

    // [SerializeField] 後で追加検討
    // testing




    /******************************************************************************************************************************************************************/
    // 
    /******************************************************************************************************************************************************************/
    private void Awake()
    {
        // オブジェクトを生成＆非表示で保管しておく
        for (int i = 0; i < MaxMeteor; i++)
        {
            GameObject obj = Instantiate(PrefabObj); // オブジェクトを生成する
            Meteor.Add(obj); // リストに追加
            Meteor[i].SetActive(false); // 非表示にする
            // ResetMeteorProperties(i); // 噴石の設定を初期化
        }

        // エラー確認：配列サイズ
        if (MeteorProp.Length != Meteor.Count)
        {
            Debug.Log("MeteorProp & Meteor.Length has different array num, crucial error may occur ");
        }

    }
    //private void ResetMeteorProperties(int index)
    //{
    //    MeteorProp[index] = new MeteorProperties
    //    {
    //        Pos = Vector3.zero,
    //        SpawnPos = Vector3.zero,
    //        Time = 0,
    //        Use = false,
    //        WillUse = false,
    //        OnGround = false,
    //        CntDespawn = 0
    //    };
    //}

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //debug
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ActivateMeteors();
            }
        }

        if (SpawnMeteor)
        {
            UpdateSpawnTimer();
            UpdateMeteors();
        }
    }

    private void UpdateSpawnTimer()
    {
        if (!AllStarted)
        {
            SpawnTimer++;
            for (int i = 0; i < MeteorProp.Length; i++)
            {
                if (SpawnTimer >= MeteorProp[i].Time && MeteorProp[i].WillUse)
                {
                    SetMeteorUse();
                    CntStartedMeteor++;
                }
            }
            if (CntStartedMeteor >= MaxUsingMeteor)
            {
                AllStarted = true;
            }
        }
    }
    private void UpdateMeteors()
    {
        int allInactive = 0;    // すべての噴石が使用されていない状態か確認→すべてUseではないなら、SpawnMeteorフラグをoffにする
        for (int i = 0; i < MeteorProp.Length; i++)
        {
            if (MeteorProp[i].Use)
            {
                float radius = 0.1f;    // 噴石モデルの半径
                if (MeteorProp[i].Pos.y < MeteorProp[i].SpawnPos.y - Height - radius)
                {
                    MeteorProp[i].OnGround = true;
                }
                else
                {
                    // 噴石を落下させる
                    MeteorProp[i].Pos.y -= Speed;

                    // 火口からスポーン地点へ向かう計算
                    Vector3 targetDirection = MeteorProp[i].Pos - Crater;
                    targetDirection.y = 0; // Y方向は無視する

                    if (targetDirection.magnitude > 0.01f) // 小さな値より大きい場合
                    {
                        targetDirection.Normalize(); // 正規化して方向ベクトルを得る
                        MeteorProp[i].Pos += targetDirection * Speed * 0.01f;
                    }
                }
            }
            else
            {
                allInactive++;  // すべての噴石が不使用になったか、チェック
                Meteor[i].SetActive(false); // 非表示にする
            }
        }
        if (allInactive >= MeteorProp.Length)
        {
            // もし、すべての噴石がUseではなくなったら、噴石落下イベントを停止させる
            SpawnMeteor = false;
        }


        // 地面に着地したら、時間経過でデスポーンさせる
        for (int i = 0; i < MeteorProp.Length; i++)
        {
            if (MeteorProp[i].Use && MeteorProp[i].OnGround)
            {
                MeteorProp[i].CntDespawn++;
                if (MeteorProp[i].CntDespawn > 6000)
                {
                    MeteorProp[i].CntDespawn = 0;
                    MeteorProp[i].Use = false;
                }
            }
        }

        // 噴石の設定をオブジェクトへ反映
        {
            for (int i = 0; i < MeteorProp.Length; i++)
            {
                if (MeteorProp[i].Use)
                {
                    Meteor[i].transform.position = MeteorProp[i].Pos;
                }
            }
        }
    }




    /******************************************************************************************************************************************************************/
    // 
    /******************************************************************************************************************************************************************/

    // 噴石のスポーン地点を正確に決める
    private void SetMeteorPos(Vector3 pos)
    {
        int SpawnTime = 0;
        for (int i = 0; i < MeteorProp.Length; i++)
        {
            if (!MeteorProp[i].WillUse)
            {
                MeteorProp[i].SpawnPos = new Vector3(pos.x, pos.y + Height, pos.z);
                MeteorProp[i].Time = SpawnTime;
                MeteorProp[i].WillUse = true;
                return;
            }
            SpawnTime += Interval;
        }

        Debug.Log("(SetMeteorPos) Array is full. Cannot set meteor. ");
    }
    // 噴石を使用中にする
    private void SetMeteorUse()
    {
        for (int i = 0; i < MeteorProp.Length; i++)
        {
            if (MeteorProp[i].WillUse && !MeteorProp[i].Use)
            {
                MeteorProp[i].WillUse = false;
                MeteorProp[i].Use = true;
                Meteor[i].SetActive(true); // 表示する
                MeteorProp[i].Pos = MeteorProp[i].SpawnPos;
                return;
            }
        }

        Debug.Log("(SetMeteorUse) Array is full. Cannot set meteor. ");
    }
    // 噴石ギミックを初期化する
    private void InitMeteors()
    {
        if (EventNum >= MaxEventNum) return;    // 最大イベント数に達したら、実行しない

        if (!SpawnMeteor)
        {
            SpawnMeteor = true;
            SpawnTimer = 0;
            CntStartedMeteor = 0;
            MaxUsingMeteor = UseNumAtOnceTank[EventNum];
            AllStarted = false;
            // 詳細な噴石のスポーン地点を決める
            for (int i = 0; i < MaxUsingMeteor; i++)
            {
                Vector3 pos = MeteorsPosTank[i + AddNumforPosTank];
                SetMeteorPos(pos);

                MeteorProp[i].OnGround = false; // 着地判定初期化
                MeteorProp[i].CntDespawn = 0;
            }

            // 最初の噴石を出現させる
            SetMeteorUse();
            EventNum++;
            AddNumforPosTank += MaxUsingMeteor;
        }
    }
    // 噴石ギミックをActivateする
    public void ActivateMeteors()
    {
        if (SpawnMeteor)
        {
            // 前回発動させた噴石が残っていたら、消去して初期化する
            for (int i = 0; i < MeteorProp.Length; i++)
            {
                MeteorProp[i].WillUse = false;
                if (MeteorProp[i].Use)
                {
                    MeteorProp[i].Use = false;
                    Meteor[i].SetActive(false); // 非表示にする
                }
            }
            SpawnMeteor = false;
        }

        // もし、すべてのイベント噴石を落としたら、ループさせる
        if (EventNum >= MaxEventNum)
        {
            EventNum = 0;
            AddNumforPosTank = 0;
            CntStartedMeteor = 0;
        }

        InitMeteors();
    }


    // ゲッター・セッター
    public List<GameObject> GetInstantiatedMeteors()
    {
        return Meteor; // インスタンス化したオブジェクトのリストを返す
    }
    public int GetMaxMeteor()
    {
        return MaxMeteor;
    }
    public MeteorProperties GetMeteorProperties(int num)
    {
        return MeteorProp[num];
    }
}

