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
     
    }
}
