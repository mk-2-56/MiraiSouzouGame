using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinGaugeController : MonoBehaviour
{
    // Start is called before the first frame update

    private float gaugeValue = 0f;
    [SerializeField] private Image image;
    void Start()
    {
        gaugeValue = 0f;
        transform.parent.GetComponent<PlayerEffectDispatcher>().GaugeE += SetGaugeValue;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetGaugeValue(float value)
    {
        if (image == null) return;
        gaugeValue += value;
        image.fillAmount = Mathf.Clamp01(gaugeValue); // �Q�[�W�l��0�`1�̊Ԃɐ���
    }
}
