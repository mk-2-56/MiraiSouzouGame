using System.Collections.Generic;
using UnityEngine;

public class MeteorsManager : MonoBehaviour
{
    [Header("Meteor Settings")]
    [SerializeField] private ObjectPool meteorPool;         // 覐΂̃I�u�W�F�N�g�v�[��
    [SerializeField] private Transform[] spawnPositions;    // �X�|�[���n�_���w�肷��Transform�z��
    [SerializeField] private float spawnHeight = 50f;       // �X�|�[�����鍂��
    [SerializeField] private float fallSpeed = 10f;         // �������x
    [SerializeField] private float groundY = 0f;            // �n�ʂ̍����i覐΂���\���ɂȂ鍂���j
    private List<GameObject> activeMeteors = new List<GameObject>(); // ���݃A�N�e�B�u��覐΃��X�g

    private void Update()
    {
        // �A�N�e�B�u��覐΂�����
        for (int i = activeMeteors.Count - 1; i >= 0; i--)
        {
            GameObject meteor = activeMeteors[i];
            meteor.transform.position += Vector3.down * fallSpeed * Time.deltaTime;

            // 覐΂��n�ʂɓ��B������v�[���ɖ߂�
            if (meteor.transform.position.y <= groundY)
            {
                ResetMeteor(meteor);
            }
        }
    }

    public void ActivateMeteors()
    {
        // �e�X�|�[���n�_��覐΂�z�u���ăA�N�e�B�u������
        foreach (Transform spawnPoint in spawnPositions)
        {
            Vector3 spawnPosition = spawnPoint.position;
            spawnPosition.y += spawnHeight; // �X�|�[��������ǉ�

            GameObject meteor = meteorPool?.GetObject(spawnPosition, Quaternion.identity);
            activeMeteors.Add(meteor); // �A�N�e�B�u��覐΃��X�g�ɒǉ�
            //markManager.AddMark(meteor.transform);
        }
    }

    /// <summary>
    /// 覐΂��A�N�e�B�u�����ăv�[���ɖ߂�
    /// </summary>
    private void ResetMeteor(GameObject meteor)
    {
        //if (markManager != null)
        //{
        //    Mark mark = meteor.GetComponentInChildren<Mark>();
        //    if (mark != null)
        //    {
        //        markManager.RemoveMark(mark);
        //    }
        //}

        meteor.SetActive(false); // ��A�N�e�B�u��
        activeMeteors.Remove(meteor); // ���X�g����폜
        meteorPool?.ReturnObject(meteor); // �v�[���ɖ߂�
    }

    // �V�[���r���[�ɃX�|�[���ʒu�����o��
    private void OnDrawGizmos()
    {
        if (spawnPositions == null) return;

        Gizmos.color = Color.red;
        foreach (Transform spawnPoint in spawnPositions)
        {
            if (spawnPoint != null)
            {
                Gizmos.DrawSphere(new Vector3(spawnPoint.position.x, spawnPoint.position.y + spawnHeight, spawnPoint.position.z), 1.0f);
                Gizmos.DrawLine(spawnPoint.position, new Vector3(spawnPoint.position.x, spawnPoint.position.y + spawnHeight, spawnPoint.position.z));
            }
        }
    }
}