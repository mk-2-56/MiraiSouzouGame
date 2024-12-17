using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;
using static UnityEngine.InputSystem.Controls.AxisControl;

public class VFXSpeedController : MonoBehaviour
{
    public float speedToLengthMultiplier = 1f; // 長さのスケール係数
    public float speedToThicknessMultiplier = 1f; // 太さのスケール係数

//EngineEffect
    [SerializeField] private VisualEffect EngineEffect;
    [SerializeField] private string paramName_Scale = "Scale"; // 太さのパラメータ名
    [SerializeField] private string paramName_Velocity = "Velocity"; // 長さのパラメータ名
    [SerializeField] private AudioSource boostAudioSource;
    [SerializeField] private AudioSource boostingAudioSource;
    [SerializeField] float   mutiplier = 1.0f;

    // 長さの制限
    [SerializeField] private float maxTrailVelZ = -12f;
    // 太さの制限
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
            // 長さの更新 (Velocity)
            EngineEffect.SetVector3(paramName_Velocity, new Vector3(0f, 0f, maxTrailVelZ));

            // 太さの更新 (Scale)
            EngineEffect.SetVector3(paramName_Scale, new Vector3(5f, maxTrailScaleY, 1f));
        }
        {//ゲージの処理
            if (playerCanvasController != null)
            {
                playerCanvasController.SetGaugeToBoost(true);
            }
            else
            {
                Debug.Log("PlayerCanvasController not found!");
            }
        }

        {//サウンド
            boostAudioSource.Play();
            boostingAudioSource.Play();
        }
    }

    public void DisableEngineEffect()
    {
        // 長さの更新 (Velocity)
        EngineEffect.SetVector3(paramName_Velocity, new Vector3(0f, 0f, 0f));

        // 太さの更新 (Scale)
        EngineEffect.SetVector3(paramName_Scale, new Vector3(0f, 0f, 0f));
        
        playerCanvasController.SetGaugeToBoost(false);

        boostingAudioSource?.Stop();

    }

}