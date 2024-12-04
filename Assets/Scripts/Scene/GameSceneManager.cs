using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneManager : BaseSceneManager
{
    [SerializeField]  GameManager gameManager;
    /*    private PlayerManager playerManager;
    */    // Start is called before the first frame update

    [SerializeField] private BGMSoundData.BGM bgm;
    public override void Initialized()
    {
        //if (gameManager == null) return;

        Debug.Log("Initialized called in GameSceneManager");
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
