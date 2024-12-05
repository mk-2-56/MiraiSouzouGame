using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectDispatcher : MonoBehaviour
{
    public event System.Action DriftStart;
    public event System.Action DriftEnd;

    public event System.Action HandleJumpE;
    public event System.Action HandleDashE;
    public event System.Action<float> HandleSpeedE;



    // Start is called before the first frame update
    void Start()
    {
        GameObject _rRoot = transform.parent.parent.parent.gameObject;

        {
            CC.Hub tmp = _rRoot.GetComponent<CC.Hub>();
            if (tmp)
                tmp.SpeedEffect += DispatchSpeedEvent;
        }
        {
            CC.Basic tmp = _rRoot.GetComponent<CC.Basic>();
            if (tmp)
                tmp.JumpEffect += HandleJumpE;
        }
        {
            CC.Drift tmp = _rRoot.GetComponent<CC.Drift>();
            if (tmp)
                tmp.DriftEffect += HandleDriftE;
        }
        {
            CC.Dash tmp = _rRoot.GetComponent<CC.Dash>();
            if (tmp)
                tmp.DashEffect += HandleDashE;
        }
    }

    void HandleDriftE(bool flag)
    { 
        if(flag)
            DriftStart?.Invoke();
        else
            DriftEnd?.Invoke();
    }
    void DispatchSpeedEvent(float speed)
    {
        HandleSpeedE?.Invoke(speed);
    }
}
