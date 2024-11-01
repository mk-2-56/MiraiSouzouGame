using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    public CCT_Basic playerMovement;
    public SplineFollower splineFollower;
    public CameraManager cameraManager;

    private bool isEvent;
    void Start()
    {
        cameraManager.SetMainCamera();
        isEvent = false;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            isEvent = !isEvent;
            Debug.Log("Spline = " + isEvent);

            if (isEvent)
            {
                StartSplineEvent();
            }
            else
            {
                EndSplineEvent();
            }
        }


    }

    public void StartSplineEvent()
    {
        // 通常移動を無効にし、スプライン移動を開始
        playerMovement.SetMovement(false);
        splineFollower.StartSplineMovement();
        cameraManager.SetEventCamera();

    }

    public void EndSplineEvent()
    {
        // スプライン移動を終了し、通常移動を再開
        splineFollower.EndSplineMovement();
        playerMovement.SetMovement(true);
        cameraManager.SetMainCamera();
    }

    public bool IsEvent()
    {
        return isEvent;
    }

    public void SetEvent(bool isevent){
        isEvent = isevent;
    }
}