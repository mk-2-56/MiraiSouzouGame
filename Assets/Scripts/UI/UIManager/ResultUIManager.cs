using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using AU;
using Cinemachine;

public class ResultUIManager : UIManager
{
    [SerializeField] private GameObject winEffect;
    [SerializeField] private GameObject winText;
    [SerializeField] private Animator resultAnim;
    [SerializeField] private Sprite p1WinImg;
    [SerializeField] private Sprite p2WinImg;
    [SerializeField] private GameObject resultLogo;
    [SerializeField] private GameObject scoreGroup;
    [SerializeField] private GameObject losersTime;
    [SerializeField] private GameObject scoreLine;
    [SerializeField] private Camera gameCamera;
    [SerializeField] private GameObject lastCameraPos;

    private bool animationEnd;
    private bool cameraEnd;
    private int winPlayer;

    public override void Initialized()
    {
        UnityEngine.Debug.Log("init");
        winEffect.SetActive(false);
        winText.SetActive(false);
        resultLogo.SetActive(false);
        scoreGroup.SetActive(false);
        scoreGroup.GetComponent<CanvasGroup>().alpha = 0.0f;
        scoreLine.GetComponent<RectTransform>().localScale = new Vector3(0.0f, 1.0f, 1.0f);

        // èâä˙ílÇÕ1PÇ…ÇµÇ∆Ç≠
        if (GoalChecker.Instance)
            winPlayer = GoalChecker.Instance.GetWinner();
        else winPlayer = 1;
    }

    void Start()
    {
        winText.GetComponent<Image>().sprite = (winPlayer == 1) ? p1WinImg : p2WinImg;
    }

    void Update()
    {
        AnimatorStateInfo stateInfo = resultAnim.GetCurrentAnimatorStateInfo(0);

        // çƒê∂èIóπÇµÇƒÇ¢ÇΩÇÁ
        if (!animationEnd && stateInfo.normalizedTime >= 1.0f)
        {
            animationEnd = true;
            OnAnimationEnd();
        }

        if(!cameraEnd && gameCamera.transform.position == lastCameraPos.transform.position)
        {
            cameraEnd = true;
            ShowScore();
        }
    }


    private void OnAnimationEnd()
    {
        winEffect.SetActive(true);
        winText.SetActive(true);
        winText.GetComponent<RectTransform>().DOLocalMoveX(-170f, 3.5f);
        winText.GetComponent<Image>().DOFade(0.0f, 1.0f).SetDelay(2);
        winEffect.GetComponent<CanvasGroup>().DOFade(0.0f, 0.7f).SetDelay(2);
    }

    private void ShowScore()
    {
        resultLogo.SetActive(true);
        float time, timeLoser;
        if (GoalChecker.Instance)
        {
            time = GoalChecker.Instance.GetWinnerTime();
            timeLoser = GoalChecker.Instance.GetLoserTime();
        }
        else
        {
            time = 0.0f; 
            timeLoser = 0.0f;
        }

        int h = (int)(time / 3600);
        int m = (int)((time % 3600) / 60);
        int s = (int)(time % 60);

        // îsé“
        int hLoser = (int)(timeLoser / 3600);
        int mLoser = (int)((timeLoser % 3600) / 60);
        int sLoser = (int)(timeLoser % 60);

        scoreGroup.transform.Find("Time/Value").GetComponent<TextMeshProUGUI>().text = $"{h}:{m}:{s}"; 
        scoreGroup.transform.Find("TimeLoser/Value").GetComponent<TextMeshProUGUI>().text = $"{hLoser}:{mLoser}:{sLoser}";

        if (winPlayer != 1)
        {
            scoreGroup.transform.Find("Rank/Value1").GetComponent<TextMeshProUGUI>().text = "2P";
            scoreGroup.transform.Find("Rank/Value2").GetComponent<TextMeshProUGUI>().text = "1P";
        }
        scoreGroup.SetActive(true);

        ScoreAnim();
    }

    private void ScoreAnim()
    {
        TextMeshProUGUI tmpro = resultLogo.GetComponent<TextMeshProUGUI>();
        tmpro.color = Color.white;

        DOTweenTMPAnimator tmproAnimator = new DOTweenTMPAnimator(tmpro);

        for (int i = 0; i < tmproAnimator.textInfo.characterCount; ++i)
        {
            tmproAnimator.DORotateChar(i, Vector3.up * 90, 0);
            DOTween.Sequence()
                .Append(tmproAnimator.DORotateChar(i, Vector3.zero, 0.4f))
                .AppendInterval(1f)
                .Append(tmproAnimator.DOColorChar(i, new Color(1f, 1f, 0.8f), 0.2f).SetLoops(2, LoopType.Yoyo))
                .SetDelay(0.14f * i);
        }

        scoreGroup.GetComponent<CanvasGroup>().DOFade(1f, 1f).SetDelay(1);
        // timeValue.GetComponent<TextMeshProUGUI>().DOCounter(0, 00, 2f).SetDelay(1);
        scoreLine.GetComponent<RectTransform>().DOScaleX(1f, 1f).SetDelay(2);
    }


}
