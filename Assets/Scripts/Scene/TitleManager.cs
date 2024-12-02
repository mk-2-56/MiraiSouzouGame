using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : BaseSceneManager
{
    // Start is called before the first frame update
    [SerializeField] AudioSource bgm;
    [SerializeField] AudioSource clickButton;
    [SerializeField] GameManager gameManager;

    public override void Initialized()
    {
        SoundManager.Instance?.PlayBGM(bgm,BGMSoundData.BGM.BGM_Title);
    }
    void Start()
    {
        SoundManager.Instance?.PlayBGM(bgm);
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Return))
        {

            if (gameManager != null)
            {
                gameManager.GoToNextState();
            }
            else
            {
                Debug.LogError("GameManagerが設定されていません");
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            //入力SE
            SoundManager.Instance?.PlaySE(clickButton, SESoundData.SE.SE_COIN);
        }
    }
}
