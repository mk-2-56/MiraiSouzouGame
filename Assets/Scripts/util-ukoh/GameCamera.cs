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
    public Vector3 CameraDirection
    {
        get { return joint.transform.forward;}
    }

    //_____________Parameters
    [SerializeField] float param_mouseSpeed = 50.0f;
    [SerializeField] float param_lerpSpeed = 0.8f;
    [SerializeField] float param_lerpSpeedPos = 0.95f;
    [SerializeField] bool  param_locking = true;

    //_____________Members
    Transform _rFacing;
    Transform _rCog;
    Transform _rCamFacing;

    float m_xRotation = 0.0f;
    float m_yRotation = 0.0f;
    Vector2 m_mouseInput;

    GameObject joint;


    // Start is called before the first frame update
    void Start()
    {
        _rCamFacing = transform.parent;
        _rFacing = _rCamFacing.parent.Find("Facing");
        _rCog = _rFacing.Find("Cog");
        joint = new GameObject();
        joint.transform.position = transform.position;
        transform.Find("Main Camera").SetParent(joint.transform);

        Cursor.visible   = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
            param_locking = !param_locking;

        if(param_locking)
            joint.transform.rotation = _rCamFacing.rotation = Quaternion.Lerp(_rCamFacing.rotation, _rFacing.rotation, param_lerpSpeed * Time.deltaTime);
        else
            CameraControl();
    }

    private void FixedUpdate()
    {
        joint.transform.position = Vector3.Lerp( joint.transform.position, _rCog.position, param_lerpSpeedPos);
    }

    void CameraControl()
    {
        //mouse
        float mouseSpeed = param_mouseSpeed * Time.deltaTime;
        float xInput = -Input.GetAxis("Mouse Y");
        float yInput =  Input.GetAxis("Mouse X");

        m_xRotation += xInput * mouseSpeed;
        m_yRotation += yInput * mouseSpeed;
        m_xRotation = Mathf.Clamp(m_xRotation, -70.0f, 85.0f);

        _rCamFacing.rotation  = Quaternion.Euler(0, m_yRotation, 0);
        joint.transform.rotation = Quaternion.Euler(m_xRotation, m_yRotation, 0);
    }
}
