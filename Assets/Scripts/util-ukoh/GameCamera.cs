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
    [SerializeField] bool  param_locking = false;

    //_____________Members
    Transform _rCog;
    Transform _rCamFacing;

    float m_xRotation = 0.0f;
    float m_yRotation = 0.0f;
    Vector2 m_mouseInput;



    // Start is called before the first frame update
    void Start()
    {
        _rCamFacing = transform.parent;
        _rCog = transform.parent.parent.Find("Cog");

        Cursor.visible   = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if(param_locking)
        {
            transform.rotation = _rCamFacing.rotation = Quaternion.Lerp(_rCamFacing.rotation, _rCog.rotation, param_lerpSpeed * Time.deltaTime);
        }
        else
            CameraControl();
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
        transform.rotation = Quaternion.Euler(m_xRotation, m_yRotation, 0);
    }
}
