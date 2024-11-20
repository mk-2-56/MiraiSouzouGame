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

namespace CC
{ 
    public class PlayerMovementParams
    {
        public Vector3 xzPlainVel;
        public float   xzSpeed;
        public float   momentum;
        public float   accCoefficient;
        public float   airCoefficient = 0.5f;
        public Flags   flags;
        public Input   inputs;

        public Vector3    terrianNormal;
        public Quaternion terrianRotation;

        public struct Input
        { 
            public Vector3 raw;
            public Vector3 inputDirection;
        }
        public struct Flags
        {
            public bool grounded;

            public bool noInput;
            public bool boosting;
            public bool jumping;
            public bool drifting;
            public bool launching;
        }
    }


    public class Basic : MonoBehaviour
    {
        public PlayerMovementParams GetPlayerMovementParams()
        { 
            return _movementParams;
        }

        public float acc
        {  
            get{ return param_acc;}
        }

        //_____________Parameters
        [SerializeField] float param_maxSpeed = 60.0f;
        [SerializeField] float param_acc = 10.0f;
        [SerializeField] float param_jumpForce = 30.0f;
        [SerializeField] float param_torqueCoefficient = 0.03f;

        //Debug:
        GameObject   _rDebug;
        LineRenderer _rDebugLineRender;

        //_____________References
        Rigidbody _rRb;
        Transform _rFacing;
        Transform _rCamFacing;

        CC.Hub  _rCChub;
        //_____________Members
    
        Vector3 _rawInput;
        Vector3 _inputDirection;
    
        PlayerMovementParams _movementParams = new PlayerMovementParams();

        void Start()
        {
            _rRb = GetComponent<Rigidbody>();
            _rFacing    = transform.Find("Facing");
            _rCamFacing = transform.Find("CamFacing");

            _rCChub   = GetComponent<CC.Hub>(); 
            RegisterActions();

            _rDebug = transform.Find("Debug").gameObject;
            _rDebugLineRender = _rDebug.GetComponent<LineRenderer>();
        }
        
        void RegisterActions()
        {
            _rCChub.MoveEvent += HandleMove;
            _rCChub.JumpStartEvent += HandleJumpStart;
            _rCChub.JumpEndEvent   += HandleJumpEnd;
            _rCChub.BoostStartEvent += HandleBoostStart;
            _rCChub.BoostEndEvent   += HandleBoostEnd;
        }

        void HandleMove(Vector3 input)
        {
            _rawInput = input;
            _movementParams.flags.noInput = _inputDirection == Vector3.zero;
        }

        void HandleJumpStart()
        {
            if (!_movementParams.flags.grounded)
                return;
    
            _rRb.AddForce(Vector3.up * param_jumpForce, ForceMode.VelocityChange);
            _movementParams.flags.jumping = true;
            Invoke("HandleJumpEnd", 1.5f);
        }
        void HandleJumpEnd()
        {
            _movementParams.flags.jumping = false;
        }

        void HandleBoostStart()
        {
            _movementParams.flags.boosting = true;
        }
        void HandleBoostEnd()
        {
            _movementParams.flags.boosting = false;
        }

        private void FixedUpdate()
        {
            _movementParams.inputs.inputDirection = 
                _inputDirection = Vector3.Normalize(_rCamFacing.forward * _rawInput.y + _rCamFacing.right * _rawInput.x);
            BasicDebugInfo(); 

            _movementParams.xzPlainVel = new Vector3(_rRb.velocity.x, 0, _rRb.velocity.z);
            _movementParams.xzSpeed    = _movementParams.xzPlainVel.magnitude;
            SpeedSystem();
            if (!_movementParams.flags.drifting || !_movementParams.flags.grounded)
            {
                if (_movementParams.xzPlainVel.sqrMagnitude > 5.0f)
                {
                    _rFacing.rotation = Quaternion.Lerp(_rFacing.rotation,
                        Quaternion.LookRotation(_movementParams.xzPlainVel.normalized, Vector3.up), 3.0f * Time.fixedDeltaTime);
                }
                Locomovtive();
            }

            if (_movementParams.flags.grounded)
                return;
            if( !_movementParams.flags.jumping && _rRb.useGravity)
                _rRb.AddForce(Vector3.down * 9.8f * 3, ForceMode.Acceleration);
        }
    
        void Locomovtive()
        {

            float directionFector = 3.0f - 2 * Vector3.Dot(_inputDirection, _movementParams.xzPlainVel.normalized);
    
            float accMag = param_acc;
            Vector3 acc  = _inputDirection * accMag * directionFector * _movementParams.accCoefficient;
                
            if (_movementParams.flags.grounded)
            {
                Vector3 terrianSlope = new Vector3(_movementParams.terrianNormal.x, 0, _movementParams.terrianNormal.z).normalized;
                float slideBuildUp = (1 - Vector3.Dot(_movementParams.terrianNormal, Vector3.up))
                    * Mathf.Max(-1, Vector3.Dot(terrianSlope, _rFacing.forward));
    
                acc += _inputDirection * 98f * slideBuildUp;
            }
            Vector3 appliedAcc = acc;
            appliedAcc = _movementParams.terrianRotation * appliedAcc;
    
            _rDebugLineRender.SetPosition(0, _rRb.position);
            _rDebugLineRender.SetPosition(1, _rRb.position + appliedAcc);
            _rRb.AddForce(appliedAcc, ForceMode.Acceleration);
    
        }
        void SpeedSystem()
        {
            //sigmoid function for clamping the acceleratioin
            _movementParams.accCoefficient = Mathf.Min(1.0f, 2.0f / 
                (1 + Mathf.Pow( 2.7f , param_torqueCoefficient * (_movementParams.xzPlainVel.magnitude - param_maxSpeed))));
            _movementParams.momentum = Mathf.Max(0, _movementParams.xzSpeed - param_maxSpeed);

            if(!_movementParams.flags.grounded)
            {
                _movementParams.accCoefficient = _movementParams.accCoefficient * _movementParams.airCoefficient;

                if (!_movementParams.flags.noInput)
                    return;
                if (_movementParams.xzSpeed > param_maxSpeed)
                    _inputDirection = _movementParams.xzPlainVel.normalized * -0.2f;
                else
                    _rRb.AddForce(_movementParams.xzPlainVel * -0.35f, ForceMode.Acceleration);
                return;
            }


            if (_movementParams.flags.boosting)
            {
                if (_movementParams.flags.noInput)
                    _inputDirection = _movementParams.xzPlainVel.normalized;

                _movementParams.accCoefficient += 1;
                return;
            }
        }
    
        void BasicDebugInfo()
        {
            float speed = _movementParams.xzSpeed;
            AU.Debug.Log(speed, AU.LogTiming.Fixed);
            AU.Debug.Log(_movementParams.xzPlainVel, AU.LogTiming.Fixed);
            AU.Debug.Log(_movementParams.flags.boosting, AU.LogTiming.Fixed);
        }
    }
}