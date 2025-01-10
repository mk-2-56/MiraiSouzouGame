using UnityEngine;

public class Mark : MonoBehaviour
{
    public enum MarkState
    {
        Waiting,    // �}�[�N���ҋ@��
        Active,     // �}�[�N���\�����i�Ǐ]���j
        Ending      // �}�[�N���I����ԁi��\���ɂȂ鏀���j
    }

    [Header("Settings")]
    [SerializeField] private Vector3 offset = Vector3.up * 2f; // �^�[�Q�b�g����̃I�t�Z�b�g�ʒu
    [SerializeField] private float waitAppear = 0.5f;         // �\���܂ł̑ҋ@����

    private MarkState markState = MarkState.Waiting;          // ���݂̏��
    private Transform targetObject;                          // �Ǐ]����^�[�Q�b�g
    private Camera mainCamera;                               // ���C���J����
    private float stateTimer = 0f;                           // �X�e�[�g�p�̃^�C�}�[
    private RectTransform rectTransform;                     // �}�[�N�� RectTransform
    private RectTransform canvasRectTransform;               // �e Canvas �� RectTransform

    /// <summary>
    /// ������
    /// </summary>
    public void Initialize(Transform target, Camera camera)
    {
        targetObject = target;
        mainCamera = camera;
        stateTimer = 0f;
        markState = MarkState.Waiting;

        // RectTransform ���擾
        rectTransform = GetComponent<RectTransform>();
        canvasRectTransform = rectTransform.parent.GetComponent<RectTransform>();

        gameObject.SetActive(true); // �}�[�N���A�N�e�B�u��
    }

    public void UpdateMark()
    {
        if (targetObject == null || mainCamera == null)
        {
            ResetMark();
            return;
        }

        // �^�[�Q�b�g�ɒǏ]���A�J���������Ɍ�����
        FollowTarget();

        // �X�e�[�g���Ƃ̏���
        stateTimer += Time.deltaTime;
        switch (markState)
        {
            case MarkState.Waiting:
                HandleWaitingState();
                break;

            case MarkState.Active:
                HandleActiveState();
                break;

            case MarkState.Ending:
                ResetMark();
                break;
        }
    }

    /// <summary>
    /// �^�[�Q�b�g��Ǐ]
    /// </summary>
    private void FollowTarget()
    {
        // �^�[�Q�b�g�̏�̃��[���h���W���擾
        Vector3 worldPosition = targetObject.position + offset;

        // ���[���h���W���X�N���[�����W�ɕϊ�
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(worldPosition);

        // �X�N���[�����W�� Canvas ���̃��[�J�����W�ɕϊ�
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRectTransform, screenPosition, mainCamera, out Vector2 localPosition))
        {
            rectTransform.anchoredPosition = localPosition; // �}�[�N�̈ʒu���X�V
        }
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
    /// �A�N�e�B�u�ȏ�Ԃ̏����i���ʂȉ��o�Ȃǂ������Œǉ��\�j
    /// </summary>
    private void HandleActiveState()
    {
        // �K�v�ɉ����āA�}�[�N�̃A�N�e�B�u��Ԃł̏������L�q
    }

    /// <summary>
    /// �}�[�N�����Z�b�g�i��A�N�e�B�u���j
    /// </summary>
    public void ResetMark()
    {
        targetObject = null;
        markState = MarkState.Waiting;
        stateTimer = 0f;
        gameObject.SetActive(false); // �v�[���ɖ߂��ۂɔ�A�N�e�B�u��
    }

    /// <summary>
    /// �}�[�N���g�p�\���ǂ����𔻒�
    /// </summary>
    /// <returns>�}�[�N���g�p�\���ǂ���</returns>
    public bool IsUsable()
    {
        return markState == MarkState.Active;
    }
}