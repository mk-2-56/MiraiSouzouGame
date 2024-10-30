using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    GameObject charaObj;
    public Vector3 cameraVec;

    void Start()
    {
        cameraVec = new Vector3(500.0f, 6.0f, 500.0f);
        charaObj = GameObject.Find("TestPlayerCapsule");
    }

    // Update is called once per frame
    void Update()
    {
        cameraVec.x = charaObj.transform.position.x;
        //cameraVec.x = charaObj.transform.position.y + 2.0f;
        //cameraVec.z = charaObj.transform.position.z;
        //transform.position = new Vector3(0.0f, 0.0f, -3.0f);
        transform.position = cameraVec;

        transform.LookAt(charaObj.transform.position);
    }
}
