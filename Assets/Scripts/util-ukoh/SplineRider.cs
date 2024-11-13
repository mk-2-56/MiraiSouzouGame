using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;

public class SplineRider : MonoBehaviour
{
    //[SerializeField] float param_speed;

    [SerializeField] SplineContainer param_container;
    
    MsgBuffer _debug;

    bool inUse;
    GameObject _rider;
    Rigidbody _rRiderRB;

    float _t;
    int _resolution;

    LineRenderer _lineRenderer;

    Vector3 debug_resultPosition;
    Vector3 debug_resultDirection;

    // Start is called before the first frame update
    void Start()
    {
        _resolution = (int)param_container.CalculateLength() / 2;
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Awake()
    {
        _debug = AU.Debug.GetMsgBuffer();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _lineRenderer.enabled = inUse;

        _debug.FixedText = "InRange:" + inUse.ToString();

        if(!inUse)
            return;

        _t += Time.fixedDeltaTime;

        float t;
        float3 outPosition;
        SplineUtility.GetNearestPoint(
            param_container.Spline, _rider.transform.position - transform.position,
            out outPosition, out t, _resolution, 4);
        Vector3 direction = Vector3.Normalize(param_container.EvaluateTangent(t));
        //Vector3 direction = Vector3.Normalize(param_container.EvaluateAcceleration(t));

        debug_resultPosition = (Vector3)outPosition;
        debug_resultDirection = debug_resultPosition + direction;
        _lineRenderer.SetPosition(0, debug_resultPosition);
        _lineRenderer.SetPosition(1, _rider.transform.position);
        _lineRenderer.SetPosition(2, _rider.transform.position + 12 * direction);

        //_rRiderRB.velocity = _rRiderRB.velocity.magnitude * direction;

        float directionDot = Vector3.Dot(direction, _rRiderRB.velocity);
        bool applyForce = directionDot < 60.0f;
        _debug.FixedText += "\tApplyingForce: " + applyForce.ToString();
        if (applyForce)
        { 
            float forceMag = 60.0f * ( 3 - 2 * Vector3.Dot(direction, _rRiderRB.velocity.normalized));
            _rRiderRB.AddForce(direction * forceMag, ForceMode.Acceleration);
        }
    }


    void OnTriggerEnter(Collider other)
    { 
        _rider = other.gameObject;
        _rRiderRB = _rider.GetComponent<Rigidbody>();
        inUse = true;
    }
    void OnTriggerExit()
    {
        inUse = false;
    }
}
