using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimationControl : MonoBehaviour
{

    [SerializeField] private Animator animator;

    public event System.Action LandAnimE;
    public event System.Action LifetedE;

    public event System.Action DriftRightStartAnimE;
    public event System.Action DriftRightEndAnimE;

    public event System.Action DriftLeftStartAnimE;
    public event System.Action DriftLeftEndAnimE;

    public event System.Action BigJumpAnimE;

    // Start is called before the first frame update
    private void Start()
    {
        GameObject _rRoot = transform.parent.parent.parent.gameObject;

        {//Hub
           
        }
        {//OnGround
            CC.Hover tmp = _rRoot.GetComponent<CC.Hover>();
            if (tmp)
            {
                tmp.GroundedE += DispatchGroundedEvent;
                tmp.OffGroundE += DispatchOffGroundEvent;
            }
            LandAnimE += SetOnGround;
            LifetedE += SetOffGround;
        }

        {
            BigJumpAnimE += BigJumpTrigger;
        }

        {//Drift
            CC.Drift tmp = _rRoot.GetComponent<CC.Drift>();
            if (tmp)
                tmp.DriftEffect += DriftE;
            DriftRightStartAnimE += SetRightDrift;
            DriftRightEndAnimE += SetRightDriftFalse;
            DriftLeftStartAnimE += SetLeftDrift;
            DriftLeftEndAnimE += SetLeftDriftFalse;
        }
    }

    //Drift
    private void DriftE(bool flag)
    {
        GameObject _rRoot = transform.parent.parent.parent.gameObject;
        CC.Hub tmp = _rRoot.GetComponent<CC.Hub>();
        Vector2 dir = tmp.moveRawInput;

        Debug.Log("Player‚ÌmoveRawInputX: "+ dir.x.ToString());

        if (flag)
        {//ƒhƒŠƒtƒg‚µ‚Ä‚¢‚é
            if (dir.x > 0f)            
                DriftRightStartAnimE?.Invoke();            
            else           
                DriftLeftStartAnimE?.Invoke();
        }
        else
        {
                DriftRightEndAnimE?.Invoke();
                DriftLeftEndAnimE?.Invoke();
        }    
    }


    private void DispatchDriftStart()
    {
        DriftRightStartAnimE?.Invoke();
    }
    private void DispatchDriftEnd()
    {
        DriftRightEndAnimE?.Invoke();
    }

    //BigJump
    public void DispatchBigJump()
    {
        DispatchGroundedEvent();
        BigJumpAnimE?.Invoke();
    }
    //Ground?
    private void DispatchGroundedEvent()
    {
        LandAnimE?.Invoke();
    }

    public void DispatchOffGroundEvent()
    {
        LifetedE?.Invoke();
    }

    // Update is called once per frame
    private void Update()
    {
       /* if (Input.GetKeyDown(KeyCode.J))
        {
            SetOnGround();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            SetOffGround();
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            SetRightDrift(true);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            SetRightDrift(false);
        }*/

    }


    public void BigJumpTrigger()
    {
        animator.SetTrigger("BigJump");
    }
    public void SetOnGround()
    {
        animator.SetBool("OnGround", true);
    }
    public void SetOffGround()
    {
        animator.SetBool("OnGround", false);
    }

    public void SetRightDrift()
    {
        animator.SetBool("DriftRight", true);
    }

    public void SetRightDriftFalse()
    {
        animator.SetBool("DriftRight", false);
    }

    public void SetRightDrift(bool b)
    {
        animator.SetBool("DriftRight", b);
    }

    public void SetLeftDrift()
    {
        animator.SetBool("DriftLeft", true);
    }

    public void SetLeftDriftFalse()
    {
        animator.SetBool("DriftLeft", false);
    }

    public void SetLeftDrift(bool b)
    {
        animator.SetBool("DriftLeft", b);
    }




}

