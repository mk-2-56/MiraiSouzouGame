using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ukoh-2024-10-28
/// プレイヤー基本移動
/// 
/// 今はほぼ全private、
/// interface含め要望なども勿論、
/// 必要がありましたら気軽に連絡を
/// </summary>

public class CCT_Basic : MonoBehaviour
{
    //Debug:
    MsgBuffer _debugText;
    //_____________Properties

    //_____________Parameters
    [SerializeField] float param_maxSpeed    = 60.0f;
    [SerializeField] float param_maxMomentum = 160.0f;
    [SerializeField] float param_maxAngularAcc = 15.0f;
    [SerializeField] float param_airCoefficient = 0.2f;
    [SerializeField] float param_acc = 10.0f;


    //_____________References
    Rigidbody _rRb;
    Transform _rCamFacing;
    Transform _rCog;
    CCHover _rCCHover;

    //_____________Members
    Quaternion _targetFacing;
    Vector3 _xzPlainVel;
    float _xzSpeed;
    float _momentum;
    float _accCoefficient;

    float _horiInput;
    float _vertInput;

    Vector3 _inputDirection;
    Vector3 _rawInput;
    Vector3 _terrianNormal;
    Quaternion _terrianRotation;

    float _yRotationCog;

    //______________Flags
    bool _nuetralInput;
    bool _boosting;
    bool _drifting;
    bool _driftingOld;
    bool _grounded;

    void Start()
    {
        _rRb = GetComponent<Rigidbody>();
        _rCamFacing = transform.Find("CamFacing");
        _rCog = transform.Find("Cog");
        _rCCHover = GetComponent<CCHover>();

        _debugText = DebugText.GetMsgBuffer();
    }

    void Update()
    {
        InputFetcher();
    }
    private void FixedUpdate()
    {
        _debugText.Text = "";

        _xzPlainVel = new Vector3(_rRb.velocity.x, 0, _rRb.velocity.z);
        _xzSpeed    = _xzPlainVel.magnitude;

        _debugText.Text += "Grounded: " + _grounded.ToString();
        _debugText.Text += " Input: " + _inputDirection.ToString();
        _debugText.Text += "<br>Speed: " + _xzPlainVel.magnitude.ToString("F4");
        _debugText.Text += " Momentum: " + _momentum.ToString("F4");

        _grounded = _rCCHover.Grounded;
        _terrianNormal = _rCCHover.Groundhit.normal;

        _terrianRotation = Quaternion.identity;
        if (_grounded)
        {
            bool terrianClamp = !(Mathf.Abs(Vector3.Dot(_terrianNormal, _rRb.velocity.normalized)) < 0.7f);
            _debugText.Text += "<br>TerrianNormalClamp :" + terrianClamp.ToString();
            if (!terrianClamp)
                _terrianRotation = Quaternion.FromToRotation(Vector3.up, _terrianNormal);
        }

        NewSpeedSystem();
        MomentumSystem();

        Drifting();
        Locomovtive();
        _driftingOld = _drifting;
    }

    void Locomovtive()
    {
        if (_drifting && _grounded)
            return;

        float directionFector = 2.0f - Vector3.Dot(_inputDirection, _xzPlainVel.normalized);

        float accMag = param_acc;
        Vector3 acc  = _inputDirection * accMag;
        Vector3 mAcc = _inputDirection * _momentum - _xzPlainVel.normalized * ( _momentum * ( -directionFector + 2));
            
        mAcc = Vector3.zero;

        if (!_grounded)
        { 
            mAcc = Vector3.zero;
        }
        else
        {
            acc *= directionFector * _accCoefficient;

            Vector3 terrianSlope = new Vector3(_terrianNormal.x, 0, _terrianNormal.z).normalized;
            float slideBuildUp = (1 - Vector3.Dot(_terrianNormal, Vector3.up))
                * Mathf.Max(-1, Vector3.Dot(terrianSlope, _rCog.forward));

            _debugText.Text += "<br>Slide: " + (98f * slideBuildUp).ToString("F4");
            acc += _inputDirection * 98f * slideBuildUp;
        }
        Vector3 appliedAcc = acc + mAcc ;
        appliedAcc = _terrianRotation * appliedAcc;

        _debugText.Text += "<br>Acc: " + acc.ToString();
        _debugText.Text += "<br>MomentumAcc: " + mAcc.ToString();
        _rRb.AddForce(appliedAcc, ForceMode.Acceleration);

        if (_xzPlainVel.sqrMagnitude > 5.0f)
        {
            _rCog.rotation = Quaternion.LookRotation(_xzPlainVel.normalized, Vector3.up);
        }
    }

