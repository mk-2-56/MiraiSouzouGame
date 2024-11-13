using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launch : MonoBehaviour
{

    [SerializeField] float param_LaunchSpeed = 90.0f;
    [SerializeField] ActivateShockWave param_Effct;
    //[SerializeField] TargetManager param_TargetManager;
    TargetManager _targetManager;

    Rigidbody _rRb;
    CCT_Basic _rMovementContoller;

    void Start()
    {
        _rRb = GetComponent<Rigidbody>();
        _rMovementContoller = GetComponent<CCT_Basic>();
        _rMovementContoller.ResigterLaunchModule(new LaunchCommand(this));
        _targetManager = GameObject.Find("TestingTargets").GetComponent<TargetManager>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public class LaunchCommand : Command
    {
        public LaunchCommand(Launch module) { _launchModule = module;}

        Launch _launchModule;
        public override void Execute()
        {
            Vector3 target;
            Vector3 positionOrigin = _launchModule._rRb.position;
            Vector3 inputDirection = _launchModule._rMovementContoller.InputDirectionWorld;
            if (_launchModule._targetManager.FindBestTarget(positionOrigin, inputDirection, out target))
            _launchModule.LaunchPlayerTowards(target);
            //throw new System.NotImplementedException();
        }
    }


    void LaunchPlayerTowards(Vector3 pos)
    {
        //TestingTarget

        ResetSpeed();
        _rRb.useGravity = false;
        _rRb.AddForce((pos - _rRb.position ).normalized * param_LaunchSpeed, ForceMode.VelocityChange);
        float airTime = (pos - _rRb.position).magnitude / param_LaunchSpeed;
        param_Effct.Execute();

        Invoke("EndLaunching", airTime);
    }

    void EndLaunching()
    {
        _rRb.useGravity = true;
        _rRb.velocity += Vector3.down * _rRb.velocity.y;
    }

    void ResetSpeed()
    {
        _rRb.velocity = Vector3.zero;
    }

}
