using UnityEngine;

public class MeteorsActivatePoint1 : MonoBehaviour
{
    [SerializeField] private MeteorsManager meteorsManager; // MeteorMovement ��Inspector�Ŋ��蓖��
    [SerializeField] private float waitTime = 10f;          // �ăA�N�e�B�x�[�g�̑ҋ@����

    private bool canActivate = true;
    private float timer = 0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canActivate)
        {
            Debug.Log("MeteorsActive");
            meteorsManager?.ActivateMeteors();
            canActivate = false; // �A�N�e�B�x�[�g�𖳌���
        }
    }

    private void Update()
    {
        if (!canActivate)
        {
            timer += Time.deltaTime;
            if (timer >= waitTime)
            {
                canActivate = true; // �A�N�e�B�x�[�g�\�ɖ߂�
                timer = 0f;
            }
        }
    }
}