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
    public void SetPlayerIndex(int index)
    {
     
        _playerIndex = index;
        _axisNamePrefix = "P" + (_playerIndex).ToString() + "_";

        Debug.Log(_axisNamePrefix + "/t" + _playerIndex);
        //temp
        //learn and use scripted object later
    }

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

    public string inputAxisPrefix
    { 
        get{ return _axisNamePrefix; }
    }

    public void ResigterLaunchModule(Command cmd)
    { 
        _launchingCommand = cmd;
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
    GameObject _rDebug;
    LineRenderer _rDebugLineRender;
    //_____________Properties

    //_____________Parameters
    [Tooltip("スピードのソフト上限。")]
    [SerializeField] float param_maxSpeed = 60.0f;
    [Tooltip("加速度/加速の速さ")]
    [SerializeField] float param_acc = 10.0f;
    [Tooltip("Jumpの強度/高さ")]
    [SerializeField] float param_jumpForce = 30.0f;
    [Tooltip("スピード上限を超えた力の低下率")]
    [SerializeField] float param_torqueCoefficient = 0.03f;
    [Tooltip("ドリフト時の回転速度上限")]
    [SerializeField] float param_maxAngularAcc = 15.0f;

    //temp
    int    _playerIndex;
    string _axisNamePrefix;

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
    Vector2 _lookInput;

    Vector3 _terrianNormal;
    Quaternion _terrianRotation;
    
    Command _launchingCommand;

    //______________Flags
    //State _PCstate;

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
        _rDebug = transform.Find("Debug").gameObject;
        _rDebugLineRender = _rDebug.GetComponent<LineRenderer>();
    }

    private void Awake()
    {
    }

    void Update()
    {
        InputFetcher();
    }
    private void FixedUpdate()
    {
        _xzPlainVel = new Vector3(_rRb.velocity.x, 0, _rRb.velocity.z);
        _xzSpeed    = _xzPlainVel.magnitude;

        BasicDebugInfo();
        TerrianCheck();
        if(!_grounded) 
            WallCheck();

        _rRb.useGravity = !_grounded;

        NewSpeedSystem();
        MomentumSystem();

        if(_jumping)
            Jumping();

        if(_launching) 
            _launchingCommand.Execute();


        Drifting();
        Locomovtive();
        _driftingOld = _drifting;
    }

    void BasicDebugInfo()
    {
        _debugText.FixedText += "Grounded: " + _grounded.ToString();
        _debugText.FixedText += "\tInput: " + _inputDirection.ToString();
        _debugText.FixedText += "<br>Speed: " + _xzPlainVel.magnitude.ToString("F4");
        _debugText.FixedText += "\tMomentum: " + _momentum.ToString("F4");

        _debugText.FixedText += "\tGravity: " + _rRb.useGravity.ToString();
    }

    void WallCheck()
    {
        bool wallhit = false;

        //temp
        LayerMask layerMask = LayerMask.GetMask("Terrian");
        float radius = 1.0f;
        float distance = 4.0f;
        RaycastHit hit;


        wallhit = Physics.SphereCast(_rRb.transform.position - _rCamFacing.forward, radius, _rCamFacing.forward,
            out hit, distance, layerMask);
        _debugText.FixedText += "<br>Wallhit: " + wallhit.ToString();
        if (!wallhit)
            return;

        if (hit.normal.y < 0.3f )
        { 
            _grounded = true;
            _terrianRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
        }
    }

    void TerrianCheck()
    {
        _grounded = _rCCHover.Grounded;
        _terrianNormal = _rCCHover.Groundhit.normal;
        _terrianRotation = Quaternion.identity;
        if (_grounded)
        {
            //(Mathf.Abs(Vector3.Dot(_terrianNormal, _rRb.velocity.normalized)) > 0.5f) ||
            bool terrianClamp =  _terrianNormal.y < 0.5;
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
        //Vector3 mAcc = _inputDirection * _momentum - _xzPlainVel.normalized * ( _momentum * ( -directionFector + 2));
            
        if (_grounded)
        {
            acc *= directionFector * _accCoefficient;

            Vector3 terrianSlope = new Vector3(_terrianNormal.x, 0, _terrianNormal.z).normalized;
            float slideBuildUp = (1 - Vector3.Dot(_terrianNormal, Vector3.up))
                * Mathf.Max(-1, Vector3.Dot(terrianSlope, _rFacing.forward));

            _debugText.FixedText += "<br>Slide: " + (98f * slideBuildUp).ToString("F4");
            acc += _inputDirection * 98f * slideBuildUp;
        }
        Vector3 appliedAcc = acc;
        appliedAcc = _terrianRotation * appliedAcc;

        _debugText.FixedText += "<br>Acc: " + acc.ToString();
        _rDebugLineRender.SetPosition(0, _rRb.position);
        _rDebugLineRender.SetPosition(1, _rRb.position + appliedAcc);
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
            if (!_nuetralInput)
                return;
            if (_xzSpeed > param_maxSpeed)
                _inputDirection = _xzPlainVel.normalized * -0.2f;
            else
                _rRb.AddForce( _xzPlainVel * -0.35f, ForceMode.Acceleration);
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
        _debugText.UpdateText += "PlayerIndex: " + _playerIndex.ToString()
            + "Prefix:" + _axisNamePrefix;
        _axisNamePrefix = "P" + (_playerIndex).ToString() + "_";

        _boosting  = Input.GetAxis(_axisNamePrefix + "Accele") > 0;
        _drifting  = Input.GetAxis(_axisNamePrefix + "Drift")  > 0;
        _launching = Input.GetAxis(_axisNamePrefix + "Boost")  > 0;
        _jumping   = Input.GetAxis(_axisNamePrefix + "Jump")   > 0;

        float horiInput = Input.GetAxisRaw(_axisNamePrefix + "Horizontal");
        float vertInput = Input.GetAxisRaw(_axisNamePrefix + "Vertical");

        _rawInput = new Vector2(vertInput, horiInput);
        _nuetralInput = _rawInput == Vector3.zero;
        
        _inputDirection = Vector3.Normalize(_rCamFacing.forward * vertInput + _rCamFacing.right * horiInput);
    }
}
