using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC
{
    public class Drift : MonoBehaviour
    {
        [SerializeField] float param_maxAngularAcc = 72.0f;



        float _acc;

        PlayerMovementParams _rMovementParams;

        bool _driftInput;
        bool _driftingOld;

        Rigidbody _rRb;
        Transform _rCamFacing;
        Transform _rFacing;

        void HandleDriftStart()
        {
            _rMovementParams.flags.drifting = _driftInput = true;
        }
        void HandleDriftEnd()
        {
            _driftInput = false;
            //_rMovementParams.flags.drifting = false;
        }

        // Start is called before the first frame update
        void Start()
        {
            _rRb = GetComponent<Rigidbody>();
            _rFacing = transform.Find("Facing");

            CC.Hub CChub = GetComponent<CC.Hub>();
            CChub.DriftStartEvent += HandleDriftStart;
            CChub.DriftEndEvent += HandleDriftEnd;

            CC.Basic temp = GetComponent<CC.Basic>();
            _rMovementParams = temp.GetPlayerMovementParams();
            _acc = temp.acc;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (!_rMovementParams.enabled)
                return;
            if (_driftingOld)
                _rMovementParams.flags.drifting = _driftInput ||
                    Vector3.Dot(_rMovementParams.xzPlainVel.normalized, _rFacing.forward) < 0.90f;

            if (_rMovementParams.flags.grounded && _rMovementParams.flags.drifting)
                Drifting();
            _driftingOld = _rMovementParams.flags.drifting;
        }

        void Drifting()
        {
            Vector3 acc;
            float directionFector = 2.0f - Vector3.Dot(_rFacing.forward, _rMovementParams.xzPlainVel.normalized);

            if (_rMovementParams.flags.noInput)
            {
                if (_rMovementParams.xzPlainVel != Vector3.zero)
                    _rRb.AddForce(_rMovementParams.terrianRotation *
                        (_rMovementParams.xzPlainVel.normalized * -_acc), ForceMode.Acceleration);
                return;
            }

            Vector3 mAcc = _rFacing.forward * _rMovementParams.momentum - _rMovementParams.xzPlainVel.normalized
                * (_rMovementParams.momentum * (-directionFector + 2));

            _rFacing.rotation = Quaternion.RotateTowards(
                _rFacing.rotation, Quaternion.LookRotation(_rMovementParams.inputs.inputDirection, Vector3.up),
                param_maxAngularAcc * Time.deltaTime);

            acc = _rFacing.forward * (_acc + _rMovementParams.momentum) * (directionFector - 1);
            Vector3 appliedAcc = acc + mAcc;
            appliedAcc = _rMovementParams.terrianRotation * appliedAcc;
            _rRb.AddForce(appliedAcc, ForceMode.Acceleration);
        }

    }
}