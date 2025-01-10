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
        // �w�肵�����b�Z�[�W�𖳎�����
        if (logString.Contains("There are 2 audio listeners in the scene"))
        {
            return; // ���̃��O�͖���
        }
        // ���̃��O�͒ʏ�ʂ菈��
        Debug.unityLogger.Log(type, logString);
    }
}