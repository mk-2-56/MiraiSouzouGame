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
        // �ʏ�ړ��𖳌��ɂ��A�X�v���C���ړ����J�n
        playerMovement.SetMovement(false);
        splineFollower.StartSplineMovement();
        cameraManager.SetEventCamera();

    }

    public void EndSplineEvent()
    {
        // �X�v���C���ړ����I�����A�ʏ�ړ����ĊJ
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