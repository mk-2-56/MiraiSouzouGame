using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    Vector3 moveForward;       //�ړ��x�E�ړ������
    Rigidbody rb;
    public float speed = 15.0f / 2; //�L�����N�^�[�̉��̈ړ��X�s�[�h
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
        if(Input.GetKey(rightKey))//�E�ړ�
        {
            moveForward.x = 1;
        }
        else if (Input.GetKey(leftKey)) //���ړ�
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
            Destroy(collision.gameObject);      //�v���g�^�C�v�p�Ɉꉞ�R�C���Ɠ���������j�󂳂���

         }
    }
}
