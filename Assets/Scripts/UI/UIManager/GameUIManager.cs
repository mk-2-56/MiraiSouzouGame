using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using AU;
using static CC.PlayerMovementParams;

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

    [SerializeField] private GameObject tutorialCanvas;
    [SerializeField] private GameObject gameUICanvas;
    [SerializeField] private GameObject inTransition;
    [SerializeField] private GameObject outTransition;
    [SerializeField] private GameObject tutorialWindow;
    [SerializeField] private GameObject p1OK;
    [SerializeField] private GameObject p2OK;
    private PlayerManager pm;
    private float countStartTime;

    private bool countActive;
    bool anyPlayer = false;
    bool countdownOver = false;
    public int countDown;

    private Vector3 UIscale;

    private int iconCount;
    public bool mode;//0=tutorial,1=game
    public bool tutorialPlayerControl = true;
    public bool IsTutorial()
    {
        return !mode;
    }
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
        countDown = 3;
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
        p1OK.SetActive(false);
        p2OK.SetActive(false);
        iconCount = 0;
        pm = playerManager.GetComponent<PlayerManager>();
        mode = false;
        StartTutorial();

    }



    // Update is called once per frame
    void Update()
    {
        
        if (tutorialPlayerControl)
        {
            pm.SetPlayerControl(true);
        }
        else
        {
            pm.SetPlayerControl(false);
        }

        if (!mode)
        {
            if (pm.GetPlayerCount() == 0)
            {
                tutorialWindow.SetActive(false);
            }
            else
            {
                tutorialWindow.SetActive(true);
            }
        }
        else if ( mode )
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

            if(!countdownOver)
            {
                pm.FaceFoward();

            }

        }

        //debug
/*        if (UnityEngine.Input.GetKeyDown(KeyCode.H))
        {
            EndTutorial();
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.J))
        {
            StartGame();
        }*/

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
            UnityEngine.Debug.Log(countDown);

            if (old)
                old.SetActive(false);   //deactivate old sprite

            if (countDown < 0)
            {
                countdownOver = true;
                pm.SetPlayerControl(true);
                tutorialPlayerControl = true;
                TimeManager.Instance.StartTimer();
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
        if (!countdownOver)
            transform.root.gameObject.GetComponent<CC.Hub>().disableInput = true;

        GameObject playerIcon = Instantiate(iconPrefab, MiniMap.transform);

        // ミニマップ上のアイコンの位置を設定
        playerIcon.GetComponent<MiniMapIcon>().targetObject = transform;
        playerIcon.GetComponent<MiniMapIcon>().MiniMapCamera = MiniMapCamera.GetComponent<Camera>();

        if (iconCount == 0)
        {
            playerIcon.GetComponent<Image>().sprite = p1Icon;

        }
        else if (iconCount == 1)
        {
            playerIcon.GetComponent<Image>().sprite = p2Icon;

        }
        iconCount++;
    }

    public void StartTutorial()
    {
        pm.SetPlayerControl(true);
        mode = false;
        gameUICanvas.SetActive(false);
        tutorialCanvas.SetActive(true);
        gameCameraManager.SetRenderTarget(0);
        StartInTransition();
    }
    public void EndTutorial()
    {
        tutorialPlayerControl = false;
        pm.SetPlayerControl(false);

        StartOutTransition();
    }

    public void StartGame()
    {
        mode = true;
        gameUICanvas.SetActive(true);
        tutorialCanvas.SetActive(false);
        gameCameraManager.SetRenderTarget(1);
        StartInTransition();
        gameCameraManager.SetStartCameraWork(true);
        SoundManager.Instance?.SetBGMVolume(1);
        SoundManager.Instance?.SetSEVolume(1);
        SoundManager.Instance?.SetMasterVolume(1);
        SoundManager.Instance?.PlayBGM(BGMSoundData.BGM.BGM_Game);

    }
    public void StartInTransition()
    {
        outTransition.SetActive(false);
        inTransition.SetActive(false);
        inTransition.SetActive(true);

    }

    public void StartOutTransition()
    {
        outTransition.SetActive(false);
        outTransition.SetActive(true);
    }

    public void PlayerStanby(int playerNum)
    {
        if (playerNum == 1)
        {
            Player1OK();
        }
        else if (playerNum == 2)
        {
            Player2OK();
        }
    }
    public void Player1OK()
    {
        p1OK.SetActive(true);
    }
    public void Player2OK()
    {
        p2OK.SetActive(true);

    }

}
