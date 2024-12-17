using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaBGMTrigger : MonoBehaviour
{
    [SerializeField] private List<BGMSoundData.BGM> areaBgms; // このエリアのBGM
    [SerializeField] private bool stopPreviousBGM = true; // 前のBGMを停止するかどうか

    private void OnTriggerEnter(Collider other)
    {
        // プレイヤーがエリアに入ったとき
        if (other.CompareTag("Player")) // プレイヤーのTagが"Player"の場合
        {
            if (stopPreviousBGM)
            {
                SoundManager.Instance?.FadeOutAllBGM(1f);
                PlayListBGM();
            }
            else
            {
                SoundManager.Instance?.FadeOutAllBGM(1f);
                PlayListBGM();
            }
        }
    }

    private void PlayListBGM()
    {
        foreach (BGMSoundData.BGM bgm in areaBgms)
        {
            SoundManager.Instance?.PlayBGM(bgm);
            Debug.Log("Play Area BGM : " + bgm.ToString());
        }
    }
}
