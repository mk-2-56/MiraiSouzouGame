using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuPanel;
    private bool isPaused = false;

    public void Initialized()
    {
        // �|�[�Y���j���[���\���ɂ��ĊJ�n
        pauseMenuPanel?.SetActive(false);
        isPaused = false;
    }

    void Update()
    {
        // ESC�L�[�Ń|�[�Y�̃I��/�I�t��؂�ւ���
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
        // �Q�[�����ꎞ��~���A�|�[�Y���j���[��\��
        Time.timeScale = 0f;
        isPaused = true;
        pauseMenuPanel.SetActive(true);
    }

    public void ResumeGame()
    {
        if (pauseMenuPanel == null) return;
        // �Q�[�����ĊJ���A�|�[�Y���j���[���\��
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
        // ���X�^�[�g���Ƀ^�C���X�P�[�������Z�b�g
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnReturnToLobbyButtonPressed()
    {
        // ���r�[�ɖ߂�ۂɃ^�C���X�P�[�������Z�b�g
        Time.timeScale = 1f;
        SceneManager.LoadScene("Title"); // �^�C�g���V�[���ɖ߂�
    }
}