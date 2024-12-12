using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationControl : MonoBehaviour
{

    [SerializeField] private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            JumpTrigger();
            SetOnGround(false);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            SetOnGround(true);
        }

    }

    public void JumpTrigger()
    {
        animator.SetTrigger("Jump");
    }
    public void SetOnGround(bool b)
    {
        animator.SetBool("OnGround", b);
    }

    public void SetRightDrift(bool b)
    {
        animator.SetBool("RightDrift", b);
    }
    public void SetLeftDrift(bool b)
    {
        animator.SetBool("LeftDrift", b);
    }



}
