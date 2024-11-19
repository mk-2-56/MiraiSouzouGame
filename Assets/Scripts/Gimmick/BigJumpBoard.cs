using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigJumpBoard : MonoBehaviour
{
    // Start is called before the first frame update

    public SplineFollower splineFollower;

    private bool isUse;
    //Collider m_ObjectCollider;
    void Start()
    {
        //m_ObjectCollider = GetComponent<Collider>();
        isUse = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isUse)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("BigJump!");
                splineFollower.StartSplineMovement(other.transform);
                //other.gameObject.GetComponent<Rigidbody>().AddForce(other.transform.up * jumpBoardForce, ForceMode.Impulse);
                /*            Debug.Log(other.transform.up.ToString() + jumpBoardForce.ToString());
                */
                //isCollision = true;
            }
        }
       
    }
}
