using UnityEngine;

public class StateKeeper : MonoBehaviour
{
    // シーン間で共有したい状態を保持
    public GameManager.SceneState CurrentSceneState { get; set; } = GameManager.SceneState.Title;

    // シーン間でStateServiceが持続するようにDontDestroyOnLoadを使用
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}