using AU;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneManager : BaseSceneManager
{
    [SerializeField] private GameManager gameManager;
    private PlayerManager playerManager;
    // Start is called before the first frame update
    public override void Initialized()
    {
        if (gameManager == null) return;
        playerManager = gameManager.GetComponent<PlayerManager>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
