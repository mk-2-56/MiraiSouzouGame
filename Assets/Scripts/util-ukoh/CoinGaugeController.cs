using UnityEngine;
using UnityEngine.UI;
using static Unity.VisualScripting.Member;

public class CoinGaugeController : MonoBehaviour
{
    public void Initialized()
    {
        image = GetComponent<Image>();

        if (image != null)
        {
            image.fillAmount = gaugeValue;

            SetGaugeValue(gaugeValue); // 初期ゲージ値
        }

        coinCollector = transform.parent.Find("CoinCollector").GetComponent<CoinCollector>();
        if (coinCollector != null)
        {
            coinCollector._CoinCollectedE += AddGaugeValue;
        }

        isFilled = false;
    }
    public float GetGaugeValue() { return gaugeValue; }
    public bool isFilled { set; get; } = false;

    public void SetGaugeValue(float value)
    {
        gaugeValue = Mathf.Clamp01(value); // ゲージ値を0〜1の範囲に制限
        if (gaugeValue > 0.99f && !isFilled)
        {
            image.fillAmount = 1.0f;
            image.color = _fulledColor;
            PlayFulledSE();
            isFilled = true;
        }
        else 
        {
            image.fillAmount = gaugeValue;
            image.color = _notFulledColor;
            isFilled = false;
        }
        Debug.Log(gaugeValue.ToString() + "." + image.fillAmount.ToString());
    }

    public void AddGaugeValue(float value)
    {
        if (isFilled == true && value > 0) 
        {
            Debug.Log("ゲージ溜まったよ");
            return;
        }
        SetGaugeValue(gaugeValue + value);
    }
    public void PlayFulledSE()
    {
        if (isFilled) return;
        _gaugeFilledAS.Play();
        /*        SoundManager.Instance?.PlaySE(_gaugeFilledAS);
        */
    }

    public void  UpdateGauge()
    {
        if (!isFilled) return;
        // ゲージの進捗に応じてちらつき速度を調整
        _flickerSpeed = Mathf.Lerp(_minFlickerSpeed, _maxFlickerSpeed, gaugeValue);

        // タイマーでちらつきのタイミングを制御
        _timer += Time.deltaTime;
        if (_timer >= _flickerSpeed)
        {
            _timer = 0f; // タイマーをリセット

            // アルファ値を切り替え
            Color currentColor = image.color;
            if (_isFadingIn)
            {
                currentColor.a = 1f;
            }
            else
            {
                currentColor.a = 0f;
            }
            image.color = currentColor;
            _isFadingIn = !_isFadingIn; // 状態を切り替え
        }
    }
    [SerializeField] Color _notFulledColor = Color.white;
    [SerializeField] Color _fulledColor = Color.blue;     // ゲージ満タン時の色

    [SerializeField] AudioSource _gaugeFilledAS;
    [SerializeField] private float gaugeValue = 0f; // 現在のゲージ値
    [SerializeField] private float _minFlickerSpeed = 0.5f;
    [SerializeField] private float _maxFlickerSpeed = 0.1f;
    private float _flickerSpeed;  // 現在のちらつき速度

    private bool _isFadingIn = true; // フェードイン中かどうか
    private float _timer = 0f; // フェードのタイマー
    private Image image;           // UI上のゲージイメージ
    private CoinCollector coinCollector;
   
    
  

}