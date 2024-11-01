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

public abstract class Command
{
    public abstract void Execute();
};

public class CCT_Basic : MonoBehaviour
{
    public float Speed
    {
        get{ return _xzSpeed;}
    }

    public Vector3 InputDirectionWorld
    { 
        get{
            if(_nuetralInput)
                return _rFacing.forward;
            return _inputDirection;
        }
    }

    public bool UseGravity
    { 
        get{ return _rRb.useGravity;}
        set{ _rRb.useGravity = value;}
    }

    public void ResigterLaunchModule(Command cmd)
    { 
        _launchCommand = cmd;
    }

    enum State
    { 
        Loco,
        Boosting,
        Drifting,
        Launching,
    }

    //Debug:
    MsgBuffer _debugText;
    //_____________Properties

    //_____________Parameters
    [SerializeField] float param_maxSpeed    = 60.0f;
    [SerializeField] float param_maxMomentum = 160.0f;
    [SerializeField] float param_maxAngularAcc = 15.0f;
    [SerializeField] float param_jumpForce = 30.0f;
    [SerializeField] float param_acc = 10.0f;
    [SerializeField] float param_torqueCoefficient = 0.03f;


    //_____________References
    Rigidbody _rRb;
    Transform _rCamFacing;
    Transform _rCog;
    Transform _rFacing;
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

    Command _launchCommand;

    //______________Flags
    State _PCstate;

    bool _grounded;
    bool _nuetralInput;

    bool _boosting;
    bool _drifting;
    bool _driftingOld;
    bool _launching;
    bool _jumping;

    void Start()
    {
        _rRb = GetComponent<Rigidbody>();
        _rCamFacing = transform.Find("CamFacing");
        _rFacing = transform.Find("Facing");
        _rCog = _rFacing.Find("Cog");
        _rCCHover = GetComponent<CCHover>();

        _debugText = AU.Debug.GetMsgBuffer();
    }

    void Update()
    {
        InputFetcher();
    }
    private void FixedUpdate()
    {
        //_debugText.FixedText = "";

        _xzPlainVel = new Vector3(_rRb.velocity.x, 0, _rRb.velocity.z);
        _xzSpeed    = _xzPlainVel.magnitude;

        BasicDebugInfo();
        TerrianCheck();

        NewSpeedSystem();
        MomentumSystem();

        if(_jumping)
            Jumping();

        if(_launching) 
            _launchCommand.Execute();


        Drifting();
        Locomovtive();
        _driftingOld = _drifting;
    }

    void BasicDebugInfo()
    {
        _debugText.FixedText += "Grounded: " + _grounded.ToString();
        _debugText.FixedText += " Input: " + _inputDirection.ToString();
        _debugText.FixedText += "<br>Speed: " + _xzPlainVel.magnitude.ToString("F4");
        _debugText.FixedText += " Momentum: " + _momentum.ToString("F4");

        _debugText.FixedText += "\tGravity: " + _rRb.useGravity.ToString();
    }

    void TerrianCheck()
    {
        _grounded = _rCCHover.Grounded;
        _terrianNormal = _rCCHover.Groundhit.normal;
        _terrianRotation = Quaternion.identity;
        if (_grounded)
        {
            bool terrianClamp = !(Mathf.Abs(Vector3.Dot(_terrianNormal, _rRb.velocity.normalized)) < 0.5f);
            _debugText.FixedText += "<br>TerrianNormalClamp :" + terrianClamp.ToString();
            if (!terrianClamp)
                _terrianRotation = Quaternion.FromToRotation(Vector3.up, _terrianNormal);
            _rCog.position = _rCCHover.Groundhit.point;
            _rCog.rotation = _terrianRotation * _rFacing.rotation;
        }
        else if(_rRb.useGravity)
            _rRb.AddForce(Vector3.down * 9.8f * 3, ForceMode.Acceleration);
    }


    void Locomovtive()
    {
        if (_drifting && _grounded)
            return;

        float directionFector = 3.0f - 2 * Vector3.Dot(_inputDirection, _xzPlainVel.normalized);

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
                * Mathf.Max(-1, Vector3.Dot(terrianSlope, _rFacing.forward));

            _debugText.FixedText += "<br>Slide: " + (98f * slideBuildUp).ToString("F4");
            acc += _inputDirection * 98f * slideBuildUp;
        }
        Vector3 appliedAcc = acc + mAcc ;
        appliedAcc = _terrianRotation * appliedAcc;

