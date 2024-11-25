using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoControl : MonoBehaviour
{
    [SerializeField]private VideoPlayer videoPlayer; // VideoPlayer���A�^�b�`

    private void Start()
    {
        // VideoPlayer���A�^�b�`����Ă��Ȃ���Ύ擾
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }

        // VideoPlayer���Đ��J�n���ꂽ��t���[�����Ď�
        videoPlayer.Play();
    }

    private void Update()
    {
        if (videoPlayer.isPlaying)
        {
            // ���t���[������10�t���[���O�ňꎞ��~
            if (videoPlayer.frame >= (long)(videoPlayer.frameCount - 10))
            {
                videoPlayer.Pause();
            }
        }
    }
}
