using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugJump : MonoBehaviour
{
    private GameObject player;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    private void OnTriggerEnter()
    {
        // player.transform.position = ;
    }
}