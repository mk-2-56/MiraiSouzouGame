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
    //Debug:
    MsgBuffer _debugText;

    //Property:
    public bool Grounded
    { 
        get { return _grounded;}
    }
    public RaycastHit Groundhit
    { 
        get { return _rayHit;}
    }

    //Parameters:
    [SerializeField] float HoverHeight = 1.5f;
    [SerializeField] float HoverSpringStrength = 30.0f;
    [SerializeField] float HoverDamperStrength = 30.0f;

    //Memebers:
    Rigidbody _rRb;

    bool _grounded;
    RaycastHit _rayHit;

    void Start()
    {
        _rRb = GetComponent<Rigidbody>();
        _debugText = Debug.GetMsgBuffer();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        Vector3 rayDir = Vector3.down + _rRb.velocity * Time.fixedDeltaTime;
        //Vector3 rayDir = Vector3.down;
        float rayLength = HoverHeight + 3.0f;

        _grounded = Physics.Raycast(_rRb.position + Vector3.up, rayDir, out _rayHit, rayLength);

        if (_grounded)
        {
            float dTime = Time.fixedDeltaTime;

            float verticleVel = -_rRb.velocity.y;

            float x = _rayHit.distance - HoverHeight;

            float hoverForce = x * HoverSpringStrength - verticleVel * HoverDamperStrength;

            //debug.Msg = hoverForce.ToString("F4");
            _debugText.Text = "HoverForce: " + hoverForce.ToString("F4");

            _rRb.AddForce(Vector3.down * hoverForce, ForceMode.Acceleration);
        }
    }
}
