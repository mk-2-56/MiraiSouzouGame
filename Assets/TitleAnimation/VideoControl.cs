using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoControl : MonoBehaviour
{
    [SerializeField]private VideoPlayer videoPlayer; // VideoPlayerをアタッチ
    public Material material;

    private void Start()
    {
        // VideoPlayerがアタッチされていなければ取得
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }
        videoPlayer.loopPointReached += OnVideoLoopPointReached;
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

    void OnVideoLoopPointReached(VideoPlayer vp)
    {
        // 動画終了時にアルファをリセット（例）
        material.SetFloat("_Alpha", 1.0f);
    }

}
