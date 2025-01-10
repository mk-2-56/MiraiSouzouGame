using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultState : IState
{
    public void Enter()
    {
        SceneManager.LoadScene("Result");
        Debug.Log("Title State: Entered");
    }

    public void Exit()
    {
        Debug.Log("Title State: Exited");
    }
}
