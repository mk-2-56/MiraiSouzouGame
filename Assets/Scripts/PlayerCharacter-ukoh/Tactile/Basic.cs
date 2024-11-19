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
    public class TerrianHit
    {
        public bool    grounded;

        public Vector3 terrianNormal;
        public Quaternion terrianRotation;
    }

    public struct ActionFlags
    {
        public bool noInput;
        public bool boosting;
        public bool jumping;
        public bool drifting;
        public bool launching;
    }


    public class Basic : MonoBehaviour
    {
        public float Speed
        {
            get{ return _xzSpeed;}
        }
        
        public TerrianHit GetTerrianHit()
        { 
            return _terrianHit;
        }

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


        //Debug:
        GameObject _rDebug;
        LineRenderer _rDebugLineRender;


        //_____________References
        Rigidbody _rRb;
        Transform _rCamFacing;
        Transform _rFacing;

        CC.Hub  _rCChub;

        //_____________Members
        ActionFlags _flags;
        Vector3 _xzPlainVel;
        float   _xzSpeed;
        float   _momentum;
        float   _accCoefficient;
        float   _airCoefficient = 0.5f;
    
        Vector2 _rawInput;
        Vector3 _inputDirection;
    
        TerrianHit _terrianHit = new TerrianHit();

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
            _rCChub.JumpStartEvent += HandleJumpStart;
            _rCChub.JumpEndEvent += HandleJumpEnd;
            _rCChub.MoveEvent += HandleMove;
        }

        void HandleMove(Vector2 input)
        {
            _rawInput = input;
        }

        void HandleJumpStart()
        {
            if (!_terrianHit.grounded)
                return;
    
            _rRb.AddForce(Vector3.up * param_jumpForce, ForceMode.VelocityChange);
            _flags.jumping = true;
            Invoke("HandleJumpEnd", 1.5f);
        }
        void HandleJumpEnd()
        {
            _flags.jumping = false;
        }
    
        private void FixedUpdate()
        {
            _xzPlainVel = new Vector3(_rRb.velocity.x, 0, _rRb.velocity.z);
            _xzSpeed    = _xzPlainVel.magnitude;
            SpeedSystem();
            Locomovtive();

            if (_terrianHit.grounded)
                return;
            if( !_flags.jumping && _rRb.useGravity)
                _rRb.AddForce(Vector3.down * 9.8f * 3, ForceMode.Acceleration);
            BasicDebugInfo(); 
        }
    
    
        void Locomovtive()
        {
            _inputDirection = Vector3.Normalize(_rCamFacing.forward * _rawInput.y + _rCamFacing.right * _rawInput.x);

            float directionFector = 3.0f - 2 * Vector3.Dot(_inputDirection, _xzPlainVel.normalized);
    
            float accMag = param_acc;
            Vector3 acc  = _inputDirection * accMag * directionFector * _accCoefficient;
                
            if (_terrianHit.grounded)
            {
                Vector3 terrianSlope = new Vector3(_terrianHit.terrianNormal.x, 0, _terrianHit.terrianNormal.z).normalized;
                float slideBuildUp = (1 - Vector3.Dot(_terrianHit.terrianNormal, Vector3.up))
                    * Mathf.Max(-1, Vector3.Dot(terrianSlope, _rFacing.forward));
    
                acc += _inputDirection * 98f * slideBuildUp;
            }
            Vector3 appliedAcc = acc;
            appliedAcc = _terrianHit.terrianRotation * appliedAcc;
    
            _rDebugLineRender.SetPosition(0, _rRb.position);
            _rDebugLineRender.SetPosition(1, _rRb.position + appliedAcc);
            _rRb.AddForce(appliedAcc, ForceMode.Acceleration);
    
            if (_xzPlainVel.sqrMagnitude > 5.0f)
            {
                _rFacing.rotation = Quaternion.LookRotation(_xzPlainVel.normalized, Vector3.up);
            }
        }
    
        void SpeedSystem()
        {
            //sigmoid function for clamping the acceleratioin
            _accCoefficient = Mathf.Min(1.0f, 2.0f / 
                (1 + Mathf.Pow( 2.7f , param_torqueCoefficient * (_xzPlainVel.magnitude - param_maxSpeed))));
            if(!_terrianHit.grounded)
                _accCoefficient = _accCoefficient * _airCoefficient;
    
            _momentum = Mathf.Max(0, _xzSpeed - param_maxSpeed);
            //if(_boosting)
            //    _accCoefficient += 1.0f;
        }
    
        void BasicDebugInfo()
        {
            AU.Debug.Log(_inputDirection, AU.LogTiming.Fixed);
            AU.Debug.Log(_flags.jumping, AU.LogTiming.Fixed);
        }
    }
}