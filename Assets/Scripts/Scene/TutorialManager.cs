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
        // Enterキーが押されたら次の状態に遷移
        if (Input.GetKeyDown(KeyCode.Return)) // KeyCode.ReturnはEnterキー
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
    }
}
