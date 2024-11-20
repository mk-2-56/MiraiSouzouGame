using UnityEngine;

public class MeteorsActivatePoint1 : MonoBehaviour
{
    [SerializeField] private MeteorsManager meteorsManager; // MeteorMovement をInspectorで割り当て
    [SerializeField] private float waitTime = 10f;          // 再アクティベートの待機時間

    private bool canActivate = true;
    private float timer = 0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canActivate)
        {
            Debug.Log("MeteorsActive");
            meteorsManager?.ActivateMeteors();
            canActivate = false; // アクティベートを無効化
        }
    }

    private void Update()
    {
        if (!canActivate)
        {
            timer += Time.deltaTime;
            if (timer >= waitTime)
            {
                canActivate = true; // アクティベート可能に戻す
                timer = 0f;
            }
        }
    }
}