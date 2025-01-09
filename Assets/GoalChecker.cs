using AU;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

public class GoalChecker : MonoBehaviour
{
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private SceneLoader sceneLoader;
    private int goalCount;

    public void Initialized()
    {
        goalCount = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerCanvasController controller = other.GetComponentInChildren<PlayerCanvasController>();
            controller.ShowFinish();

            playerManager.SetPlayerControl(other.gameObject, false);
            goalCount++;

            if(goalCount >= playerManager.GetPlayerCount())
            {   // ëSàıÉSÅ[ÉãÇµÇΩ
                StartCoroutine(WaitAndSetGameEnd(2.5f)); // ó]âC
            }
        }
    }

    private IEnumerator WaitAndSetGameEnd(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        sceneLoader.SetGameEnd(true);
    }
}
