using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using static UnityEngine.Rendering.DebugUI;
using AU;

public class PlayerCanvasController : MonoBehaviour
{
    public CoinCollector       _coinCollector;
    public GameObject Canvas { get { return _canvas; } set { _canvas = value; } }
    public CC.Hub playerHub;
    private Image _focusOverlay;

    [SerializeField] float param_minSpeed = 40f;
    [SerializeField] float param_maxSpeed = 100f;
    [SerializeField] Color _startColor = Color.white;
    [SerializeField] Color _middleColor = Color.yellow;
    [SerializeField] Color _endColor = Color.red;

    private GameObject _canvas;
    private CoinGaugeController gaugeController;
    private TextMeshProUGUI textMeshPro;
    private GameObject rank_1st;
    private GameObject rank_2nd;
    private GameObject ui_finish;
    private bool isPlayerBoosting = false;
    public void Initialized()
    {
        if (Canvas == null) {
            UnityEngine.Debug.LogError("PlayerCanvasController: Canvas or Camera is not assigned.");
            return;
        }
        {
            isPlayerBoosting = false;
        }

        {//コントローラー初期化処理

            GameObject collectorObject = new GameObject("CoinCollector");
            _coinCollector = collectorObject.AddComponent<CoinCollector>();
            // Canvasの子オブジェクトに設定
            collectorObject.transform.SetParent(Canvas.transform);
        }

        // Canvasをカメラにリンク
        Canvas canvasComponent = _canvas.GetComponent<Canvas>();
        canvasComponent.renderMode = RenderMode.ScreenSpaceCamera;

        gaugeController = _canvas.transform.Find("gauge_red").GetComponent<CoinGaugeController>();
        gaugeController.Initialized();
        textMeshPro = _canvas.transform.Find("SpeedText").GetComponent<TextMeshProUGUI>();
        rank_1st = _canvas.transform.Find("1stGold").gameObject;
        rank_2nd = _canvas.transform.Find("2ndSliver").gameObject;
        ui_finish = _canvas.transform.Find("Finish").gameObject;
        _focusOverlay = _canvas.transform.Find("FocusEffect").gameObject.GetComponent<Image>();
        //
        this.transform.Find("Facing/Cog/EffectDispatcher").GetComponent<PlayerEffectDispatcher>().SpeedE += SetSpeedText;

        
    }

    public void EnableFocusEffect()
    {
        StartCoroutine(EffectUpdate(true, 1.2f));
        Invoke("DisableFocusEffect", 1.5f);
    }

    public void DisableFocusEffect()
    {
        StartCoroutine(EffectUpdate(false, 1.2f));
    }

    IEnumerator EffectUpdate(bool fIn, float duration)
    {
        float t = 1.0f;
        if (fIn)
        {
            t = 0.0f;
            _focusOverlay.enabled = true;
            StartCoroutine(AU.Fader.FadeIn(t, value => { t = value; }, duration));
        }
        else
        {
            StartCoroutine(AU.Fader.FadeOut(t, value => { t = value; }, duration));
        }

        if(fIn)
        {
            while (true)
            {
                _focusOverlay.material.SetFloat("_Intensity", t);
                yield return new WaitForFixedUpdate();

                if (fIn && t > 1.0f ||
                    !fIn && t < 0.0f)
                    break;
            }
        }

        if (!fIn) 
        { _focusOverlay.enabled = false;
        }
    }


    private void Update()
    {//Debug

    }

    private void FixedUpdate()
    {
        if (Canvas == null) return;

        if (!(gaugeController.isFilled) && !isPlayerBoosting)
        {
            AddGaugeVaule(0.002f);//ゲージが徐々に増えてく
        }

        gaugeController.UpdateGauge();
        if (isPlayerBoosting == true)
        {
            AddGaugeVaule(-0.006f);
        }
        if (playerHub.curPosition == 1)
        {
            rank_1st.SetActive(true);
            rank_2nd.SetActive(false);
        }
        else
        {
            rank_1st.SetActive(false);
            rank_2nd.SetActive(true);
        }
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

    public void SetGaugeToBoost(bool isEnable)
    {
        isPlayerBoosting = isEnable;
    }

    public void ShowFinish()
    {
        SoundManager.Instance?.PlaySE(SESoundData.SE.SE_Goal);
        ui_finish.SetActive(true);
        ui_finish.GetComponent<RectTransform>().DOScale(1.0f, 0.7f);
    }
}
