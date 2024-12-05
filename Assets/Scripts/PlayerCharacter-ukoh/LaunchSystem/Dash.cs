using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CC
{ 
    public class Dash : MonoBehaviour
    {
        public event System.Action DashEffect;

        [SerializeField] float param_DashSpeed = 90.0f;
        TargetManager _targetManager;
    
        Rigidbody _rRb;
        PlayerMovementParams _rMovementParams;
    
        void Start()
        {
            _rRb = GetComponent<Rigidbody>();
            _rMovementParams = GetComponent<CC.Basic>().GetPlayerMovementParams();
            _targetManager = this.GetComponent<TargetManager>();

            CC.Hub CChub = GetComponent<CC.Hub>();
            CChub.DashEvent += HandleDash;
        }
    
        // Update is called once per frame
        void Update()
        {

        }
        
        void HandleDash()
        {
            Vector3 target;
            Vector3 positionOrigin = _rRb.position;
            Vector3 inputDirection = _rMovementParams.inputs.inputDirection;
            bool hasTarget = _targetManager.FindBestTarget(positionOrigin, inputDirection, out target);
            if (hasTarget)
            { 
                DashPlayerTowards(target);
                DashEffect?.Invoke();
            }
        }
    
        void DashPlayerTowards(Vector3 pos)
        {
            //TestingTarget
    
            ResetSpeed();
            _rRb.useGravity = false;
            _rRb.AddForce((pos - _rRb.position ).normalized * param_DashSpeed, ForceMode.VelocityChange);
            float airTime = (pos - _rRb.position).magnitude / param_DashSpeed;
    
            Invoke("EndDashing", airTime);
        }
    
        void EndDashing()
        {
            _rRb.useGravity = true;
            _rRb.velocity += Vector3.down * _rRb.velocity.y;
        }
    
        void ResetSpeed()
        {
            _rRb.velocity = Vector3.zero;
        }
    }
}