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
    public void SetPlayerReference(GameObject pc)
    {
        _rRb = pc.GetComponent<Rigidbody>();

        _rFacing    = pc.transform.Find("Facing");
        _rCog       = _rFacing.Find("Cog");
        _rCamFacing = pc.transform.Find("CamFacing");

        _rCChub     = pc.GetComponent<CC.Hub>();
        _rCChub.LookEvent += Look;

        transform.SetParent(null);
    }

    //_____________Parameters
    [SerializeField] float param_lerpSpeed  = 0.8f;
    [SerializeField] float param_lerpSpeedPos = 0.95f;
    [SerializeField] float param_lerpSpeedPivot = 0.95f;
    [SerializeField] bool  param_locking = true;
    [SerializeField] Vector2 param_pivotAngle = new Vector2(30, 45);

    [SerializeField] float param_springForce = 10.0f;
    [SerializeField] float param_damperForce = 2.0f;

    //_____________Members
    CC.Hub      _rCChub;

    Rigidbody   _rRb;
    Transform   _rFacing;
    Transform   _rCog;
    Transform   _rCamFacing;

    Rigidbody   _rCamera;
    Vector3 _camOffset;
    Vector3 _camOffsetOld;

    Quaternion _baseRotation;

    Vector2 _input = Vector2.zero;
    Vector2 _camRotation = Vector2.zero;

    Vector2 _camPivot      = Vector2.zero;
    Vector2 _camPivotInput = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        if(transform.parent)
        {
            SetPlayerReference(transform.parent.gameObject);
        }

        Cursor.visible   = false;
        Cursor.lockState = CursorLockMode.Locked;
        transform.parent = null;
        _baseRotation    = _rFacing.rotation;
        _rCamera   = transform.Find("Camera").GetComponent<Rigidbody>();
        _camOffset = _rCamera.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
            param_locking = !param_locking;
    }

    private void FixedUpdate()
    {
        float dtime = Time.fixedDeltaTime;
        transform.position = Vector3.Lerp( transform.position, _rCog.position, param_lerpSpeedPos * dtime);
        { 
            Vector3 springForce = transform.rotation * (param_springForce * (_camOffset - _rCamera.transform.localPosition))
                - _rCamera.velocity * param_damperForce;
            _rCamera.AddForce(springForce, ForceMode.Acceleration);
        }
        CamControl();

        if (param_locking)
        {
            _camPivot = Vector2.Lerp( _camPivot, _camPivotInput ,param_lerpSpeedPivot * dtime);

            Quaternion tarRot;
            Vector3 dv = _rFacing.forward;
            dv.y = 2 * _rRb.velocity.y * dtime;
            tarRot = Quaternion.LookRotation(dv);
            _baseRotation = Quaternion.Lerp(_baseRotation, tarRot, param_lerpSpeed * dtime);
            transform.rotation = _baseRotation * Quaternion.Euler(-_camPivot.y, _camPivot.x,  0);

            _rCamera.rotation = Quaternion.LookRotation(_rCog.position - _rCamera.position - new Vector3(0, - 0.5f * _camOffset.y, 0), Vector3.up);

            Vector3 tmp = transform.rotation.eulerAngles;
            _rCamFacing.rotation = Quaternion.Euler(0, tmp.y, 0);
            _camRotation = new Vector2(tmp.y, -tmp.x);
            AU.Debug.Log(_camRotation, AU.LogTiming.Fixed);
        }
    }

    void CamControl()
    {
        if (param_locking)
            return;

        _camRotation += _input;
        _camRotation.y = Mathf.Clamp(_camRotation.y, -70.0f, 85.0f);

        _rCamFacing.rotation = Quaternion.Euler(0, _camRotation.x, 0);
        transform.rotation   = Quaternion.Euler(-_camRotation.y, _camRotation.x, 0);
    }


    void Look(Vector2 input)
    {
        _camPivotInput = _input = input.normalized;
        _camPivotInput *= param_pivotAngle;
    }
}
