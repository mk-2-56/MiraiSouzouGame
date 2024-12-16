using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using AU;

public class GameUIManager : UIManager
{
    [SerializeField] private GameObject UI3;
    [SerializeField] private GameObject UI2;
    [SerializeField] private GameObject UI1;
    [SerializeField] private GameObject UIGO;
    [SerializeField] private GameObject MiniMap;
    [SerializeField] private GameObject MiniMapCamera;
    [SerializeField] private GameObject FinishUI;
    [SerializeField] private GameObject playerManager;
    [SerializeField] private GameObject centerLine;
    [SerializeField] private GameObject iconPrefab;
    [SerializeField] private Sprite p1Icon;
    [SerializeField] private Sprite p2Icon;


    private PlayerManager pm;
    private float countStartTime;

    private bool countActive;
    private int countDown;

    private Vector3 UIscale;

    private int iconCount;

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
        pm = playerManager.GetComponent<PlayerManager>();
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

        if (Input.GetKeyDown(KeyCode.G))
        {
            StartCount();  
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            ShowFinish();  
        }

        if (countActive)
        {
            UpdateCount();
        }



    }

    //カウントダウンを始めたい時にこの関数を呼んでください。
    public void StartCount()
    {
        countStartTime = Time.time;
        countActive= true;
        countDown = 4;
        pm.SetPlayerControl(false);

    }
    public void UpdateCount()
    {
        float countDT = Time.time - countStartTime;
        if((countDT>0.0f)&&(countDown==4))
        {
            countDown = 3;

            UI3.SetActive(true);
            UI3.GetComponent<Image>().DOFade(0.0f, 1.0f).Play();
            

        }
        else if((countDT>1.0f)&&(countDown==3))
        {
            countDown = 2;

            UI3.SetActive(false);
            UI2.SetActive(true);
            UI2.GetComponent<Image>().DOFade(0.0f, 1.0f).Play();


        }
        else if((countDT>2.0f) &&(countDown==2))
        {
            countDown = 1;

            UI2.SetActive(false);

            UI1.SetActive(true);
            UI1.GetComponent<Image>().DOFade(0.0f, 1.0f).Play();

        }
        else if((countDT>3.0f) &&(countDown==1))
        {
            countDown = 0;
            UI1.SetActive(false);

            UIGO.SetActive(true);
            UIGO.GetComponent<Image>().DOFade(0.0f, 1.0f).Play();
            playerManager.GetComponent<PlayerManager>().SetPlayerControl(true);


        }
        else if((countDT>4.0f) &&(countDown==0))
        {
            countActive = false;

            UIGO.SetActive(false);

        }
    }

    public void ShowFinish()
    {
        FinishUI.SetActive(true);
        FinishUI.GetComponent<RectTransform>()
            .DOScale(1.0f, 0.7f)
            .Play();
    }

    public void AddPlayerIcon(Transform transform)
    {
        // アイコンのインスタンスを生成

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
