using UnityEngine;

public class Mark : MonoBehaviour
{
    public enum MarkState
    {
        Waiting,    // �}�[�N���ҋ@��
        Active,     // �}�[�N���\�����i�A�j���[�V�������j
        Expanding,  // �A�j���[�V�����I����
        Ending      // �}�[�N���I����ԁi��\���ɂȂ鏀���j
    }

    [Header("References")]
    [SerializeField] private GameObject mark;   // �O�p�}�[�N�� GameObject�i�v���n�u�̎q�j

    [Header("Settings")]
    [SerializeField] private float waitAppear = 0.5f;   // �\���܂ł̑ҋ@����
    [SerializeField] private float expandDuration = 1.5f; // �g��A�j���[�V�����̎���
    [SerializeField] private Vector3 initialScale = new Vector3(2f, 2f, 2f); // �����X�P�[��
    [SerializeField] private Vector3 targetScale = new Vector3(1f, 1f, 1f);   // �ŏI�X�P�[��
    [SerializeField] private Vector3 offset = Vector3.up * 2f; // �^�[�Q�b�g����̃I�t�Z�b�g�ʒu

    private MarkState markState = MarkState.Waiting;    // ���݂̏��
    private Transform targetObject;                    // �Ǐ]����^�[�Q�b�g
    private Camera mainCamera;                         // �v���C���[�J����
    private float stateTimer = 0f;                     // �X�e�[�g�̌o�ߎ���

    /// <summary>
    /// �}�[�N��������
    /// </summary>
    /// <param name="target">�Ǐ]����^�[�Q�b�g</param>
    /// <param name="camera">�Ǐ]����J����</param>
    public void Initialize(Transform target, Camera camera)
    {
        targetObject = target;
        mainCamera = camera;
        markState = MarkState.Waiting;
        stateTimer = 0f;
        transform.localScale = initialScale;
        gameObject.SetActive(true);
    }

    /// <summary>
    /// �}�[�N�̏�Ԃ��X�V
    /// </summary>
    public void UpdateMark()
    {
        if (targetObject == null || mainCamera == null)
        {
            ResetMark();
            return;
        }

        // �^�[�Q�b�g�ɒǏ]���J��������������
        FollowTarget();

        // ���݂̃X�e�[�g�ɉ����ď�����؂�ւ�
        stateTimer += Time.deltaTime;
        switch (markState)
        {
            case MarkState.Waiting:
                HandleWaitingState();
                break;

            case MarkState.Active:
                HandleActiveState();
                break;

            case MarkState.Expanding:
                HandleExpandingState();
                break;

            case MarkState.Ending:
                HandleEndingState();
                break;
        }
    }

    /// <summary>
    /// �^�[�Q�b�g��Ǐ]���J��������������
    /// </summary>
    private void FollowTarget()
    {
        transform.position = targetObject.position + offset;
        transform.LookAt(mainCamera.transform);
    }

    /// <summary>
    /// �ҋ@���̃X�e�[�g����
    /// </summary>
    private void HandleWaitingState()
    {
        if (stateTimer >= waitAppear)
        {
            stateTimer = 0f;
            markState = MarkState.Active;
        }
    }

    /// <summary>
    /// �A�N�e�B�u��Ԃ̏����i�g��A�j���[�V�����j
    /// </summary>
    private void HandleActiveState()
    {
        transform.localScale = Vector3.Lerp(initialScale, targetScale, stateTimer / expandDuration);
        if (stateTimer >= expandDuration)
        {
            stateTimer = 0f;
            markState = MarkState.Expanding;
        }
    }

    /// <summary>
    /// �g���̃X�e�[�g����
    /// </summary>
    private void HandleExpandingState()
    {
        // �����I�ɕK�v�Ȓǉ������������ɋL�q
        // ��: ���̃G�t�F�N�g���ԊǗ�
    }

    /// <summary>
    /// �I�����̃X�e�[�g����
    /// </summary>
    private void HandleEndingState()
    {
        ResetMark();
    }

    /// <summary>
    /// �}�[�N�����Z�b�g�i��A�N�e�B�u���j
    /// </summary>
    public void ResetMark()
    {
        targetObject = null;
        markState = MarkState.Waiting;
        stateTimer = 0f;
        gameObject.SetActive(false);
    }

    /// <summary>
    /// �}�[�N���g�p�\���𔻒�
    /// </summary>
    /// <returns>�}�[�N���g�p�\���ǂ���</returns>
    public bool IsUsable()
    {
        return markState == MarkState.Active;
    }
}