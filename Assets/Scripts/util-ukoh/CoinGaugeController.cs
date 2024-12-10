using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinGaugeController : MonoBehaviour
{
    // Start is called before the first frame update
    public event System.Action GaugeEffect;

    private float gaugeValue = 0f;
    [SerializeField] private Image image;
    void Start()
    {
        gaugeValue = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetGaugeValue(float value)
    {
        image?.fillAmount += value;
    }
}
