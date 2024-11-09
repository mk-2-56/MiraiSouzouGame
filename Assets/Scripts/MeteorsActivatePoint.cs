using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorsActivatePoint : MonoBehaviour
{
    private MeteorMovement meteor;
    private bool canActivate = true; // �A�N�e�B�x�[�g�\���ǂ����̃t���O
    private float waitTime = 10f; // �ҋ@����
    private float timer = 0f; // �^�C�}�[�p�̕ϐ�


    void Start()
    {
        // ���O�ŃI�u�W�F�N�g��������
        GameObject meteorSpawner = GameObject.Find("MeteorsSpawner");

        // MeteorMovement�R���|�[�l���g���擾
        meteor = meteorSpawner.GetComponent<MeteorMovement>();

        if (meteorSpawner == null)
        {
            Debug.LogError("Error: MeteorsSpawner not found.");
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canActivate)
        {
            // PlayerController�����������ꍇ�A�֐����Ăяo��
            if (meteor != null)
            {
                meteor.ActivateMeteors();
                canActivate = false; // �A�N�e�B�x�[�g�𖳌��ɂ���
            }
            else
            {
                Debug.Log("error");
            }

        }
    }

    private void Update()
    {
        // �ҋ@���̏ꍇ�A�^�C�}�[��i�߂�
        if (!canActivate)
        {
            timer += Time.deltaTime; // �^�C�}�[�����Z
            if (timer >= waitTime)
            {
                canActivate = true; // �A�N�e�B�x�[�g�\�ɖ߂�
                timer = 0f; // �^�C�}�[�����Z�b�g
            }
        }
    }
}
