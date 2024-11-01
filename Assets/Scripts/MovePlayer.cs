using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    Vector3 moveForward;       //移動度・移動する力
    Rigidbody rb;
    public float speed = 15.0f / 2; //キャラクターの仮の移動スピード
    public KeyCode rightKey = KeyCode.D;
    public KeyCode leftKey = KeyCode.A;
    void Start()      
    {
        rb = this.GetComponent<Rigidbody>();
        moveForward = new Vector3(0.0f, 0.0f,0.0f);

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(rightKey))//右移動
        {
            moveForward.x = 1;
        }
        else if (Input.GetKey(leftKey)) //左移動
        {
            moveForward.x = -1;
        }
        else
        {
            moveForward.x = 0;

        }

        if(Input.GetKey(KeyCode.W))
        {
            moveForward.z = 1;
            transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
        }
        else if(Input.GetKey(KeyCode.S))
        {
            moveForward.z = -1;
            transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
        }
        else
        {
            moveForward.z = 0;

        }
    }
    private void FixedUpdate()
    {
        rb.AddForce(moveForward * speed);
    }
    private void OnCollisionEnter(Collision collision)
    {
         if(collision.gameObject.CompareTag("Coins"))
         {
            Destroy(collision.gameObject);      //プロトタイプ用に一応コインと当たったら破壊させる

         }
    }
}
