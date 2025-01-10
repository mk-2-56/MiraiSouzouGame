using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigJumpBoard : MonoBehaviour
{
    public SplineFollower splineFollower;
    private bool isUse;
    void Start()
    {
        //m_ObjectCollider = GetComponent<Collider>();
        isUse = false;
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isUse && other.CompareTag("Player"))
        {
            Debug.Log("BigJump!");
            splineFollower.StartSplineMovement(other.transform);
            isUse = true;    
        }
       
    }

    private void OnTriggerExit(Collider other)
    {
        isUse = false;
    }
}
