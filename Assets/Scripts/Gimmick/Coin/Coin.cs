using UnityEngine;
using UnityEngine.VFX;

public class Coin : MonoBehaviour
{
    [SerializeField] private float RotSpeed;          // ��]���x
    [SerializeField] private VisualEffect coinEffect; // �R�C���擾�G�t�F�N�g
    [SerializeField] private float gaugeValue = 0.1f; // �Q�[�W�̑�����

    private AudioSource coinSound;
    private bool isTouched = false;
    private Vector3 rotationAxis = Vector3.up;

    void Start()
    {
        coinSound = GetComponent<AudioSource>();
    }

    void Update()
    {
        // ��]�A�j���[�V����
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

        // �����ڂⓖ���蔻�������
        Destroy(GetComponent<MeshRenderer>());
        Destroy(GetComponent<Collider>());

        // �G�t�F�N�g�Đ�
        coinEffect.transform.position = transform.position;
        coinEffect.SendEvent("EventCoin");

        // �����Đ�
        if (coinSound != null)
        {
            coinSound.PlayOneShot(coinSound.clip);
        }

        // �����I�������R�C����j��
        Destroy(gameObject, coinSound?.clip.length ?? 0);
    }
}