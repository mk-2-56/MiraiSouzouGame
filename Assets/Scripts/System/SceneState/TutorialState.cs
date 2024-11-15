using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialState : IState
{
    public void Enter()
    {
        SceneManager.LoadScene("Tutorial");
        Debug.Log("Title State: Entered");
    }

    public void Exit()
    {
        Debug.Log("Title State: Exited");
    }
}
