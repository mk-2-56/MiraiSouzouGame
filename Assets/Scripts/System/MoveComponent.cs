using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveComponent : MonoBehaviour
{
    private Rigidbody rd;
    private Vector3 dir;
    [SerializeField] private float speed = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        rd = GetComponent<Rigidbody>();
        dir = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (rd == null)
        {
            TfMove();
        }
        else
        {
            RbMove();
        }
    }

    private void TfMove()
    {
        transform.position += dir * speed * Time.deltaTime;
    }

    private void RbMove()
    {
        rd.MovePosition(dir * speed * Time.deltaTime);
    }

    //à⁄ìÆï˚å¸ê›íË
    public void SetDirection(Vector3 dir)
    {
        this.dir = dir;
    }
}
