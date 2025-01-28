using AU;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

public class GoalChecker : MonoBehaviour
{
    public static GoalChecker Instance { get; private set; }

    Dictionary<int , float> TimeRanking = new Dictionary<int , float>();
    private Color WinnerColor = Color.white;
    private PlayerManager playerManager;
    private SceneLoader sceneLoader;
    private int goalCount;


    public void Initialized()
    {
        goalCount = 0;
    }
    public Color GetWinnerColor()
    {
        return WinnerColor;
    }
    public int GetWinner()
    {
        int winner = 1;
        foreach (KeyValuePair<int, float> pt in TimeRanking)
        {
            if (pt.Key != winner && pt.Value < TimeRanking[winner])
            {
                winner = pt.Key;
            }
        }
        return winner;
    }

    public float GetWinnerTime()
    {
        int winnerKey = GetWinner();
        return TimeRanking[winnerKey];
    }

    public int GetLoser()
    {
        foreach (KeyValuePair<int, float> pt in TimeRanking)
        {
            if (pt.Key != GetWinner())
            {
                return pt.Key;
            }
        }
        return 0;
    }

    public float GetLoserTime()
    {
        return TimeRanking[GetLoser()];
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance.gameObject); // 古いインスタンスを削除
        }

        Instance = this; // 新しいインスタンスを登録
        DontDestroyOnLoad(gameObject); // シーン間で保持

    }
    private void Start()
    {
        playerManager = FindObjectOfType<PlayerManager>();
        sceneLoader  = FindObjectOfType<SceneLoader>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerCanvasController controller = other.GetComponentInChildren<PlayerCanvasController>();
            controller.ShowFinish();

            playerManager.PlayerGoal(other.gameObject, false);
            goalCount++;
            TimeManager.Instance.StopTimer();
            TimeRanking.Add(playerManager.GetPlayerId(other.gameObject), TimeManager.Instance.GetElapsedTime());
            if(TimeRanking.Count == 1)
            {
                GameObject model = other.gameObject.transform.Find("Facing/Cog/AnimationController/Character_V2").gameObject;
                WinnerColor = model.transform.Find("Raincoat").GetComponent<SkinnedMeshRenderer>().material.GetColor("_BASE_COLOR");
            }
            if (goalCount >= playerManager.GetPlayerCount())
            {   // 全員ゴールした
                StartCoroutine(WaitAndSetGameEnd(2.5f)); // 余韻
            }
        }
    }

    private IEnumerator WaitAndSetGameEnd(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        sceneLoader.SetGameEnd(true);
    }


}
