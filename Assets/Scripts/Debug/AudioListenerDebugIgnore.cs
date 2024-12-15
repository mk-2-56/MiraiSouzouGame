using UnityEngine;

public class LogFilter : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        // 指定したメッセージを無視する
        if (logString.Contains("There are 2 audio listeners in the scene"))
        {
            return; // このログは無視
        }
        // 他のログは通常通り処理
        Debug.unityLogger.Log(type, logString);
    }
}