using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleState : IState
{
    public void Enter()
    {
        SceneManager.LoadScene("Title");
        Debug.Log("Title State: Entered");
    }

    public void Exit()
    {
        Debug.Log("Title State: Exited");
    }
}
