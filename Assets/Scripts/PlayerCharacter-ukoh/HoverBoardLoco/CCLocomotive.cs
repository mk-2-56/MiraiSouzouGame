using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ukoh-2024-10-28
/// プレイヤー基本移動旧バージョン
/// 
/// 滑る機能が新バージョンとの相違点、
/// 一時保存
/// </summary>
/// 
public class CCLocomotive : MonoBehaviour
{
    //Debug:
    MsgBuffer _debugText;

    //_____________Properties

    //_____________Parameters
    [SerializeField] float param_maxSpeed = 20.0f;
    [SerializeField] float param_maxAngularSpeed = 10.0f;
    [SerializeField] float param_acc = 10.0f;


    //_____________References
    Rigidbody _rRb;
    Transform _rCamFacing;
    Transform _rCog;
    CCHover _rCCHover;

    //_____________Members
    Quaternion _targetFacing;

    float _horiInput;
    float _vertInput;

    Vector3 _inputDirection;

    bool _boosting;




    private void Start()
    {
        _rRb = GetComponent<Rigidbody>();
        _rCamFacing = transform.Find("CamFacing");
        _rCog = transform.Find("Cog");
        _rCCHover = GetComponent<CCHover>();

        _debugText = DebugText.GetMsgBuffer();
    }

    private void Update()
    {
        DirectionInput();
    }

    private void FixedUpdate()
    {
        LocoMovement();
    }

    void DirectionInput()
    {
        _horiInput = Input.GetAxisRaw("Horizontal");
        _vertInput = Input.GetAxisRaw("Vertical");
        _inputDirection = Vector3.Normalize(_rCamFacing.forward * _vertInput + _rCamFacing.right * _horiInput);

        _boosting = Input.GetKey(KeyCode.LeftShift);
    }


    void LocoMovement()
    {
        //Vector3 targetVel = (param_maxSpeed + _momentum) * _inputDirection;
        //Vector3 idealAcc = (targetVel - new Vector3(_rRb.velocity.x, 0, _rRb.velocity.z)) / Time.fixedDeltaTime;
        //float maxAccMag = param_acc * directionFector;
        //Vector3 appliedAcc = Vector3.ClampMagnitude(idealAcc, maxAccMag);



        Vector3 moveForce = _inputDirection * param_acc;
        float angularAcc  = param_maxAngularSpeed;
        float speedLimit  = param_maxSpeed;
        if(_boosting)
            speedLimit *= 2;


        float horiSpeed = new Vector2(_rRb.velocity.x, _rRb.velocity.z).magnitude;
        bool damping  = horiSpeed > speedLimit;
        _targetFacing = _rCamFacing.rotation;

        _debugText.Text = "Speed:" + horiSpeed.ToString("F4") + " SpeedLimit:" + speedLimit.ToString("F4");

        {
            const float k = 10.0f;
            float a = Mathf.Max(0, (horiSpeed - speedLimit));
            float damp = (k / (a + k));

            if (damping)
            {
                //angularAcc *= 1/2;
                float limiter = Vector3.Dot(_inputDirection, new Vector3(_rRb.velocity.x, 0, _rRb.velocity.z).normalized);
                moveForce = moveForce * damp * limiter + moveForce * (1 - limiter);
            }
            _debugText.Text += "<br>MoveForce:" + moveForce.magnitude.ToString("F4") + " Damping:" + damping.ToString() + " Coeffecient:" + damp.ToString("F4");
        }

        //Vector3 forceDir = _inputDirection;
        Vector3 slideDir = _rCog.forward;
        float slideForceMag = 0.0f;

        bool grounded = _rCCHover;
        Vector3 terrianNormal = _rCCHover.Groundhit.normal;
        if (damping && _inputDirection.sqrMagnitude > 0.1f)
        {
            //_targetFacing = Quaternion.FromToRotation(Vector3.forward, _inputDirection);
            _targetFacing = Quaternion.LookRotation(_inputDirection, Vector3.up);
        }
        _rCog.rotation = Quaternion.RotateTowards(_rCog.rotation, _targetFacing, angularAcc);

        if (grounded)
        {
            Vector3 terrianSlope = new Vector3(terrianNormal.x, 0, terrianNormal.z).normalized;
            if((damping || Mathf.Abs(_vertInput) > 0 || Mathf.Abs(_horiInput) > 0))
                slideForceMag = 98f * (1 - Vector3.Dot(terrianNormal, Vector3.up)) * Vector3.Dot(terrianSlope, _rCog.forward);
            Quaternion terrianRotation = Quaternion.FromToRotation(Vector3.up, terrianNormal);
            moveForce = terrianRotation * moveForce;
            //forceDir = terrianRotation * forceDir;
            slideDir = terrianRotation * slideDir;

            _debugText.Text += "<br>slideForceMag:" + slideForceMag.ToString("F4");

            _rRb.AddForce(-terrianNormal * Mathf.Max(0, Vector3.Dot(_rRb.velocity, terrianNormal)),
                ForceMode.Acceleration);
        }
        _rRb.AddForce(moveForce + slideForceMag * slideDir, ForceMode.Acceleration);
    }
}
