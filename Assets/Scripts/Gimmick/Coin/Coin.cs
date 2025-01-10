using UnityEngine;
using UnityEngine.VFX;

public class Coin : MonoBehaviour
{
    [SerializeField] private float RotSpeed;          // 回転速度
    [SerializeField] private VisualEffect coinEffect; // コイン取得エフェクト
    [SerializeField] private float gaugeValue = 0.1f; // ゲージの増加量

    private AudioSource coinSound;
    private bool isTouched = false;
    private Vector3 rotationAxis = Vector3.up;

    void Start()
    {
        coinSound = GetComponent<AudioSource>();
    }

    void Update()
    {
        // 回転アニメーション
        if (!isTouched)
        {
            transform.RotateAround(transform.position, rotationAxis, RotSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetCoin(other);
        }
    }

    private void GetCoin(Collider other)
    {
        if (isTouched) return;

        isTouched = true;

        PlayerCanvasController playerCanvasController = other.GetComponent<PlayerCanvasController>();
        CoinCollector coinCollector = playerCanvasController._coinCollector;

        if (coinCollector != null)
        {
            coinCollector.GetOneCoin();
        }

        // 見た目や当たり判定を消去
        Destroy(GetComponent<MeshRenderer>());
        Destroy(GetComponent<Collider>());

        // エフェクト再生
        coinEffect.transform.position = transform.position;
        coinEffect.SendEvent("EventCoin");

        // 音を再生
        if (coinSound != null)
        {
            coinSound.PlayOneShot(coinSound.clip);
        }

        // 音が終わったらコインを破棄
        Destroy(gameObject, coinSound?.clip.length ?? 0);
    }
}