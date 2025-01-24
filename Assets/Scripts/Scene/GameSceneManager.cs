using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneManager : BaseSceneManager
{
    [SerializeField]  GameManager gameManager;
    public override void Initialized()
    {
        SoundManager.Instance?.SetBGMVolume(1);
        SoundManager.Instance?.SetSEVolume(1);
        SoundManager.Instance?.PlayBGM(BGMSoundData.BGM.BGM_Tutorial);
    }
    public void PlayGameBGM()
    {
        SoundManager.Instance?.PlayBGM(BGMSoundData.BGM.BGM_Game);
    }
}
