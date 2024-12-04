using UnityEngine;
using UnityEngine.VFX;

public class VFXSpeedController : MonoBehaviour
{
    [SerializeField] private VisualEffect visualEffect;
    [SerializeField] private string paramName_Scale = "Scale"; // 太さのパラメータ名
    [SerializeField] private string paramName_Velocity = "Velocity"; // 長さのパラメータ名

    private Rigidbody rigidbody;
    public float speedToLengthMultiplier = 1f; // 長さのスケール係数
    public float speedToThicknessMultiplier = 1f; // 太さのスケール係数

    private float thresholdSpeed = 30f;
    // 長さの制限
    private float minTrailVelZ = -2f;
    private float maxTrailVelZ = -6f;

    // 太さの制限
    private float minTrailScaleY = 3f;
    private float maxTrailScaleY = 20f;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {

       
    }

    private void FixedUpdate()
    {
        float objSpeed = rigidbody.velocity.magnitude;
/*        Debug.Log("Player Current Speed : " + objSpeed.ToString());
*/
        float trailVelZ = 0f, trailScaleY = 0f;
        if (objSpeed < thresholdSpeed)
        {
            visualEffect.SetVector3(paramName_Velocity, new Vector3(0f, 0f, trailVelZ));
            visualEffect.SetVector3(paramName_Scale, new Vector3(0f, trailScaleY, 0f));
        }
        else
        {
            // 長さの更新 (Velocity)
            trailVelZ = Mathf.Clamp(objSpeed * speedToLengthMultiplier, minTrailVelZ, maxTrailVelZ);
            visualEffect.SetVector3(paramName_Velocity, new Vector3(0f, 0f, trailVelZ));

            // 太さの更新 (Scale)
            trailScaleY = Mathf.Clamp(objSpeed * speedToThicknessMultiplier, minTrailScaleY, maxTrailScaleY);
            visualEffect.SetVector3(paramName_Scale, new Vector3(4f, trailScaleY, 1f));
        }

    }
}