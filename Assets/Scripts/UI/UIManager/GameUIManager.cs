using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class GameUIManager : UIManager
{
    [SerializeField] private GameObject UI3;
    [SerializeField] private GameObject UI2;
    [SerializeField] private GameObject UI1;

    List<GameObject> countDownNumbers = new(); 

    [SerializeField] private GameObject UIGO;
    [SerializeField] private GameObject MiniMap;
    [SerializeField] private GameObject MiniMapCamera;
    [SerializeField] private GameObject playerManager;
    [SerializeField] private GameObject centerLine;
    [SerializeField] private GameObject iconPrefab;
    [SerializeField] private Sprite p1Icon;
    [SerializeField] private Sprite p2Icon;
    [SerializeField] private GameCameraManager gameCameraManager;

    private AU.PlayerManager pm;

    bool anyPlayer      = false;
    bool countdownOver  = false;
    private int  countDown;

    private Vector3 UIscale;

    private int iconCount;

    private void Start()
    {
        countDownNumbers.Add(UIGO);
        countDownNumbers.Add(UI1);
        countDownNumbers.Add(UI2);
        countDownNumbers.Add(UI3);
    }

    // Start is called before the first frame update
    public override void Initialized()
    {
        UIscale.x = 5f;
        UIscale.y = 5f;
        UIscale.z = 5f;
        UnityEngine.Debug.Log("init");
        UI3.SetActive(false);
        UI2.SetActive(false);
        UI1.SetActive(false);
        UIGO.SetActive(false);
        UI3.transform.localScale= UIscale;
        UI2.transform.localScale= UIscale;
        UI1.transform.localScale= UIscale;
        UIGO.transform.localScale= UIscale;

        iconCount = 0;
        pm = playerManager.GetComponent<AU.PlayerManager>();
        //StartCount();
        //ShowFinish();

    }

    // Update is called once per frame
    void Update()
    {
        if (pm.GetPlayerCount() == 1)
        {
            RectTransform rt = MiniMap.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(1.0f, 0.0f);
            rt.anchorMax = new Vector2(1.0f, 0.0f);
            rt.pivot = new Vector2(1.0f, 0.0f);
            rt.anchoredPosition = new Vector2(0.0f, 0.0f);

        }
        if (pm.GetPlayerCount() == 2)
        {
            centerLine.SetActive(true);
            RectTransform rt = MiniMap.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0.5f, 0.5f);
            rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.anchoredPosition = new Vector2(0.0f, 0.0f);
        }
    }

    //カウントダウンを始めたい時にこの関数を呼んでください。
    public void StartCount()
    {
        countdownOver = false;
        StartCoroutine(WaitForAnyPlayer());
    }


    IEnumerator WaitForAnyPlayer()
    {
        while (true)
        {
            if (anyPlayer)
            {
                countDown = 4;
                StartCoroutine(UpdateCountdown());
                SoundManager.Instance?.PlaySE(SESoundData.SE.SE_CountDown);
                break;
            }
            yield return new WaitForFixedUpdate();
        }
    }
    IEnumerator UpdateCountdown()
    {
        GameObject cur = null;
        GameObject old = null;

        while (true)
        {
            countDown--;
            Debug.Log(countDown);

            if(old)
                old.SetActive(false);   //deactivate old sprite

            if (countDown < 0)
            {
                countdownOver = true;
                pm.SetPlayerControl(true);
                break;
            }
            cur = countDownNumbers[countDown];

            cur.SetActive(true);    //activate appropriate sprite
            cur.GetComponent<Image>().DOFade(0.0f, 1.0f).Play();

            old = cur;

            yield return new WaitForSeconds(1.0f);
        }
    }


    public void AddPlayerIcon(Transform transform)
    {
        anyPlayer |= true;

        // アイコンのインスタンスを生成
        if(!countdownOver)
            transform.root.gameObject.GetComponent<CC.Hub>().disableInput = true;

        GameObject playerIcon = Instantiate(iconPrefab, MiniMap.transform);

        // ミニマップ上のアイコンの位置を設定
        playerIcon.GetComponent<MiniMapIcon>().targetObject = transform;
        playerIcon.GetComponent<MiniMapIcon>().MiniMapCamera = MiniMapCamera.GetComponent<Camera>();

        if (iconCount==0)
        {
            playerIcon.GetComponent<Image>().sprite = p1Icon;

        }
        else if (iconCount==1)
        {
            playerIcon.GetComponent<Image>().sprite = p2Icon;

        }
        iconCount++;
    }

}
