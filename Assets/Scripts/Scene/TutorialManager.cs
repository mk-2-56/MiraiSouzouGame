using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BGMSoundData;

public class TutorialManager : BaseSceneManager
{
    [SerializeField] private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {

    }
    public override void Initialized()
    {
/*        SoundManager.Instance?.PlayBGM(bgm, BGMSoundData.BGM.BGM_Tutorial);
*/   
    }
    void Update()
    {
        // Enter�L�[�������ꂽ�玟�̏�ԂɑJ��
        if (Input.GetKeyDown(KeyCode.Return)) // KeyCode.Return��Enter�L�[
        {
            if (gameManager != null)
            {
                gameManager.GoToNextState();
            }
            else
            {
                Debug.LogError("GameManager���ݒ肳��Ă��܂���");
            }
        }
    }
}
