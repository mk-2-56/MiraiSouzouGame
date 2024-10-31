using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorKinematicFalser : MonoBehaviour
{
    private Vector3[] originalPositions; // ���̈ʒu��ۑ�����z��
    private Quaternion[] originalRotations; // ���̉�]��ۑ�����z��
    private Vector3[] originalVelocities; // ���̑��x��ۑ�����z��
    private Vector3[] originalAngularVelocities; // ���̊p���x��ۑ�����z��

    void Start()
    {
        // �q�I�u�W�F�N�g�̌��̈ʒu���L�^
        Rigidbody[] childRbs = GetComponentsInChildren<Rigidbody>();
        originalPositions = new Vector3[childRbs.Length];
        originalRotations = new Quaternion[childRbs.Length];
        originalVelocities = new Vector3[childRbs.Length];
        originalAngularVelocities = new Vector3[childRbs.Length];

        for (int i = 0; i < childRbs.Length; i++)
        {
            originalPositions[i] = childRbs[i].transform.position;
            originalRotations[i] = childRbs[i].transform.rotation; // ��]���L�^
            originalVelocities[i] = childRbs[i].velocity; // ���x���L�^
            originalAngularVelocities[i] = childRbs[i].angularVelocity; // �p���x���L�^
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // �v���C���[�ƏՓ˂��������m�F
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Collision is ok");
            // �q�I�u�W�F�N�g�̂��ׂĂ�Rigidbody���擾
            Rigidbody[] childRbs = GetComponentsInChildren<Rigidbody>();

            foreach (Rigidbody childRb in childRbs)
            {
                Debug.Log("foreach is ok");
                // isKinematic��false�ɂ���
                if (childRb != null)
                {
                    childRb.isKinematic = false;

                    // ��юU�点��
                    var random = new System.Random();
                    var min = -3;
                    var max = 3;
                    var vect = new Vector3(random.Next(min, max), random.Next(0, max), random.Next(min, max));

                    childRb.AddForce(vect, ForceMode.Impulse);
                    childRb.AddTorque(vect, ForceMode.Impulse);
                }
            }
        }
    }

    void Update()
    {
        // ��\���̏ꍇ�AisKinematic��true�ɖ߂��A���̈ʒu�ɖ߂�
        if (!gameObject.activeInHierarchy)
        {
            ResetKinematic();
        }
    }

    public void ResetKinematic()
    {
        Debug.Log("reset is ok");
        Rigidbody[] childRbs = GetComponentsInChildren<Rigidbody>();
        for (int i = 0; i < childRbs.Length; i++)
        {
            if (childRbs[i] != null)
            {
                // ���̈ʒu�ɖ߂�
                childRbs[i].transform.position = originalPositions[i];
                // ��]�����̉�]�ɖ߂�
                childRbs[i].transform.rotation = originalRotations[i];
                // �������͂����Z�b�g����
                childRbs[i].velocity = originalVelocities[i]; // ���̑��x�ɖ߂�
                childRbs[i].angularVelocity = originalAngularVelocities[i]; // ���̊p���x�ɖ߂�

                // isKinematic��true�ɖ߂�
                childRbs[i].isKinematic = true;
            }
        }
    }
}