        _debugText.FixedText += "<br>Acc: " + acc.ToString();
        _debugText.FixedText += "<br>MomentumAcc: " + mAcc.ToString();
        _rRb.AddForce(appliedAcc, ForceMode.Acceleration);

        if (_xzPlainVel.sqrMagnitude > 5.0f)
        {
            _rFacing.rotation = Quaternion.LookRotation(_xzPlainVel.normalized, Vector3.up);
        }
    }

    void Drifting()
    {
        if (!_grounded )
            return;

        Vector3 acc;
        float directionFector = 2.0f - Vector3.Dot(_rFacing.forward, _xzPlainVel.normalized);

        if(_nuetralInput)
        {
            if(_xzPlainVel != Vector3.zero)
                _rRb.AddForce( _terrianRotation * (_xzPlainVel.normalized * -param_acc), ForceMode.Acceleration);
            return;
        }


        if (_driftingOld)
        {
            Vector3 targetVel = _rFacing.forward * _xzSpeed * (-directionFector + 2);
            acc = targetVel - _xzPlainVel;
            acc = _terrianRotation * acc;
            _rRb.AddForce(acc, ForceMode.VelocityChange);
        }
        if (!_drifting)
            return;

        Vector3 mAcc = _rFacing.forward * _momentum -_xzPlainVel.normalized * (_momentum * (-directionFector + 2));

        _rFacing.rotation = Quaternion.RotateTowards(
            _rFacing.rotation, Quaternion.LookRotation(_inputDirection, Vector3.up),
            param_maxAngularAcc * Time.fixedDeltaTime);
            
        acc = _rFacing.forward * (param_acc + _momentum) * (directionFector - 1);
        Vector3 appliedAcc = acc + mAcc;
        appliedAcc = _terrianRotation * appliedAcc;
        _rRb.AddForce(appliedAcc, ForceMode.Acceleration);
    }
    void Jumping()
    { 
        if(!_grounded)
            return;

        _grounded = false;
        _rRb.AddForce(Vector3.up * param_jumpForce, ForceMode.VelocityChange);
    }

    void Boosting()
    {
        if (!_boosting)
        {
            if (_nuetralInput && _xzSpeed > 0)
                _inputDirection = _xzPlainVel.normalized * -0.2f;
            return;
        }
        if (_nuetralInput)
            _inputDirection = _xzPlainVel.normalized;

        _accCoefficient += 1;
    }
    void NewSpeedSystem()
    { 
        //sigmoid function for clamping the acceleratioin
        _accCoefficient = Mathf.Min(1.0f, 2.0f / (1 + Mathf.Pow( 2.7f , param_torqueCoefficient * (_xzPlainVel.magnitude - param_maxSpeed))));
        Boosting();
        _debugText.FixedText += " AccCoefficient: " + _accCoefficient.ToString("F4");
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
            * Mathf.Max( 0, Vector3.Dot(terrianSlope, _rFacing.forward));//do max with zero to negate uphill speed decrease
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
        _debugText.FixedText += "Momentum: " + _momentum.ToString("F4");
    }

    void MomentumSystem()
    {
        if (!_grounded)
            return;

        float dTime = Time.fixedDeltaTime;
        float currentVel = _rRb.velocity.magnitude;
        float buildUp = param_acc;

        _momentum = Mathf.Max( 0, _xzSpeed - param_maxSpeed);
        _debugText.FixedText += "Momentum: " + _momentum.ToString("F4") + " Boosting: " + _boosting.ToString();
    }

    void InputFetcher()
    {
        _horiInput = Input.GetAxisRaw("Horizontal");
        _vertInput = Input.GetAxisRaw("Vertical");
        _nuetralInput = _inputDirection == Vector3.zero;
        _rawInput = new Vector2(_vertInput, _horiInput);
        _inputDirection = Vector3.Normalize(_rCamFacing.forward * _vertInput + _rCamFacing.right * _horiInput);

        _boosting = Input.GetButton("Fire1");
        _drifting = Input.GetButton("Fire2");
        _launching = Input.GetButton("Fire3");
        _jumping = Input.GetButton("Jump");
    }
}
