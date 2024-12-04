using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaBGMTrigger : MonoBehaviour
{
    [SerializeField] private BGMSoundData.BGM areaBGM; // このエリアのBGM
    [SerializeField] private bool stopPreviousBGM = true; // 前のBGMを停止するかどうか

    private void OnTriggerEnter(Collider other)
    {
        // プレイヤーがエリアに入ったとき
        if (other.CompareTag("Player")) // プレイヤーのTagが"Player"の場合
        {
            if (stopPreviousBGM)
            {
                SoundManager.Instance.PlayBGM(areaBGM);
            }
            else
            {
                SoundManager.Instance.PlayBGM(areaBGM); // 前のBGMを停止しないなら複数再生可能
            }
        }
    }
}
