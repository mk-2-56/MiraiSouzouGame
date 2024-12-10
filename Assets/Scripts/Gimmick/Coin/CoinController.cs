using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.VFX;


public class CoinController : MonoBehaviour
{
    [SerializeField] float RotSpeed;                // �A�C�h����Ԃ̉�]�A�j�����x
    [SerializeField] VisualEffect coinEffect;
    [SerializeField] private GaugeController gaugeController;

    private AudioSource coinSound;
    private bool isTouched;
    private Vector3 Axe = Vector3.up;
    void Start()
    {
        coinSound = GetComponent<AudioSource>();
    }

    void Update()
    {
        // ��]�����Ă���
        if (!isTouched) transform.RotateAround(transform.position, Axe, RotSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            isTouched = true;

            //PlayerEffectDispatcher dispatcher = other.GetComponent<PlayerEffectDispatcher>();

            //if (dispatcher != null) {
            //    dispatcher.CoinCollectedE?.Invoke();
            //}
/*            gaugeController.SetGaugeValue(0.1f);
*/            // ��U��\����
            Destroy(GetComponent<MeshRenderer>());
            Destroy(GetComponent<BoxCollider>());

            // �X�R�A�ɉ��Z ���Ƃ�UImanager�Ƃ�������悤�ɂ�����
            // coinScore.AddCoin();

            // �G�t�F�N�g�Ɖ����Đ�
            coinEffect.transform.position = transform.position;
            coinEffect.SendEvent("EventCoin");
            if (coinSound != null) coinSound.PlayOneShot(coinSound.clip);

            // ������I�������R�C���͏���
            Destroy(gameObject, coinSound.clip.length);
        }
    }

}
