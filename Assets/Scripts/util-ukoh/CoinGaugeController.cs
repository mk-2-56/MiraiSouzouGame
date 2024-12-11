using UnityEngine;
using UnityEngine.UI;

public class CoinGaugeController : MonoBehaviour
{
    private float gaugeValue = 0f; // 現在のゲージ値
    private Image image;           // UI上のゲージイメージ
    private CoinCollector coinCollector;
    void Start()
    {
        image = GetComponent<Image>();

        if (image != null)
        {
            image.fillAmount = 0f; // 初期化
        }

        coinCollector = FindObjectOfType<CoinCollector>();
        if (coinCollector != null) {
            coinCollector._CoinCollectedE += AddGaugeValue;
        }
        SetGaugeValue(0f); // 初期ゲージ値
    }

    public void SetGaugeValue(float value)
    {
        gaugeValue = Mathf.Clamp01(value); // ゲージ値を0〜1の範囲に制限
        UpdateGaugeUI();
    }

    public void AddGaugeValue(float value)
    {
        SetGaugeValue(gaugeValue + value);
    }

    private void UpdateGaugeUI()
    {
        if (image != null)
        {
            image.fillAmount = gaugeValue; // UIを更新
        }
    }
}