using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{

    [SerializeField] private GameObject pauseMenuPanel;
    private bool isPaused = false;

    public static PauseManager Instance { get; private set; } 
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Initialized()
    {
        if (PauseManager.Instance == null) return;
        if (pauseMenuPanel == null) return;

        // �|�[�Y���j���[���\���ɂ��ĊJ�n
        pauseMenuPanel?.SetActive(false);
        isPaused = false;
    }

    void Update()
    {
        // ESC�L�[�Ń|�[�Y�̃I��/�I�t��؂�ւ���
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SoundManager.Instance.PlaySE(SESoundData.SE.SE_Select);
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

        if (Input.GetKeyDown(KeyCode.Return))
        {
            SoundManager.Instance.PlaySE(SESoundData.SE.SE_Click);
        }
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
        {
            SoundManager.Instance.PlaySE(SESoundData.SE.SE_Select);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SoundManager.Instance.PlaySE(SESoundData.SE.SE_Cancel);
        }

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