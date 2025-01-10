using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugJump : MonoBehaviour
{
    private GameObject player;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            player = GameObject.FindWithTag("Player");
            Debug.Log("プレイヤー検出");
            player.transform.position = new Vector3(-30602.59f, 1950.877f, 25767.68f);
        }
    }

    private void OnTriggerEnter()
    {
        // player.transform.position = new Vector3(-30602.59f, 1942.877f, 25767.68f);
    }
}