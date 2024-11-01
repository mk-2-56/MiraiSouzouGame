using UnityEngine;
using UnityEngine.Splines;

public class SplineFollower : MonoBehaviour
{
    public SplineContainer splineContainer;
    public float speed = 5.0f;
    public CameraManager cameraManager;

    private float progress = 0.0f;
    private bool isSpline = false;

    // Update is called once per frame
    void Update()
    {
        if (isSpline)
        {
            FollowSpline();
        }

    }
    private void FollowSpline()
    {
        // �X�v���C���ɉ����ăv���C���[���ړ�
        progress += speed * Time.deltaTime / splineContainer.CalculateLength();
        progress = Mathf.Clamp01(progress);

        Vector3 currentPosition = splineContainer.EvaluatePosition(progress);
        transform.position = currentPosition;

        // �i�s�����Ɋ�Â��Č�����ݒ�
        float lookAheadDistance = 0.01f;
        float nextProgress = Mathf.Clamp01(progress + lookAheadDistance);
        Vector3 nextPosition = splineContainer.EvaluatePosition(nextProgress);
        Vector3 direction = (nextPosition - currentPosition).normalized;

        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }

        // �I�_�ɓ��B������ړ����~
        if (progress >= 1f)
        {
            EndSplineMovement();
        }
    }

    public void StartSplineMovement()
    {
        isSpline = true;
        progress = 0f;
    }

    public void EndSplineMovement()
    {
        isSpline = false;
        // ���̃X�N���v�g�Œʏ�ړ��ɖ߂��������s��
    }

}
