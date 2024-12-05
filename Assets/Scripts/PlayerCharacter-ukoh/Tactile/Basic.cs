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
        public bool enabled = true;

        public Vector3 xzPlainVel;
        public float xzSpeed;
        public float momentum;
        public float accCoefficient;
        public float airCoefficient = 0.5f;
        public Flags flags;
        public Input inputs;

        public Vector3 terrianNormal;
        public Quaternion terrianRotation;

        public struct Input
        {
            public Vector3 raw;
            public Vector3 inputDirection;
        }
        public struct Flags
        {
            public bool antiGrav;
            public bool grounded;
            public bool groundedFlat;

            public bool noInput;
            public bool boosting;
            public bool jumping;
            public bool minJumping;
            public bool drifting;
            public bool launching;
        }
    }


    public class Basic : MonoBehaviour
    {
        public event System.Action        JumpEffect;
        public Vector3 inutDIrectionWorld
        {
            get { return _movementParams.inputs.inputDirection; }
        }

        public PlayerMovementParams GetPlayerMovementParams()
        {
            return _movementParams;
        }

        public float acc
        {
            get { return param_acc; }
        }

        //_____________Parameters
        [SerializeField] float param_maxSpeed = 60.0f;
        [SerializeField] float param_acc = 10.0f;
        [SerializeField] float param_jumpForce = 30.0f;
        [SerializeField] float param_torqueCoefficient = 0.03f;
        [SerializeField] float param_minAirTime = 0.1f;
        [SerializeField] float param_maxAirTime = 0.9f;
        //Debug:
        GameObject _rDebug;
        LineRenderer _rDebugLineRender;

        //_____________References
        Rigidbody _rRb;
        Transform _rFacing;
        Transform _rCamFacing;

        CC.Hub _rCChub;
        //_____________Members

        Vector3 _rawInput;
        Vector3 _inputDirection;

        PlayerMovementParams _movementParams = new PlayerMovementParams();

        void Start()
        {
            _rRb = GetComponent<Rigidbody>();
            _rFacing = transform.Find("Facing");
            _rCamFacing = transform.Find("CamFacing");

            _rCChub = GetComponent<CC.Hub>();
            RegisterActions();

            _rDebug = transform.Find("Debug").gameObject;
            _rDebugLineRender = _rDebug.GetComponent<LineRenderer>();
            //_rDebug.gameObject.SetActive(false);
        }

        void RegisterActions()
        {
            _rCChub.MoveEvent       += HandleMove;
            _rCChub.JumpStartEvent  += HandleJumpStart;
            _rCChub.JumpEndEvent    += HandleJumpEnd;
            _rCChub.BoostStartEvent += HandleBoostStart;
            _rCChub.BoostEndEvent   += HandleBoostEnd;
        }

        void HandleMove(Vector3 input)
        {
            _rawInput = input;
            _movementParams.flags.noInput = input == Vector3.zero;
        }

        void HandleJumpStart()
        {
            if (!_movementParams.flags.grounded)
                return;

            _rRb.AddForce(Vector3.up * param_jumpForce, ForceMode.VelocityChange);
            _movementParams.flags.jumping = true;
            _movementParams.flags.minJumping = true;
            Invoke("EndMinimalJump", param_minAirTime);
            Invoke("HandleJumpEnd", param_maxAirTime);
            JumpEffect?.Invoke();
        }
        void HandleJumpEnd()
        {
            _movementParams.flags.jumping = false;
        }
        void EndMinimalJump()
        {
            _movementParams.flags.minJumping = false;
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
            if (!_movementParams.enabled)
                return;
            //input direction to Camera Space
            _movementParams.inputs.inputDirection =
                _inputDirection = Vector3.Normalize(_rCamFacing.forward * _rawInput.y + _rCamFacing.right * _rawInput.x);

            //noGravity conditions
            //On Ground, or fast enough or jumping
            _movementParams.flags.antiGrav =
                _movementParams.flags.groundedFlat || (_movementParams.flags.grounded && _movementParams.momentum > 0.0f)
                || _movementParams.flags.jumping || _movementParams.flags.minJumping;
            _rRb.useGravity = !(_movementParams.flags.antiGrav);

            SpeedSystem();
            if (!_movementParams.flags.drifting || !_movementParams.flags.grounded)
            {
                if (_movementParams.flags.antiGrav)
                {
                    Vector3 facingV = _movementParams.xzPlainVel.normalized;

                    _rFacing.rotation = Quaternion.Lerp(_rFacing.rotation, Quaternion.LookRotation(facingV, Vector3.up), 3f * Time.deltaTime);
                }
                Locomovtive();
            }

            if (_movementParams.flags.grounded)
                return;
            if (_rRb.useGravity)//Extra Grav
                _rRb.AddForce(Vector3.down * 9.8f * 3, ForceMode.Acceleration);

            BasicDebugInfo();
        }

        void Locomovtive()
        {
            float directionFector = 3.0f - 2 * Vector3.Dot(_inputDirection, _movementParams.xzPlainVel.normalized);

            float accMag = param_acc;
            Vector3 acc = _inputDirection * accMag * directionFector * _movementParams.accCoefficient;

            if (_movementParams.flags.groundedFlat && !_movementParams.flags.noInput)
            {//斜面による加速/減速効果
                Vector3 terrianSlope = new Vector3(_movementParams.terrianNormal.x, 0, _movementParams.terrianNormal.z).normalized;
                float slideBuildUp = (1 - Vector3.Dot(_movementParams.terrianNormal, Vector3.up))
                    * Mathf.Max(-1, Vector3.Dot(terrianSlope, _rFacing.forward));

                acc += _inputDirection * 98f * slideBuildUp;
            }
            Vector3 appliedAcc = acc;
            if (_movementParams.flags.antiGrav)
                appliedAcc = _movementParams.terrianRotation * appliedAcc;
            _rRb.AddForce(appliedAcc, ForceMode.Acceleration);

            {//debug
                Vector3 p0 = _rRb.position + _movementParams.terrianNormal;
                Vector3 p1 = p0 + appliedAcc;
                _rDebugLineRender.SetPosition(0, p0);
                _rDebugLineRender.SetPosition(1, p1);
            }
        }
        void SpeedSystem()
        {
            Vector3 nativeVel = Quaternion.Inverse(_movementParams.terrianRotation) * _rRb.velocity;
            nativeVel.y = 0;
            _movementParams.xzPlainVel = nativeVel;
            _movementParams.xzSpeed = _movementParams.xzPlainVel.magnitude;

            //sigmoid function for clamping the acceleratioin
            _movementParams.accCoefficient = Mathf.Min(1.0f, 2.0f /
                (1 + Mathf.Pow(2.7f, param_torqueCoefficient * (_movementParams.xzPlainVel.magnitude - param_maxSpeed))));
            _movementParams.momentum = Mathf.Max(0, _movementParams.xzSpeed - param_maxSpeed);

            if (!_movementParams.flags.grounded)
            {//空中時の推進力減少
                _movementParams.accCoefficient = _movementParams.accCoefficient * _movementParams.airCoefficient;
            }

            if (_movementParams.flags.boosting)
            {//Boost時の推進力増大
                if (_movementParams.flags.noInput)
                    _inputDirection = _movementParams.xzPlainVel.normalized;

                _movementParams.accCoefficient += 1;
                return;
            }

            if (_movementParams.flags.noInput)
            {//Adjust input behaviour when no input from player
                if (_movementParams.xzSpeed > param_maxSpeed)
                    _inputDirection = _movementParams.xzPlainVel.normalized * -0.2f;
                else
                    _rRb.AddForce(_movementParams.xzPlainVel * -0.35f, ForceMode.Acceleration);
                return;
            }

        }

        void BasicDebugInfo()
        {
            float speed = _movementParams.xzSpeed;
            AU.Debug.Log(speed, AU.LogTiming.Fixed);
            AU.Debug.Log(_movementParams.xzPlainVel, AU.LogTiming.Fixed);

            AU.Debug.Log(_rFacing.forward, AU.LogTiming.Fixed);
        }
    }
}