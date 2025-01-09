using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultSceneManager : BaseSceneManager
{
    [SerializeField] GameManager gameManager;

    [SerializeField] private BGMSoundData.BGM bgm;

    public override void Initialized()
    {
        SoundManager.Instance?.SetBGMVolume(1);
        SoundManager.Instance?.SetSEVolume(1);
        //SoundManager.Instance?.PlayBGM(BGMSoundData.BGM.BGM_Result);
        //Debug.Log("Play BGM : " + (BGMSoundData.BGM.BGM_Result));
    }
    //void Start()
    //{

    //}

    //void Update()
    //{

    //}
}
