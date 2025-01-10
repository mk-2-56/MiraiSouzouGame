using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ResultPlayerManager : MonoBehaviour
{

    [SerializeField] List<Color> p_playerColors = new();
    [SerializeField] private GameObject player;

    public int winPlayer;

    void Start()
    {
        Color pcColor = p_playerColors[winPlayer];

        GameObject model = player.transform.Find("Character_V2").gameObject;
        model.transform.Find("Raincoat").GetComponent<SkinnedMeshRenderer>().material.SetColor("_BASE_COLOR", pcColor);
        model.transform.Find("L_boots").GetComponent<SkinnedMeshRenderer>().material.SetColor("_BASE_COLOR", pcColor);
        model.transform.Find("R_boots").GetComponent<SkinnedMeshRenderer>().material.SetColor("_BASE_COLOR", pcColor);
    }
}