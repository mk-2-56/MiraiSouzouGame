using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ukoh-2024-10-28
/// プレイヤーRB浮遊機能
/// 
/// 凸凹な地形でも滑らかに移動させる
/// parameters:
/// HoverHeight             浮遊高さ
/// HoverSpringStrength     浮遊力の強さ
/// HoverDamperStrength     wip、今は何もしない
/// </summary>

namespace CC
{
    public class Hover : MonoBehaviour
    {
        public event System.Action OffGroundE;
        public event System.Action GroundedE;

        LayerMask _layerMask;

        public RaycastHit Groundhit
        {
            get { return _rayHit; }
        }

        //Parameters:
        [SerializeField] float HoverHeight = 1.5f;
        [SerializeField] float HoverSpringStrength = 30.0f;
        [SerializeField] float HoverDamperStrength = 1.0f;

        //Memebers:
        Rigidbody _rRb;
        Transform _rCog;
        Transform _rFacing;
        RaycastHit _rayHit;

        Vector3 _terrianDirOld;
        bool    _groundedOld;

        CC.PlayerMovementParams _rMovementParams;

        void Start()
        {
            _rRb = GetComponent<Rigidbody>();
            _rFacing = transform.Find("Facing");
            _rCog = _rFacing.Find("Cog");



            _rMovementParams = GetComponent<CC.Basic>().GetPlayerMovementParams();
            _layerMask = LayerMask.GetMask("Terrian");
        }

        // Update is called once per frame
        void Update()
        {
        }

        private void FixedUpdate()
        {
            if (!_rMovementParams.enabled)
                return;
            float dtime = Time.deltaTime;

            TerrianCheck();
            if (_rMovementParams.flags.grounded)
            {
                _rCog.position = _rayHit.point;
                _rCog.rotation = _rMovementParams.terrianRotation * _rFacing.rotation;
            }
            else
                if (_rRb.velocity != Vector3.zero)
                _rCog.rotation = Quaternion.Lerp(_rCog.rotation, Quaternion.LookRotation(_rRb.velocity.normalized), 3 * dtime);

            AU.Debug.Log(_rCog.forward, AU.LogTiming.Fixed);
        }

        void TerrianCheck()
        {
            Vector3 rayDir;
            rayDir = _terrianDirOld;
            float offset = 2.0f;
            float rayLength = HoverHeight + 2.0f + offset;
            Vector3 offsetVec = _terrianDirOld * offset;

            float radius = 2.0f;

            if (_rMovementParams.flags.minJumping)
            {
                _rMovementParams.flags.grounded = false;
            }
            else
            {
                if (!_rMovementParams.flags.grounded)
                {
                    _rMovementParams.flags.grounded = Physics.SphereCast(_rRb.position - offsetVec, radius,
                    rayDir, out _rayHit, rayLength, _layerMask);
                }
                _rMovementParams.flags.grounded = Physics.Raycast(_rRb.position - offsetVec,
                    rayDir, out _rayHit, rayLength, _layerMask);
            }

            if(_groundedOld != _rMovementParams.flags.grounded)
            { 
                if(_rMovementParams.flags.grounded)
                    GroundedE?.Invoke();
                else
                    OffGroundE?.Invoke();
            }
            _groundedOld = _rMovementParams.flags.grounded;

            if (!_rMovementParams.flags.grounded)
            {
                _terrianDirOld = _rRb.velocity.normalized;
                _rMovementParams.flags.groundedFlat = false;
                _rMovementParams.terrianNormal = Vector3.up;
                _rMovementParams.terrianRotation = Quaternion.identity;
                return;
            }

            _rMovementParams.terrianNormal = _rayHit.normal;
            _rMovementParams.terrianRotation = Quaternion.FromToRotation(Vector3.up, _rMovementParams.terrianNormal);
            _terrianDirOld = -_rayHit.normal;

            bool terrianWalkale = _rayHit.normal.y > 0.8;
            _rMovementParams.flags.groundedFlat = terrianWalkale;
            if (!(_rMovementParams.flags.groundedFlat || _rMovementParams.flags.antiGrav))
                return;

            float verticalVel = Vector3.Dot(_rRb.velocity, _rayHit.normal);
            float x = -(_rayHit.distance - offset - HoverHeight);
            float hoverForce = x * HoverSpringStrength - verticalVel * HoverDamperStrength;

            _rRb.AddForce(_rayHit.normal * hoverForce, ForceMode.Acceleration);
        }

        public void SetOffGround()
        {
            OffGroundE?.Invoke();
        }

    }


}