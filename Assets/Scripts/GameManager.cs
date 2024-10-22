using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    //GameManagerを常に一つ
    public static GameManager gameManager {  get; private set; }

    //ゲーム状態
    public enum GameState
    {
        MainMenu,
        Playing,
        Dressing,
        Paused,
        Result,
    }
    // 現在のゲーム状態を保持
    public GameState CurrentState { get; private set; }
    private void Awake()
    {
        if(gameManager == null)
        {
            gameManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            //存在しているなら破棄する
            Destroy(gameObject);
        }    
    }
    private void Start()
    {
        
    }

    public void SetState(GameState newState)
    {
        CurrentState = newState;

        switch (newState)
        {

            case GameState.MainMenu:
                SceneManager.LoadScene("Menu");

                Debug.Log("Menu");

                break;
            case GameState.Playing:
                SceneManager.LoadScene("Game");

                Debug.Log("Game");

                break;
            case GameState.Dressing:
                SceneManager.LoadScene("Dress");

                Debug.Log("Dressing!");

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

    // ゲームをリスタートするメソッド
    public void RestartGame()
    {
        // ゲームをプレイ状態に変更（シーンを再ロード）
        SetState(GameState.Playing);
    }

    public void QuitGame()
    {
        // ゲームを終了
        Application.Quit();
    }

}
