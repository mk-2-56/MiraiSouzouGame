using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor.Search;
#endif
using UnityEngine;

public class CoinCollector : MonoBehaviour
{
    // Start is called before the first frame update
    public event System.Action<float> _CoinCollectedE;

    private PlayerCanvasController playerCanvasController;
    [SerializeField] private float CoinMax = 20f;
    [SerializeField] private float plusValuePerCoin = 1.0f;

    private float totalCoins = 0f;   // スコアとしての合計コイン数

    void Start()
    {
        playerCanvasController = transform.parent.GetComponent<PlayerCanvasController>();
        if(playerCanvasController != null)
        {
            //playerEffectDispatcher.CoinE += GetOneCoin;
        }
        totalCoins = 0f;
    }

    public void GetOneCoin()
    {
        totalCoins += plusValuePerCoin;

        _CoinCollectedE.Invoke(plusValuePerCoin / CoinMax);

    }

    public void CoinReset()
    {
        totalCoins = 0f;
    }
    public float GetTotalCoins()
    {
        return totalCoins; // 現在のコイン数を取得
    }
}
