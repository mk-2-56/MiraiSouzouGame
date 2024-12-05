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

    [SerializeField] private float thresholdSpeed = 30f;
    // 長さの制限
    [SerializeField] private float minTrailVelZ = -2f;
    [SerializeField] private float maxTrailVelZ = -10f;

    // 太さの制限
    [SerializeField] private float minTrailScaleY = 0f;
    [SerializeField] private float maxTrailScaleY = 30f;

    void Start()
    {

        transform.parent.GetComponent<PlayerEffectDispatcher>().HandleSpeedE += UpdateEngineEffect;

    }

    void Update()
    {

       
    }
    private void FixedUpdate()
    {
              
    }

    public void UpdateEngineEffect(float objSpeed)
    {
        float trailVelZ = 0f, trailScaleY = 0f;
        if (objSpeed < thresholdSpeed)
        {
            EngineEffect.SetVector3(paramName_Velocity, new Vector3(0f, 0f, trailVelZ));
            EngineEffect.SetVector3(paramName_Scale, new Vector3(0f, trailScaleY, 0f));
        }
        else
        {
            // 長さの更新 (Velocity)
            trailVelZ = Mathf.Clamp(objSpeed * speedToLengthMultiplier, minTrailVelZ, maxTrailVelZ);
            EngineEffect.SetVector3(paramName_Velocity, new Vector3(0f, 0f, trailVelZ));

            // 太さの更新 (Scale)
            trailScaleY = Mathf.Clamp(objSpeed * speedToThicknessMultiplier, minTrailScaleY, maxTrailScaleY);
            EngineEffect.SetVector3(paramName_Scale, new Vector3(4f, trailScaleY, 1f));
        }
    }
}