using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollector : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private float CoinMax = 20f;
    [SerializeField] private float plusValuePerTimes = 1.0f;

    private float curCoin = 0f;

    void Start()
    {
        CoinGaugeController coinGaugeController = transform.parent.parent.parent.parent.GetComponent<CoinGaugeController>();
        transform.parent.GetComponent<PlayerEffectDispatcher>().CoinCollectedE += SetGaugeValue(plusValuePerTimes);
        curCoin = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetOneCoin()
    {
        (curCoin += 1f) > CoinMax ? CoinMax : curCoin;

        coinGaugeController.SetGaugeValue(plusValuePerTimes/CoinMax);
    }

    public void CoinReset()
    {
        curCoin = 0f;
        coinGaugeController.SetGaugeValue(-1f);
    }



}
