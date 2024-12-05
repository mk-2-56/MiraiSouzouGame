using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectDispatcher : MonoBehaviour
{
    public event System.Action DriftStartE;
    public event System.Action DriftEndE;

    public event System.Action BoostStartE;
    public event System.Action BoostEndE;

    public event System.Action JumpE;
    public event System.Action DashE;
    public event System.Action<float> SpeedE;

    public event System.Action LandedE;
    public event System.Action LifetedE;


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
            { 
                tmp.JumpEffect += DispatchJump;
                tmp.BoostEffectStart += BoostStart;
                tmp.BoostEffectEnd   += BoostEnd;
            }
        }
        {
            CC.Hover tmp = _rRoot.GetComponent<CC.Hover>();
            if (tmp)
                tmp.GroundedE  += DispatchGroundedEvent ;
                tmp.OffGroundE += DispatchOffGroundEvent;
        }
        {
            CC.Drift tmp = _rRoot.GetComponent<CC.Drift>();
            if (tmp)
                tmp.DriftEffect += DriftE;
        }
        {
            CC.Dash tmp = _rRoot.GetComponent<CC.Dash>();
            if (tmp)
                tmp.DashEffect += DispatchDash;
        }
    }

    void DriftE(bool flag)
    { 
        if(flag)
            DriftStartE?.Invoke();
        else
            DriftEndE?.Invoke();
    }
    void BoostStart()
    {
        BoostStartE?.Invoke();
    }
    void BoostEnd()
    {
        BoostEndE?.Invoke();
    }


    void DispatchSpeedEvent(float speed)
    {
        SpeedE?.Invoke(speed);
    }

    void DispatchDriftStart()
    {
        DriftStartE?.Invoke();

    }
    void DispatchDriftEnd()
    {
        DriftEndE?.Invoke();
    }

    void DispatchJump()
    {
        JumpE?.Invoke();
    }
    void DispatchDash()
    {
        DashE?.Invoke();
    }
    void DispatchGroundedEvent()
    {
        LandedE?.Invoke();
    }
    void DispatchOffGroundEvent()
    {
        LifetedE?.Invoke();
    }
}
