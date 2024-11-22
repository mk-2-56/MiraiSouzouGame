using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private AudioManager   audioManager;
    [SerializeField] private UIManager      uiManager;
    [SerializeField] private PauseManager   pauseManager;
    [SerializeField] private AU.PlayerManager  playerManager;
    [SerializeField] private CameraManager cameraManager;

    // ゲーム状態を表すEnum
    public enum SceneState { Title, Tutorial, Game, Result };
    private IState currentState;
    private StateKeeper stateKeeper; // 現在のSceneStateを保持するための変数

    void Awake()
    {
        stateKeeper = FindObjectOfType<StateKeeper>();
        if (stateKeeper == null)
        {
            UnityEngine.Debug.LogError("stateKeeper が見つかりません。");
        }
        else
        {
            // 初期状態を設定
            SetState(stateKeeper.CurrentSceneState);
        }
    }
    void Start()
    {
        Initialized();
    }

    public void Initialized()
    {
        cameraManager?.Initialized();
        audioManager?.Initialized();
        uiManager?.Initialized();
        pauseManager?.Initialized();
        playerManager?.Initialized();

        UnityEngine.Debug.Log("GameManager initialized for scene: " + SceneManager.GetActiveScene().name);
    }

    public void SetState(SceneState newState)
    {
        UnityEngine.Debug.Log("現時点のシーンは：" + SceneManager.GetActiveScene().name);

        UnityEngine.Debug.Log(newState.ToString());

        // 現在のシーンと新しいシーンが異なる場合のみシーンをロード
        if (stateKeeper.CurrentSceneState != newState)
        {
            // 現在のステートを終了
            currentState?.Exit();
            // 新しいステートを設定
            currentState = CreateState(newState);
            stateKeeper.CurrentSceneState = newState;
            currentState?.Enter();
        }
       
    }

    public void SetStateDontLoad(SceneState newState)
    {
        UnityEngine.Debug.Log("現時点のシーンは：" + SceneManager.GetActiveScene().name);

        UnityEngine.Debug.Log(newState.ToString());

        // 現在のシーンと新しいシーンが異なる場合のみシーンをロード
        if (stateKeeper.CurrentSceneState != newState)
        {
            // 現在のステートを終了
            currentState?.Exit();
            // 新しいステートを設定
            currentState = CreateState(newState);

            stateKeeper.CurrentSceneState = newState;
            // Nullチェックを追加してエラー回避
            currentState?.Enter();
        }

    }

    private IState CreateState(SceneState newState)
    {
        switch (newState)
        {
            case SceneState.Title:
                return new TitleState();
            case SceneState.Tutorial:
                return new TutorialState();
            case SceneState.Game:
                return new GameState(); // クラス名をGamePlayStateに変更
            case SceneState.Result:
                return new ResultState();
            default:
                return null;
        }
    }

    // 次の状態に遷移するメソッド
    public void GoToNextState()
    {
        switch (stateKeeper.CurrentSceneState)
        {
            case SceneState.Title:
                SetState(SceneState.Tutorial);
                break;
            case SceneState.Tutorial:
                SetState(SceneState.Game);
                break;
            case SceneState.Game:
                SetState(SceneState.Result);
                break;
            case SceneState.Result:
                SetState(SceneState.Title);
                break;
        }
    }

    // ゲームをリスタート
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // 現在のシーンをリロード

        SetState(SceneState.Game);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}