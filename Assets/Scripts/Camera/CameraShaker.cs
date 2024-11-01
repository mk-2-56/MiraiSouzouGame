using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    public bool isShaking;
    //シェイクの強さ
    public float shakeAmount = 0.2f;
    //シェイクの時間
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

            // シェイク時間を減少
            remainingShakeTime -= Time.deltaTime;
        }
        else if (Camera.main != null)
        {
            // シェイクが終了したらカメラを元の位置に戻す
            Camera.main.transform.localPosition = originalPosition;
        }
    }

    // シェイクを開始するメソッド
    public void TriggerShake()
    {
        remainingShakeTime = shakeDuration;
    }


}
