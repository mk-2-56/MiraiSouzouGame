using System.Collections.Generic;
using UnityEngine;

public class MarkManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private ObjectPool markPool; // �}�[�N�p�̃I�u�W�F�N�g�v�[��
    [SerializeField] private Transform worldSpaceCanvas; // �}�[�N��\������World Space Canvas
    [SerializeField] private Camera mainCamera; // �v���C���[�J����

    private List<Mark> activeMarks = new List<Mark>(); // �A�N�e�B�u�ȃ}�[�N���X�g

    private void Start()
    {
        // �J�����̎����ݒ�
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            if (mainCamera == null)
            {
                Debug.LogError("Main Camera is not assigned and could not be found!");
                return;
            }
        }

        // World Space Canvas �̃`�F�b�N
        if (worldSpaceCanvas == null)
        {
            Debug.LogError("World Space Canvas is not assigned!");
        }
    }

    private void Update()
    {
        // �A�N�e�B�u�ȃ}�[�N���X�V
        for (int i = activeMarks.Count - 1; i >= 0; i--)
        {
            Mark mark = activeMarks[i];
            if (mark != null)
            {
                mark.UpdateMark();
            }
            else
            {
                activeMarks.RemoveAt(i); // Null������΃��X�g����폜
            }
        }
    }

    /// <summary>
    /// �}�[�N���^�[�Q�b�g�ɒǉ�����
    /// </summary>
    /// <param name="target">�^�[�Q�b�g��Transform</param>
    /// <returns>�ǉ�����Mark�I�u�W�F�N�g</returns>
    public Mark AddMark(Transform target)
    {
        GameObject markObject = markPool.GetObject();
        if (markObject == null)
        {
            Debug.LogWarning("No available objects in the pool!");
            return null;
        }

        // Canvas �̎q�Ƃ��Ĕz�u
        markObject.transform.SetParent(worldSpaceCanvas, false);

        // Mark �R���|�[�l���g��������
        Mark mark = markObject.GetComponent<Mark>();
        if (mark != null)
        {
            mark.Initialize(target, mainCamera);
            activeMarks.Add(mark);
        }
        else
        {
            Debug.LogError("The object from the pool does not have a Mark component!");
            markPool.ReturnObject(markObject);
        }
        return mark;
    }

    /// <summary>
    /// �}�[�N���폜���ăv�[���ɖ߂�
    /// </summary>
    /// <param name="mark">�폜����Mark�I�u�W�F�N�g</param>
    public void RemoveMark(Mark mark)
    {
        if (mark == null || !activeMarks.Contains(mark)) return;

        mark.ResetMark(); // �}�[�N�����Z�b�g
        markPool.ReturnObject(mark.gameObject); // �v�[���ɕԋp
        activeMarks.Remove(mark); // �A�N�e�B�u���X�g����폜
    }

   
}