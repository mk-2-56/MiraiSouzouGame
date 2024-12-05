using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleporter : MonoBehaviour
{
    // �ړ���̈ʒu�����X�g�ŊǗ�
    [SerializeField]private List<Vector3> targetPositions = new List<Vector3>();
    private int currentTargetIndex = 0; // ���݂̈ړ���C���f�b�N�X

    void Update()
    {
        // T�{�^���������ꂽ�ꍇ
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (targetPositions.Count == 0)
            {
                Debug.LogWarning("Target positions list is empty.");
                return;
            }

            // Player�^�O�̂����I�u�W�F�N�g��T��
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                // ���݂̃^�[�Q�b�g�ʒu�Ɉړ�
                player.transform.position = targetPositions[currentTargetIndex];
                Debug.Log($"Player moved to {targetPositions[currentTargetIndex]}");

                // ���̃^�[�Q�b�g�ʒu�ɐi�ށi���[�v����j
                currentTargetIndex = (currentTargetIndex + 1) % targetPositions.Count;
            }
            else
            {
                Debug.LogWarning("No object with Player tag found.");
            }
        }
    }
}
