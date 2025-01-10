using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TitleUIManager : UIManager
{
    [SerializeField] private GameObject TitleLogo;
    [SerializeField] private GameObject TitleText;

    private Color FadeColor;


    //_“î«‚ğ•Û‚Â‚½‚ßAwake‚ÆStart‚ÍInitialized‚Å‘‚­
    public override void Initialized()
    {
/*        TitleText?.GetComponent<TextMeshProUGUI>().DOFade(0.0f, 1.0f).SetLoops(-1, LoopType.Yoyo).Play();
*/
    }
    // Update is called once per frame
    void Update()
    {

        //TitleText.GetComponent<TextMeshPro>().DOColor(Color.white, 1f).SetLoops(-1, LoopType.Yoyo);
    }

    

}
