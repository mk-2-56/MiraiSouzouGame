using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCT_WallRide : MonoBehaviour
{


    Transform _rCog;
    Transform _rFacing;

    Quaternion _terrianRotation;

    // Start is called before the first frame update
    void Start()
    {
        _rFacing = transform.Find("Facing");
        _rCog = _rFacing.Find("Cog");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void WallCheck()
    {
        //temp
        LayerMask layerMask = LayerMask.GetMask("Terrian");
        float radius = 1.0f;
        float distance = 1.0f;
        RaycastHit hit;

        if (!Physics.SphereCast(_rCog.transform.position, radius, _rFacing.forward,
            out hit, distance, layerMask, QueryTriggerInteraction.Ignore))
            return;

        if (hit.normal.y < 0.2f)
        {
            _terrianRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
        }
    }
}
