using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;
using static UnityEngine.InputSystem.Controls.AxisControl;

public class VFXSpeedController : MonoBehaviour
{
    public float speedToLengthMultiplier = 1f; // �����̃X�P�[���W��
    public float speedToThicknessMultiplier = 1f; // �����̃X�P�[���W��

//EngineEffect
    [SerializeField] private VisualEffect EngineEffect;
    [SerializeField] private string paramName_Scale = "Scale"; // �����̃p�����[�^��
    [SerializeField] private string paramName_Velocity = "Velocity"; // �����̃p�����[�^��
    [SerializeField] private AudioSource boostAudioSource;
    [SerializeField] private AudioSource boostingAudioSource;
    [SerializeField] float   mutiplier = 1.0f;

    // �����̐���
    [SerializeField] private float maxTrailVelZ = -12f;
    // �����̐���
    [SerializeField] private float maxTrailScaleY = 30f;

    private PlayerCanvasController playerCanvasController;
    void Start()
    {
        transform.parent.GetComponent<PlayerEffectDispatcher>().BoostStartE += ActiveEngineEffect;
        transform.parent.GetComponent<PlayerEffectDispatcher>().BoostEndE += DisableEngineEffect;
        playerCanvasController = transform.root.gameObject.GetComponent<PlayerCanvasController>();
        DisableEngineEffect();
    }

    void Update()
    {

       
    }
    private void FixedUpdate()
    {
              
    }

    public void ActiveEngineEffect()
    {
        {
            EngineEffect.enabled = true;
            // �����̍X�V (Velocity)
            EngineEffect.SetVector3(paramName_Velocity, new Vector3(0f, 0f, maxTrailVelZ));

            // �����̍X�V (Scale)
            EngineEffect.SetVector3(paramName_Scale, new Vector3(5f, maxTrailScaleY, 1f));
        }
        {//�Q�[�W�̏���
            if (playerCanvasController != null)
            {
                playerCanvasController.SetGaugeToBoost(true);
            }
            else
            {
                Debug.Log("PlayerCanvasController not found!");
            }
        }

        {//�T�E���h
            boostAudioSource.Play();
            boostingAudioSource.Play();
        }
    }

    public void DisableEngineEffect()
    {
        // �����̍X�V (Velocity)
        EngineEffect.SetVector3(paramName_Velocity, new Vector3(0f, 0f, 0f));

        // �����̍X�V (Scale)
        EngineEffect.SetVector3(paramName_Scale, new Vector3(0f, 0f, 0f));
        
        playerCanvasController.SetGaugeToBoost(false);

        boostingAudioSource?.Stop();

    }

}