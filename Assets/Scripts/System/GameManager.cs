using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    public CCT_Basic playerMovement;
    public SplineFollower splineFollower;

    public Camera mainCamera;
    public Camera eventCamera;
 
    private bool isEvent;
    void Start()
    {
        isEvent = false;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
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
    }

    public void EndSplineEvent()
    {
        // �X�v���C���ړ����I�����A�ʏ�ړ����ĊJ
        splineFollower.EndSplineMovement();
        playerMovement.SetMovement(true);
    }
}