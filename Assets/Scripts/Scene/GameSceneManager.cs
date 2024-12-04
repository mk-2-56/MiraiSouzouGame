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
        SoundManager.Instance?.PlayBGM(BGMSoundData.BGM.BGM_Game);
          
        Debug.Log("Play BGM : " + (BGMSoundData.BGM.BGM_Game));
    }
    void Start()
    {
        Debug.Log("GameSceneManager Entered : Start");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
