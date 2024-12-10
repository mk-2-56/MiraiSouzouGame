using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class CoinCollector : MonoBehaviour
{
    // Start is called before the first frame update

    private PlayerEffectDispatcher playerEffectDispatcher;
    [SerializeField] private float CoinMax = 20f;
    [SerializeField] private float plusValuePerCoin = 1.0f;

    private float curCoin = 0f;

    void Start()
    {
        playerEffectDispatcher = transform.parent.GetComponent<PlayerEffectDispatcher>();
        if(playerEffectDispatcher != null)
        {
            playerEffectDispatcher.CoinE += GetOneCoin;
        }
        curCoin = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetOneCoin(float value)
    {
        curCoin += value;
        // �R�C���擾���ɃQ�[�W�X�V�C�x���g�𔭉�
        playerEffectDispatcher.DispatchGaugeEvent(plusValuePerCoin / CoinMax);
    }

    public void CoinReset()
    {
        curCoin = 0f;
    }



}
