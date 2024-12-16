using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaBGMTrigger : MonoBehaviour
{
    [SerializeField] private List<BGMSoundData.BGM> areaBgms; // ���̃G���A��BGM
    [SerializeField] private bool stopPreviousBGM = true; // �O��BGM���~���邩�ǂ���

    private void OnTriggerEnter(Collider other)
    {
        // �v���C���[���G���A�ɓ������Ƃ�
        if (other.CompareTag("Player")) // �v���C���[��Tag��"Player"�̏ꍇ
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
