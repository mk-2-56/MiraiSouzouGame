using UnityEngine;

public class SceneSwitcher : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    void Update()
    {
        // Enter�L�[�������ꂽ�玟�̏�ԂɑJ��
        if (Input.GetKeyDown(KeyCode.Return)) // KeyCode.Return��Enter�L�[
        {
            if (gameManager != null)
            {
                gameManager.GoToNextState();
            }
            else
            {
                Debug.LogError("GameManager���ݒ肳��Ă��܂���");
            }
        }
    }
}