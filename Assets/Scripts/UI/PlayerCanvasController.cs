using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;
using TMPro;
public class PlayerCanvasController : MonoBehaviour
{
    public CoinGaugeController _gaugeController;
    public CoinCollector       _coinCollector;
    public GameObject Canvas { get { return _canvas; } set { _canvas = value; } }

    [SerializeField] float param_minSpeed = 40f;
    [SerializeField] float param_maxSpeed = 100f;
    [SerializeField] Color _startColor = Color.white;
    [SerializeField] Color _middleColor = Color.yellow;
    [SerializeField] Color _endColor = Color.red;
    private GameObject _canvas;
    private CoinGaugeController gaugeController;
    private TextMeshProUGUI textMeshPro;
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
        textMeshPro = _canvas.transform.Find("SpeedText").GetComponent<TextMeshProUGUI>();

        this.transform.Find("Facing/Cog/EffectDispatcher").GetComponent<PlayerEffectDispatcher>().SpeedE += SetSpeedText;
    }

    private void Update()
    {//Debug
/*        if (Input.GetKey(KeyCode.LeftShift))
        {
            AddGaugeVaule(-0.01f);
        }*/

        
    }

    public void SetSpeedText(float speed)
    {
        textMeshPro.text = speed.ToString("F0");
        float range = param_maxSpeed - param_minSpeed;
        float d = (speed - param_minSpeed);
        float k = Mathf.Clamp(d / range, 0, 1);

        float intensity = 3 * k * k - 2 * k * k * k;
        intensity *= 2;
        if (intensity > 1) {textMeshPro.color = Color.Lerp(_middleColor, _endColor, intensity - 1f);}
        else { { textMeshPro.color = Color.Lerp(_startColor, _middleColor, intensity); } }        
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
