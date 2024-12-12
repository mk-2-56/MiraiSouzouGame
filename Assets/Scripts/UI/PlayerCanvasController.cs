using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class PlayerCanvasController : MonoBehaviour
{
    public CoinGaugeController _gaugeController;
    public CoinCollector       _coinCollector;
    public GameObject Canvas { get { return _canvas; } set { _canvas = value; } }
    private GameObject _canvas;
    private CoinGaugeController gaugeController;

    public void Initialized()
    {
        if (Canvas == null) {
            Debug.LogError("PlayerCanvasController: Canvas or Camera is not assigned.");
            return;
        }

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

        gaugeController = _canvas.transform.Find("gauge_red").GetComponent<CoinGaugeController>();       
    }

    private void Update()
    {//Debug
/*        if (Input.GetKey(KeyCode.LeftShift))
        {
            AddGaugeVaule(-0.01f);
        }*/
    }

    public float GetGaugeValue()
    {
        return gaugeController.GetGaugeValue(); ;
    }

    public void AddGaugeVaule(float value)
    {
        gaugeController.AddGaugeValue(value);
    }
    
}
