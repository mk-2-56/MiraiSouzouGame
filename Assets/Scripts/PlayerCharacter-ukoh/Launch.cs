using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launch : MonoBehaviour
{
    [SerializeField] float param_LaunchSpeed = 90.0f;
    [SerializeField] Transform testTarget;

    Rigidbody _rRb;
    CCT_Basic _rMovementContoller;

    void Start()
    {
        _rRb = GetComponent<Rigidbody>();
        _rMovementContoller = GetComponent<CCT_Basic>();
        _rMovementContoller.ResigterLaunchModule(new LaunchCommand(this));
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
            _launchModule.LaunchPlayerTowards(_launchModule.testTarget.position);
            //throw new System.NotImplementedException();
        }
    }

    void LaunchPlayerTowards(Vector3 pos)
    {
        //TestingTarget

        ResetSpeed();
        _rRb.useGravity = false;
        _rRb.AddForce((pos - _rRb.position ).normalized * param_LaunchSpeed, ForceMode.VelocityChange);
        Invoke("EndLaunching", 0.8f);
    }

    void EndLaunching()
    {
        _rRb.useGravity = true;
    }

    void ResetSpeed()
    {
        _rRb.velocity = Vector3.zero;
    }
}
