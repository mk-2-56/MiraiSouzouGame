using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance; // シングルトンでインスタンスを保持
    private float startTime;
    private float endTime;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // このオブジェクトをシーン間で保持
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartTimer()
    {
        startTime = Time.time; // ゲーム開始時の時間を記録
    }

    public void StopTimer()
    {
        endTime = Time.time; // ゲーム終了時の時間を記録
    }

    public float GetElapsedTime()
    {
        return endTime - startTime; // 経過時間を計算して返す
    }
}