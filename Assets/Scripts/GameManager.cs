using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //GameManager‚ğí‚Éˆê‚Â
    public static GameManager Instance {  get; private set; }

    //ƒQ[ƒ€ó‘Ô
    public enum GameState
    {
        MainMenu,
        Playing,
        Paused,
        GameOver,
    }
    // Œ»İ‚ÌƒQ[ƒ€ó‘Ô‚ğ•Û
    public GameState state { get; private set; }
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            //‘¶İ‚µ‚Ä‚¢‚é‚È‚ç”jŠü‚·‚é
            Destroy(gameObject);
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
