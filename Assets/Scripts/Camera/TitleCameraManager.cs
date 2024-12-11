using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleCameraManager : CameraManager
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public override void Initialized()
    {
        base.Initialized();
    } 
    // Update is called once per frame
    void Update()
    {
        
    }

    public override GameObject SpawnGameCamera(GameObject player)
    {
        return null;
    }
    public override void AdjustGameCamera(int currentPlayerCount)
    {

    }

}
