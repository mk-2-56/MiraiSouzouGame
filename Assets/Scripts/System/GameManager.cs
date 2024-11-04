using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //GameManager����Ɉ��
    public static GameManager gameManager {  get; private set; }
    public CameraManager cameraManager;
    public CCT_Basic playerMovement;
    public SplineFollower splineFollower;

    //�Q�[�����
    public enum GameState{ Title, Game, Paused, Result};
    private IState currentState { get; }
    // ���݂̃Q�[����Ԃ�ێ�
    private void Awake()
    {
        if(gameManager == null)
        {
            gameManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }    
    }
    private void Start()
    {
        SetState(GameState.Title);
    }

    public void SetState(GameState newState)
    {
        currentState.Exit();
        currentState.Enter();

        switch (newState)
        {
            case GameState.Title:
                SceneManager.LoadScene("Title");

                Debug.Log("Menu");

                break;
            case GameState.Game:
                SceneManager.LoadScene("Game");

                Debug.Log("Game");

                break;
            case GameState.Paused:
                Time.timeScale = 0.0f;
                break;
            case GameState.Result:
                SceneManager.LoadScene("Result");
                
                Debug.Log("Result!");
                break;
            default:
                break;
        }
    }

    // �Q�[�������X�^�[�g���郁�\�b�h
    public void RestartGame()
    {
        // �Q�[�����v���C��ԂɕύX�i�V�[�����ă��[�h�j
        SetState(GameState.Game);
    }

    public void QuitGame()
    {
        // �Q�[�����I��
        Application.Quit();
    }

}
