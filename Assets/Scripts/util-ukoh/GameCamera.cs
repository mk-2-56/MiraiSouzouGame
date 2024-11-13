using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ukoh-2024-10-28
/// GameCamera  プレイヤーカメラ
/// 
/// シンプルなカメラ制御なだけ
/// todo inertia機能
/// </summary>

public class GameCamera : MonoBehaviour
{
    //_____________Parameters
    [SerializeField] float param_mouseSpeed = 50.0f;
    [SerializeField] float param_lerpSpeed = 0.8f;
    [SerializeField] float param_lerpSpeedPos = 0.95f;
    [SerializeField] bool  param_locking = true;

    //_____________Members
    CCT_Basic _rMovementContoller;
    Transform _rFacing;
    Transform _rCog;
    Transform _rCamFacing;
    Transform _rCamXform;

    string _axisNamePrefix;

    float _xRotation = 0.0f;
    float _yRotation = 0.0f;

    GameObject joint;


    // Start is called before the first frame update
    void Awake()
    {
        _rCamFacing = transform.parent;
        _rFacing = _rCamFacing.parent.Find("Facing");
        _rCog = _rFacing.Find("Cog");
        joint = new GameObject();
        joint.name = "CamOrigin";
        joint.transform.position = transform.position;
        _rCamXform = transform.Find("Main Camera");
        _rCamXform.SetParent(joint.transform);

        _rMovementContoller = _rCamFacing.transform.parent.GetComponent<CCT_Basic>();
        _axisNamePrefix = _rMovementContoller.inputAxisPrefix;

        Cursor.visible   = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
            param_locking = !param_locking;

        if(param_locking)
        { 
            joint.transform.rotation = _rCamFacing.rotation = Quaternion.Lerp(_rCamFacing.rotation, _rFacing.rotation, param_lerpSpeed * Time.deltaTime);
            
        }
        else
            CameraControl();
    }

    private void FixedUpdate()
    {
        joint.transform.position = Vector3.Lerp( joint.transform.position, _rCog.position, param_lerpSpeedPos);
    }

    void CameraControl()
    {
        _axisNamePrefix = _rMovementContoller.inputAxisPrefix;

        //mouse
        float mouseSpeed = param_mouseSpeed * Time.deltaTime;
        float xInput = -Input.GetAxis("P1_" + "LookY");
        float yInput =  Input.GetAxis("P1_" + "LookX");

        _xRotation += xInput * mouseSpeed;
        _yRotation += yInput * mouseSpeed;
        _xRotation = Mathf.Clamp(_xRotation, -70.0f, 85.0f);

        _rCamFacing.rotation  = Quaternion.Euler(0, _yRotation, 0);
        joint.transform.rotation = Quaternion.Euler(_xRotation, _yRotation, 0);
    }
}
