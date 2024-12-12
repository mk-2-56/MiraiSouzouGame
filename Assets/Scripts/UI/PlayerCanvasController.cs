using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class PlayerCanvasController : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private GameObject playerManager;
    [SerializeField] private Vector3 gaugePos;
    [SerializeField] private Vector3 rankPos;
    [SerializeField] private Vector3 needlePos;
    [SerializeField] private Vector3 speedMeterPos;

    public CoinGaugeController _gaugeController;
    public CoinCollector _coinCollector;
    public GameObject Canvas { get { return _canvas; } set { _canvas = value; } }
    private GameObject _canvas;

    public void Initialized()
    {
        if (Canvas == null) {
            Debug.LogError("PlayerCanvasController: Canvas or Camera is not assigned.");
            return;
        }
        playerManager = GameObject.Find("PlayerManager");


        {//コントローラー初期化処理
            GameObject gaugeObject = new GameObject("GaugeController");
            _gaugeController = gaugeObject.AddComponent<CoinGaugeController>();

            GameObject collectorObject = new GameObject("CoinCollector");
            _coinCollector = collectorObject.AddComponent<CoinCollector>();
            // Canvasの子オブジェクトに設定
            gaugeObject.transform.SetParent(Canvas.transform);
            collectorObject.transform.SetParent(Canvas.transform);
        }

        // Canvasをカメラにリンク
        Canvas canvasComponent = _canvas.GetComponent<Canvas>();
        canvasComponent.renderMode = RenderMode.ScreenSpaceCamera;

        int playerAmount = playerManager.gameObject.GetComponent<AU.PlayerManager>().GetPlayerCount();

        InitPlayerUI(playerAmount);

     
    }

    private void InitPlayerUI(int playerAmount)
    {
        _canvas.SetActive(true);

        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        if (playerAmount == 0)
        {//プレイヤーが一人だけの場合
/*            SetOnePlayerUI();
*/        }
        else
        {
            //_canvas.transform.Find("1stGold").transform.position = new Vector3(-100f,400f,0f);
            //_canvas.transform.Find("2ndSliver").transform.position = new Vector3(-100f,400f,0f);
        }
    }

    private void SetOnePlayerUI()
    {
        //float screenWidth = Screen.width;
        //float screenHeight = Screen.height;
        //// プレイヤーごとのUI位置を決定
        //Vector2 uiPosition = Vector2.zero;

        //RectTransform tmp;

        //tmp = _canvas.transform.Find("1stGold").gameObject.GetComponent<RectTransform>();
        //tmp.anchoredPosition3D = new Vector3(screenWidth * 0.42f, screenHeight * 0.38f, 0f);

        //tmp = _canvas.transform.Find("2ndSliver").gameObject.GetComponent<RectTransform>();
        //tmp.anchoredPosition3D = new Vector3(-100f, screenHeight * 0.38f, 0f);
        //_canvas.transform.Find("2ndSliver").gameObject.SetActive(false);

        //tmp = _canvas.transform.Find("gauge").gameObject.GetComponent<RectTransform>();
        //tmp.anchoredPosition3D = new Vector3(-(screenWidth * 0.42f), screenHeight * 0.3f, 0f);
        //tmp.sizeDelta = new Vector2(300f, 300f);

        //tmp = _canvas.transform.Find("gauge_red").gameObject.GetComponent<RectTransform>();
        //tmp.anchoredPosition3D = new Vector3(-(screenWidth * 0.42f), screenHeight * 0.3f, 0f);
        //tmp.sizeDelta = new Vector2(300f, 300f);

        //tmp = _canvas.transform.Find("speedMeter").gameObject.GetComponent<RectTransform>();
        //tmp.anchoredPosition3D = new Vector3(0f, -screenHeight * 0.4f, 0f);
        //tmp.sizeDelta = new Vector2(300f, 150f);

        //tmp = _canvas.transform.Find("speedMeter").gameObject.GetComponent<RectTransform>();
        //tmp.anchoredPosition3D = new Vector3(0f, -screenHeight * 0.4f, 0f);
        //tmp.sizeDelta = new Vector2(300f, 150f);

        //tmp = _canvas.transform.Find("needle").gameObject.GetComponent<RectTransform>();
        //tmp.anchoredPosition3D = new Vector3(screenWidth * 0.026f, -(screenHeight * 0.43f), 0f);
        //tmp.sizeDelta = new Vector2(150f, 75f);

    }

    private void SetPlayerUI(int index)
    {
        
        //float screenWidth = Screen.width;
        //float screenHeight = Screen.height;
        //// プレイヤーごとのUI位置を決定
        //Vector2 uiPosition = Vector2.zero;

        //RectTransform tmp;

        //float is1P = -1.0f;
        //if (index == 1) { is1P = 1f; }

        //tmp = _canvas.transform.Find("1stGold").gameObject.GetComponent<RectTransform>();
        //tmp.anchoredPosition3D = new Vector3(screenWidth * 0.1f * is1P, screenHeight * 0.38f, 0f);

        //tmp = _canvas.transform.Find("2ndSliver").gameObject.GetComponent<RectTransform>();
        //tmp.anchoredPosition3D = new Vector3(screenWidth * 0.1f * is1P, screenHeight * 0.38f, 0f);
        //_canvas.transform.Find("2ndSliver").gameObject.SetActive(false);

        //tmp = _canvas.transform.Find("gauge").gameObject.GetComponent<RectTransform>();
        //tmp.anchoredPosition3D = new Vector3((screenWidth * 0.42f) * is1P, screenHeight * 0.3f, 0f);
        //tmp.sizeDelta = new Vector2(300f, 300f);

        //tmp = _canvas.transform.Find("gauge_red").gameObject.GetComponent<RectTransform>();
        //tmp.anchoredPosition3D = new Vector3((screenWidth * 0.42f) * is1P, screenHeight * 0.3f, 0f);
        //tmp.sizeDelta = new Vector2(300f, 300f);

        //tmp = _canvas.transform.Find("speedMeter").gameObject.GetComponent<RectTransform>();
        //tmp.anchoredPosition3D = new Vector3((screenWidth * 0.25f) * is1P, -screenHeight * 0.4f, 0f);
        //tmp.sizeDelta = new Vector2(300f, 150f);

        //tmp = _canvas.transform.Find("needle").gameObject.GetComponent<RectTransform>();
        //tmp.anchoredPosition3D = new Vector3(screenWidth * 0.25f * is1P, -(screenHeight * 0.43f), 0f);
        //tmp.sizeDelta = new Vector2(150f, 75f);

    }
}
