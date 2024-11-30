using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System.Collections;

public class GameState : IState
{

    public void Enter()
    {
        //LoadingScene loading = new LoadingScene();

        //loading.Load("Game");

        SceneManager.LoadScene("Game");
        
        Debug.Log("Title State: Entered");
    }
    



    public void Exit()
    {
        Debug.Log("Title State: Exited");
    }
}
