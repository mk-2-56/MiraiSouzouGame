using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    public bool isShaking;
    //�V�F�C�N�̋���
    public float shakeAmount = 0.2f;
    //�V�F�C�N�̎���
    public float shakeDuration = 1.0f;

    private Vector3 originalPosition;
    private float remainingShakeTime;
    // Start is called before the first frame update
    void Start()
    {
        isShaking = false;
        
        if (Camera.main != null)
        {
            originalPosition = Camera.main.transform.localPosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            isShaking = !isShaking;
        }
        if (isShaking)
        {
            TriggerShake();
        }
        if (remainingShakeTime > 0 && Camera.main != null)
        {
            Camera.main.transform.localPosition = originalPosition + Random.insideUnitSphere * shakeAmount;

            // �V�F�C�N���Ԃ�����
            remainingShakeTime -= Time.deltaTime;
        }
        else if (Camera.main != null)
        {
            // �V�F�C�N���I��������J���������̈ʒu�ɖ߂�
            Camera.main.transform.localPosition = originalPosition;
        }
    }

    // �V�F�C�N���J�n���郁�\�b�h
    public void TriggerShake()
    {
        remainingShakeTime = shakeDuration;
    }


}
