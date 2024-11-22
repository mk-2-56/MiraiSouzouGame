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

    // �Q�[����Ԃ�\��Enum
    public enum SceneState { Title, Tutorial, Game, Result };
    private IState currentState;
    private StateKeeper stateKeeper; // ���݂�SceneState��ێ����邽�߂̕ϐ�

    void Awake()
    {
        stateKeeper = FindObjectOfType<StateKeeper>();
        if (stateKeeper == null)
        {
            UnityEngine.Debug.LogError("stateKeeper ��������܂���B");
        }
        else
        {
            // ������Ԃ�ݒ�
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
        UnityEngine.Debug.Log("�����_�̃V�[���́F" + SceneManager.GetActiveScene().name);

        UnityEngine.Debug.Log(newState.ToString());

        // ���݂̃V�[���ƐV�����V�[�����قȂ�ꍇ�̂݃V�[�������[�h
        if (stateKeeper.CurrentSceneState != newState)
        {
            // ���݂̃X�e�[�g���I��
            currentState?.Exit();
            // �V�����X�e�[�g��ݒ�
            currentState = CreateState(newState);
            stateKeeper.CurrentSceneState = newState;
            currentState?.Enter();
        }
       
    }

    public void SetStateDontLoad(SceneState newState)
    {
        UnityEngine.Debug.Log("�����_�̃V�[���́F" + SceneManager.GetActiveScene().name);

        UnityEngine.Debug.Log(newState.ToString());

        // ���݂̃V�[���ƐV�����V�[�����قȂ�ꍇ�̂݃V�[�������[�h
        if (stateKeeper.CurrentSceneState != newState)
        {
            // ���݂̃X�e�[�g���I��
            currentState?.Exit();
            // �V�����X�e�[�g��ݒ�
            currentState = CreateState(newState);

            stateKeeper.CurrentSceneState = newState;
            // Null�`�F�b�N��ǉ����ăG���[���
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
                return new GameState(); // �N���X����GamePlayState�ɕύX
            case SceneState.Result:
                return new ResultState();
            default:
                return null;
        }
    }

    // ���̏�ԂɑJ�ڂ��郁�\�b�h
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

    // �Q�[�������X�^�[�g
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // ���݂̃V�[���������[�h

        SetState(SceneState.Game);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}