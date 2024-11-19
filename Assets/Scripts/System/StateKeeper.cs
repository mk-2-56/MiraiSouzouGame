using UnityEngine;

public class StateKeeper : MonoBehaviour
{
    // �V�[���Ԃŋ��L��������Ԃ�ێ�
    public GameManager.SceneState CurrentSceneState { get; set; } = GameManager.SceneState.Title;

    // �V�[���Ԃ�StateService����������悤��DontDestroyOnLoad���g�p
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}