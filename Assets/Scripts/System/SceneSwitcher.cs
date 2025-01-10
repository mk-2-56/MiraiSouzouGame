using UnityEngine;

public class SceneSwitcher : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    void Update()
    {
        // Enterキーが押されたら次の状態に遷移
        if (Input.GetKeyDown(KeyCode.Return)) // KeyCode.ReturnはEnterキー
        {
            if (gameManager != null)
            {
                gameManager.GoToNextState();
            }
            else
            {
                Debug.LogError("GameManagerが設定されていません");
            }
        }
    }
}