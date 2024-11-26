using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuPanel;
    private bool isPaused = false;

    public void Initialized()
    {
        // ポーズメニューを非表示にして開始
        pauseMenuPanel?.SetActive(false);
        isPaused = false;
    }

    void Update()
    {
        // ESCキーでポーズのオン/オフを切り替える
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        if (pauseMenuPanel == null) return;
        // ゲームを一時停止し、ポーズメニューを表示
        Time.timeScale = 0f;
        isPaused = true;
        pauseMenuPanel.SetActive(true);
    }

    public void ResumeGame()
    {
        if (pauseMenuPanel == null) return;
        // ゲームを再開し、ポーズメニューを非表示
        Time.timeScale = 1f;
        isPaused = false;
        pauseMenuPanel.SetActive(false);
    }

    public void OnResumeButtonPressed()
    {
        ResumeGame();
    }

    public void OnRestartButtonPressed()
    {
        // リスタート時にタイムスケールをリセット
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnReturnToLobbyButtonPressed()
    {
        // ロビーに戻る際にタイムスケールをリセット
        Time.timeScale = 1f;
        SceneManager.LoadScene("Title"); // タイトルシーンに戻る
    }
}