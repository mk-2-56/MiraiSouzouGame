using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleState : IState
{
    public void Enter()
    {
        Debug.Log("Title State: Entered");
    }

    public void Execute()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            GameManager.gameManager.SetState(GameManager.GameState.Game);  // 状態をPlayに切り替える
        }
    }

    public void Exit()
    {
        Debug.Log("Title State: Exited");
    }
}
