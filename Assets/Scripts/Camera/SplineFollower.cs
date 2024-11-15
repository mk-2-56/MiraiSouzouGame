using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class SplineFollower : MonoBehaviour
{
    public SplineContainer splineContainer;
    public float speed = 5.0f;

    private List<Transform> targets = new List<Transform>();      // �����̃v���C���[�̃^�[�Q�b�g
    private List<float> progresses = new List<float>();           // �e�v���C���[�̐i�s�x
    private List<PlayerSplineHandler> playerHandlers = new List<PlayerSplineHandler>(); // �v���C���[�̃X�v���C���n���h��

    private bool isSpline = false;  //�ړ����H

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i] != null)
            {
                FollowSpline(i);
            }
        }
    }
    private void FollowSpline(int index)
    {
        // �X�v���C���ɉ����ăv���C���[���ړ�
        progresses[index] += speed * Time.deltaTime / splineContainer.CalculateLength();
        progresses[index] = Mathf.Clamp01(progresses[index]); // �i�s�x��0�`1�ɃN�����v

        // �X�v���C����̈ʒu���v�Z���A�^�[�Q�b�g�ɓK�p
        Vector3 currentPosition = splineContainer.EvaluatePosition(progresses[index]);
        targets[index].position = currentPosition;

        // �����̐ݒ�
        float lookAheadDistance = 0.01f;
        float nextProgress = Mathf.Clamp01(progresses[index] + lookAheadDistance);
        Vector3 nextPosition = splineContainer.EvaluatePosition(nextProgress);
        Vector3 direction = (nextPosition - currentPosition).normalized;

        if (direction != Vector3.zero)
        {
            targets[index].rotation = Quaternion.LookRotation(direction); // �^�[�Q�b�g�̌�����i�s�����ɐݒ�
        }

        // �I�_�ɓ��B������ړ����~
        if (progresses[index] >= 1f)
        {
            EndSplineMovement(index);
        }
    }

    public void StartSplineMovement(Transform newTarget)
    {
        targets.Add(newTarget); // �ړ��Ώۂ̃^�[�Q�b�g��ݒ�
        progresses.Add(0f);
        playerHandlers.Add(newTarget.GetComponent<PlayerSplineHandler>());
/*        isSpline = true;
*/    }

    public void EndSplineMovement(int index)
    {
        // �w��̃^�[�Q�b�g�̃X�v���C���ړ����I�����A���X�g����폜
        if (playerHandlers[index] != null)
        {
            playerHandlers[index].OnSplineMovementEnd(); // �X�v���C���ړ��I����ʒm
        }
        targets.RemoveAt(index);//���̊֐�������
        progresses.RemoveAt(index);
        playerHandlers.RemoveAt(index);
        // ���̃X�N���v�g�Œʏ�ړ��ɖ߂��������s��
    }

}