    void Drifting()
    {
        if (!_grounded )
            return;

        Vector3 acc;
        float directionFector = 2.0f - Vector3.Dot(_rCog.forward, _xzPlainVel.normalized);

        if(_nuetralInput)
        {
            if(_xzPlainVel != Vector3.zero)
                _rRb.AddForce( _terrianRotation * (_xzPlainVel.normalized * -param_acc), ForceMode.Acceleration);
            return;
        }


        if (_driftingOld)
        {
            Vector3 targetVel = _rCog.forward * _xzSpeed * (-directionFector + 2);
            acc = targetVel - _xzPlainVel;
            acc = _terrianRotation * acc;
            _rRb.AddForce(acc, ForceMode.VelocityChange);
        }
        if (!_drifting)
            return;

        Vector3 mAcc = _rCog.forward * _momentum -_xzPlainVel.normalized * (_momentum * (-directionFector + 2));

        _rCog.rotation = Quaternion.RotateTowards(
            _rCog.rotation, Quaternion.LookRotation(_inputDirection, Vector3.up),
            param_maxAngularAcc * Time.fixedDeltaTime);
            
        acc = _rCog.forward * (param_acc + _momentum) * (directionFector - 1);
        Vector3 appliedAcc = acc + mAcc;
        appliedAcc = _terrianRotation * appliedAcc;
        _rRb.AddForce(appliedAcc, ForceMode.Acceleration);
    }


    void Boosting()
    {
        if (_nuetralInput)
            _inputDirection = _xzPlainVel.normalized;
        if (!_boosting)
        { 
            if (_nuetralInput)
                _inputDirection *= -0.2f;
            return;
        }

        _accCoefficient += 1;
    }
    void NewSpeedSystem()
    { 
        float k = 0.01f;
        //sigmoid function for clamping the acceleratioin
        _accCoefficient = Mathf.Min(1.0f, 2.0f / (1 + Mathf.Pow( 2.7f , k * (_xzPlainVel.magnitude - param_maxSpeed))));
        Boosting();
        _debugText.Text += " AccCoefficient: " + _accCoefficient.ToString("F4");
    }

    void MomentumSystem_Old()
    { //buildup when boosting, riding down slopes or with mechanism
        if (!_grounded)
            return;

        float dTime = Time.fixedDeltaTime;
        float currentVel = _rRb.velocity.magnitude;
        float dMomentum  = (param_maxMomentum - _momentum - currentVel);
        float buildUp = dMomentum * 0.2f;

        Vector3 terrianSlope = new Vector3(_terrianNormal.x, 0, _terrianNormal.z).normalized;
        float slideBuildUp = (1 - Vector3.Dot(_terrianNormal, Vector3.up))
            * Mathf.Max( 0, Vector3.Dot(terrianSlope, _rCog.forward));//do max with zero to negate uphill speed decrease
        _momentum += 9.8f * slideBuildUp;
        if (_boosting)
            _momentum += buildUp * dTime;
        else if (_momentum > 0)
        {
            int stopping = 0;
            if(currentVel < param_maxSpeed)
                stopping++;
            _momentum -= _momentum * (0.3f + 3 * stopping) * dTime;
            _momentum = Mathf.Max(0, _momentum);
        }
        _debugText.Text += "Momentum: " + _momentum.ToString("F4");
    }

    void MomentumSystem()
    {
        if (!_grounded)
            return;

        float dTime = Time.fixedDeltaTime;
        float currentVel = _rRb.velocity.magnitude;
        float buildUp = param_acc;

        _momentum = Mathf.Max( 0, _xzSpeed - param_maxSpeed);
        _debugText.Text += "Momentum: " + _momentum.ToString("F4") + " Boosting: " + _boosting.ToString();
    }

    void InputFetcher()
    {
        _horiInput = Input.GetAxisRaw("Horizontal");
        _vertInput = Input.GetAxisRaw("Vertical");
        _nuetralInput = _inputDirection == Vector3.zero;
        _rawInput = new Vector2(_vertInput, _horiInput);
        _inputDirection = Vector3.Normalize(_rCamFacing.forward * _vertInput + _rCamFacing.right * _horiInput);

        //_boosting = Input.GetKey(KeyCode.LeftShift) ;
        _boosting = Input.GetButton("Fire1");
        _drifting = Input.GetButton("Fire2");
    }
}
