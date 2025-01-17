using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : BaseSceneManager
{
    //// Start is called before the first frame update
    //[SerializeField] AudioSource bgm;
    //[SerializeField] AudioSource clickButton;
    [SerializeField] GameManager gameManager;

    public override void Initialized()
    {
        // SoundManagerにBGM_Titleを指定して再生
        SoundManager.Instance.PlayBGM(BGMSoundData.BGM.BGM_Title);
        /*        SoundManager.Instance?.PlayBGM(bgm,BGMSoundData.BGM.BGM_Title);
        */
    }
    void Start()
    {

    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            SoundManager.Instance.PlaySE(SESoundData.SE.SE_Click);
            //if (gameManager != null)
            //{

            //    gameManager.GoToNextState();
            //}
            //else
            //{
            //    Debug.LogError("GameManagerが設定されていません");
            //}
        }
        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
        {
            SoundManager.Instance.PlaySE(SESoundData.SE.SE_Select);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SoundManager.Instance.PlaySE(SESoundData.SE.SE_Cancel);
        }
    }
}
