using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCanvasController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Canvas { get { return _canvas; } set { _canvas = value; } }

    public CoinGaugeController _gaugeController;
    public CoinCollector _coinCollector;

    private GameObject _canvas;


    void Start()
    {
        if (Canvas == null) return;
        GameObject gaugeObject = new GameObject("GaugeController");
        _gaugeController = gaugeObject.AddComponent<CoinGaugeController>();

        GameObject collectorObject = new GameObject("CoinCollector");
        _coinCollector = collectorObject.AddComponent< CoinCollector >();

        // Canvasの子オブジェクトに設定
        gaugeObject.transform.SetParent(Canvas.transform);
        collectorObject.transform.SetParent(Canvas.transform);

    }


}
