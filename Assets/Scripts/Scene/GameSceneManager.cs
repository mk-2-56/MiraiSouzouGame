using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneManager : BaseSceneManager
{
    [SerializeField]  GameManager gameManager;

    [SerializeField] private BGMSoundData.BGM bgm;
    public override void Initialized()
    {
        SoundManager.Instance?.SetBGMVolume(1);
        SoundManager.Instance?.SetSEVolume(1);
        SoundManager.Instance?.PlayBGM(bgm);          
    }
}
