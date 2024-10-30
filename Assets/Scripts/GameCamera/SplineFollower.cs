using UnityEngine;
using UnityEngine.Splines;

public class SplineFollower : MonoBehaviour
{
    public SplineContainer splineContainer;
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float progress = 0.0f;

    // Update is called once per frame
    void Update()
    {
        // �X�v���C���ɉ������i�s�x���X�V
        progress += speed * Time.deltaTime / splineContainer.CalculateLength();
        progress = Mathf.Clamp01(progress);  // �i�s�x��0?1�ɐ���

        // �X�v���C����̌��݂̈ʒu���擾���ăv���C���[���ړ�
        Vector3 position = splineContainer.EvaluatePosition(progress);
        transform.position = position;

        // �I�u�W�F�N�g�̌������X�v���C���ɉ��킹��ꍇ
        //Quaternion rotation = splineContainer.EvaluateOr(progress);
        //transform.rotation = rotation;
    }
}
