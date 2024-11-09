using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePlayer : MonoBehaviour
{
    public float moveSpeed = 5f; // �ړ����x
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        // ���͂��擾
        float horizontal = Input.GetAxis("Horizontal"); // A/D �܂��� ��/�E�L�[
        float vertical = Input.GetAxis("Vertical");     // W/S �܂��� ��/���L�[

        // �J�����̑O���ƉE��������ɂ���
        Vector3 forward = mainCamera.transform.forward;
        Vector3 right = mainCamera.transform.right;

        // �����ʂɂ�����ړ������ɐ���
        forward.y = 0;
        right.y = 0;

        // ���K�����ĕ����x�N�g���𓾂�
        forward.Normalize();
        right.Normalize();

        // ���͂Ɋ�Â��Ĉړ�����������
        Vector3 moveDirection = (forward * vertical + right * horizontal).normalized;

        // �ړ���K�p
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }
}
