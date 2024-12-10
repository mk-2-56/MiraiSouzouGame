using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.VFX;


public class CoinController : MonoBehaviour
{
    [SerializeField] float RotSpeed;                // アイドル状態の回転アニメ速度
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
        // 回転させておく
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
*/            // 一旦非表示に
            Destroy(GetComponent<MeshRenderer>());
            Destroy(GetComponent<BoxCollider>());

            // スコアに加算 あとでUImanagerとかからやるようにしたい
            // coinScore.AddCoin();

            // エフェクトと音を再生
            coinEffect.transform.position = transform.position;
            coinEffect.SendEvent("EventCoin");
            if (coinSound != null) coinSound.PlayOneShot(coinSound.clip);

            // 音が鳴り終わったらコインは消す
            Destroy(gameObject, coinSound.clip.length);
        }
    }

}
