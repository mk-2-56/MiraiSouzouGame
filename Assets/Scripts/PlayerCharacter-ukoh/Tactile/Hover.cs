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
    
        CC.PlayerMovementParams _rTerrianHit;
    
        void Start()
        {
            _rRb = GetComponent<Rigidbody>();
            _rFacing = transform.Find("Facing");
            _rCog    = _rFacing.Find("Cog");
    
    
    
            _rTerrianHit = GetComponent<CC.Basic>().GetPlayerMovementParams();
            _layerMask = LayerMask.GetMask("Terrian");
        }
    
        // Update is called once per frame
        void Update()
        {
        }
    
        private void FixedUpdate()
        {
            TerrianCheck();
            if(_rTerrianHit.flags.grounded)
            {
                _rCog.position = _rayHit.point;
                _rCog.rotation = _rTerrianHit.terrianRotation * _rFacing.rotation;
            }
            else
                _rCog.rotation = Quaternion.Lerp(_rCog.rotation, Quaternion.LookRotation(_rRb.velocity.normalized), 0.3f);
        }

        void TerrianCheck()
        {
            Vector3 rayDir;
            rayDir = _terrianDirOld + _rRb.velocity * Time.fixedDeltaTime;
            float rayLength = HoverHeight + 2.0f;


            float radius = 2.0f;

            if (!_rTerrianHit.flags.grounded)
            {
                bool hit;
                RaycastHit temp;

                hit = Physics.Raycast(_rRb.position + Vector3.up,
                    Vector3.down, out temp, rayLength, _layerMask);
                _rTerrianHit.flags.grounded |= hit;
                if(hit) _rayHit = temp;

                hit = Physics.SphereCast(_rRb.position, radius,
                rayDir, out temp, rayLength, _layerMask);
                _rTerrianHit.flags.grounded |= hit;
                if (hit) _rayHit = temp;
            }
            else
            {
                _rTerrianHit.flags.grounded = Physics.Raycast(_rRb.position,
                    rayDir, out _rayHit, rayLength, _layerMask);
            }

            {
                string hitObjName = "";
                hitObjName = _rayHit.collider?.gameObject.name;
                AU.Debug.Log(hitObjName, AU.LogTiming.Fixed);
                AU.Debug.Log(_terrianDirOld, AU.LogTiming.Fixed);
                AU.Debug.Log(_rTerrianHit.terrianNormal, AU.LogTiming.Fixed);
            }

            if (!_rTerrianHit.flags.grounded)
            {
                _terrianDirOld = Vector3.zero;
                _rTerrianHit.flags.groundedFlat = false;
                _rTerrianHit.terrianNormal = Vector3.up;
                _rTerrianHit.terrianRotation = Quaternion.identity;
                return;
            }

            bool terrianWalkale = _rayHit.normal.y > 0.8;
            _rTerrianHit.flags.groundedFlat = terrianWalkale;

            float verticalVel = Vector3.Dot(_rRb.velocity, _rayHit.normal);
            float x = _rayHit.distance - HoverHeight;
            float hoverForce = x * HoverSpringStrength + verticalVel * HoverDamperStrength;
            AU.Debug.Log(hoverForce, AU.LogTiming.Fixed);
            AU.Debug.Log(verticalVel, AU.LogTiming.Fixed);

            _rRb.AddForce(-_rayHit.normal * hoverForce, ForceMode.Acceleration);

            _rTerrianHit.terrianNormal = _rayHit.normal;
            _rTerrianHit.terrianRotation = Quaternion.FromToRotation(Vector3.up, _rTerrianHit.terrianNormal);

            _terrianDirOld = -_rayHit.normal;
        }
    }

}