using UnityEngine;
using UnityEngine.VFX;

public class VFXSpeedController : MonoBehaviour
{
    [SerializeField] private VisualEffect visualEffect;
    [SerializeField] private string paramName_Scale = "Scale"; // �����̃p�����[�^��
    [SerializeField] private string paramName_Velocity = "Velocity"; // �����̃p�����[�^��

    private Rigidbody rigidbody;
    public float speedToLengthMultiplier = 1f; // �����̃X�P�[���W��
    public float speedToThicknessMultiplier = 1f; // �����̃X�P�[���W��

    private float thresholdSpeed = 30f;
    // �����̐���
    private float minTrailVelZ = -2f;
    private float maxTrailVelZ = -6f;

    // �����̐���
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
            // �����̍X�V (Velocity)
            trailVelZ = Mathf.Clamp(objSpeed * speedToLengthMultiplier, minTrailVelZ, maxTrailVelZ);
            visualEffect.SetVector3(paramName_Velocity, new Vector3(0f, 0f, trailVelZ));

            // �����̍X�V (Scale)
            trailScaleY = Mathf.Clamp(objSpeed * speedToThicknessMultiplier, minTrailScaleY, maxTrailScaleY);
            visualEffect.SetVector3(paramName_Scale, new Vector3(4f, trailScaleY, 1f));
        }

    }
}