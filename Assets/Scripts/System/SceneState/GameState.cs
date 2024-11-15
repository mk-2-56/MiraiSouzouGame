using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : IState
{
    public void Enter()
    {
        SceneManager.LoadScene("Game");
        Debug.Log("Title State: Entered");
    }

    public void Exit()
    {
        Debug.Log("Title State: Exited");
    }
}
