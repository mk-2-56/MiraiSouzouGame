using UnityEngine;
using UnityEngine.SceneManagement;

public class TestSceneLoader : MonoBehaviour
{
    void Start()
    {
        // シーンをロードしてみる
        SceneManager.LoadScene("MainMenu");  // "MainMenu"はテスト用のシーン名
    }
}