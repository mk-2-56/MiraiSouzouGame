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
        LayerMask _layerMask;
    
        public RaycastHit Groundhit
        { 
            get { return _rayHit;}
        }
    
        //Parameters:
        [SerializeField] float HoverHeight = 1.5f;
        [SerializeField] float HoverSpringStrength = 30.0f;
        [SerializeField] float HoverDamperStrength = 1.0f;
    
        //Memebers:
        Rigidbody  _rRb;
        Transform  _rCog;
        Transform  _rFacing;
        RaycastHit _rayHit;

        Vector3 _terrianDirOld;
    
        CC.PlayerMovementParams _rMovementPrama;
    
        void Start()
        {
            _rRb = GetComponent<Rigidbody>();
            _rFacing = transform.Find("Facing");
            _rCog    = _rFacing.Find("Cog");
    
    
    
            _rMovementPrama = GetComponent<CC.Basic>().GetPlayerMovementParams();
            _layerMask = LayerMask.GetMask("Terrian");
        }
    
        // Update is called once per frame
        void Update()
        {
        }
    
        private void FixedUpdate()
        {
            float dtime = Time.deltaTime;

            TerrianCheck();
            if(_rMovementPrama.flags.grounded)
            {
                _rCog.position = _rayHit.point;
                _rCog.rotation = _rMovementPrama.terrianRotation * _rFacing.rotation;
            }
            else
                if(_rRb.velocity != Vector3.zero)
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

            if(_rMovementPrama.flags.minJumping)
            {
                _rMovementPrama.flags.grounded = false;
            }
            else
            {
                if (!_rMovementPrama.flags.grounded)
                {
                    _rMovementPrama.flags.grounded = Physics.SphereCast(_rRb.position - offsetVec, radius,
                    rayDir, out _rayHit, rayLength, _layerMask);
                }
                    _rMovementPrama.flags.grounded = Physics.Raycast(_rRb.position - offsetVec,
                        rayDir, out _rayHit, rayLength, _layerMask);
            }

            {
                string hitObjName = "";
                hitObjName = _rayHit.collider?.gameObject.name;
            }

            if (!_rMovementPrama.flags.grounded)
            {
                _terrianDirOld = _rRb.velocity.normalized;
                _rMovementPrama.flags.groundedFlat = false;
                _rMovementPrama.terrianNormal = Vector3.up;
                _rMovementPrama.terrianRotation = Quaternion.identity;
                return;
            }

            _rMovementPrama.terrianNormal = _rayHit.normal;
            _rMovementPrama.terrianRotation = Quaternion.FromToRotation(Vector3.up, _rMovementPrama.terrianNormal);
            _terrianDirOld = -_rayHit.normal;

            bool terrianWalkale = _rayHit.normal.y > 0.8;
            _rMovementPrama.flags.groundedFlat = terrianWalkale;
            if(!(_rMovementPrama.flags.groundedFlat || _rMovementPrama.flags.antiGrav))
                return;

            float verticalVel = Vector3.Dot(_rRb.velocity, _rayHit.normal);
            float x = -(_rayHit.distance - offset - HoverHeight);
            float hoverForce = x * HoverSpringStrength - verticalVel * HoverDamperStrength;

            _rRb.AddForce(_rayHit.normal * hoverForce, ForceMode.Acceleration);
        }
    }

}