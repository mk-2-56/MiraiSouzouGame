using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoControl : MonoBehaviour
{
    [SerializeField]private VideoPlayer videoPlayer; // VideoPlayerをアタッチ

    private void Start()
    {
        // VideoPlayerがアタッチされていなければ取得
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }

        // VideoPlayerが再生開始されたらフレームを監視
        videoPlayer.Play();
    }

    private void Update()
    {
        if (videoPlayer.isPlaying)
        {
            // 総フレーム数の10フレーム前で一時停止
            if (videoPlayer.frame >= (long)(videoPlayer.frameCount - 10))
            {
                videoPlayer.Pause();
            }
        }
    }
}
