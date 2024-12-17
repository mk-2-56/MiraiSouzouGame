using UnityEngine;
using UnityEngine.UI;

public class CoinGaugeController : MonoBehaviour
{
    public float GetGaugeValue() { return gaugeValue; }

    public void SetGaugeValue(float value)
    {
        gaugeValue = Mathf.Clamp01(value); // ゲージ値を0〜1の範囲に制限
        UpdateGaugeUI();
    }

    public void AddGaugeValue(float value)
    {
        SetGaugeValue(gaugeValue + value);
    }


    [SerializeField] AudioSource gaugeFilledAS;
    private float gaugeValue = 0f; // 現在のゲージ値
    private Image image;           // UI上のゲージイメージ
    private CoinCollector coinCollector;
    private bool isFilled = false;
    void Start()
    {
        image = GetComponent<Image>();

        if (image != null)
        {
            image.fillAmount = 0f; // 初期化
        }

        coinCollector = transform.parent.Find("CoinCollector").GetComponent<CoinCollector>();
        if (coinCollector != null) {
            coinCollector._CoinCollectedE += AddGaugeValue;
        }

        SetGaugeValue(0f); // 初期ゲージ値
        isFilled = false;

    }

    private void FixeUpdate()
    {
        AddGaugeValue(0.002f);
    }
    private void UpdateGaugeUI()
    {
        if (image != null)
        {
            image.fillAmount = gaugeValue; // UIを更新
            if (image.fillAmount > 0.99f && isFilled == false)
            {
                gaugeFilledAS.Play();
                isFilled = true;
            }
        }
    }

}