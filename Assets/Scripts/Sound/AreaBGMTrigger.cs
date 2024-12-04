using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaBGMTrigger : MonoBehaviour
{
    [SerializeField] private BGMSoundData.BGM areaBGM; // ���̃G���A��BGM
    [SerializeField] private bool stopPreviousBGM = true; // �O��BGM���~���邩�ǂ���

    private void OnTriggerEnter(Collider other)
    {
        // �v���C���[���G���A�ɓ������Ƃ�
        if (other.CompareTag("Player")) // �v���C���[��Tag��"Player"�̏ꍇ
        {
            if (stopPreviousBGM)
            {
                SoundManager.Instance.PlayBGM(areaBGM);
            }
            else
            {
                SoundManager.Instance.PlayBGM(areaBGM); // �O��BGM���~���Ȃ��Ȃ畡���Đ��\
            }
        }
    }
}
