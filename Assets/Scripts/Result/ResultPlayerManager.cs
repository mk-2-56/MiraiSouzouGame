using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ResultPlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private ResultImageChanger _imageChanger;

    private Color winnerColor;

    void Start()
    {
        //
        if (GoalChecker.Instance)
        {
            //êF
            winnerColor = GoalChecker.Instance.GetWinnerColor();

        }

        GameObject model = _player.transform.Find("Character_V2").gameObject;
        model.transform.Find("Raincoat").GetComponent<SkinnedMeshRenderer>().material.SetColor("_BASE_COLOR", winnerColor);
        model.transform.Find("L_boots").GetComponent<SkinnedMeshRenderer>().material.SetColor("_BASE_COLOR", winnerColor);
        model.transform.Find("R_boots").GetComponent<SkinnedMeshRenderer>().material.SetColor("_BASE_COLOR", winnerColor);
    }
}