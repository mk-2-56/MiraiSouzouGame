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



public class CCHover : MonoBehaviour
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

    CC.TerrianHit _rTerrianHit;

    void Start()
    {
        _rRb = GetComponent<Rigidbody>();
        _rFacing = transform.Find("Facing");
        _rCog    = _rFacing.Find("Cog");



        _rTerrianHit = GetComponent<CC.Basic>().GetTerrianHit();
        _layerMask = LayerMask.GetMask("Terrian");
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        Vector3 rayDir = Vector3.down + _rRb.velocity * Time.fixedDeltaTime;
        //Vector3 rayDir = Vector3.down;
        float rayLength = HoverHeight + 1.0f;

        _rTerrianHit.grounded = Physics.Raycast(_rRb.position + Vector3.up, rayDir, out _rayHit, rayLength, _layerMask);

        if (_rTerrianHit.grounded)
        {
            float dTime = Time.fixedDeltaTime;

            float verticalVel = _rRb.velocity.y;
            float x = _rayHit.distance - HoverHeight;
            float hoverForce = x * HoverSpringStrength + verticalVel * HoverDamperStrength;

            _rRb.AddForce(Vector3.down * hoverForce, ForceMode.Acceleration);
        }

        bool terrianWalkale   = _rayHit.normal.y > 0.5;
        _rTerrianHit.grounded = terrianWalkale;

        AU.Debug.Log(terrianWalkale, AU.LogTiming.Fixed);
        AU.Debug.Log(_rTerrianHit.terrianNormal, AU.LogTiming.Fixed);

        _rTerrianHit.terrianNormal = _rayHit.normal;
        _rTerrianHit.terrianRotation = Quaternion.identity;

        _rRb.useGravity = !_rTerrianHit.grounded;

        if (_rTerrianHit.grounded)
        {
            _rTerrianHit.terrianRotation = Quaternion.FromToRotation(Vector3.up, _rTerrianHit.terrianNormal);

            _rCog.position = _rayHit.point;
            _rCog.rotation = _rTerrianHit.terrianRotation * _rFacing.rotation;
        }
    }
}
