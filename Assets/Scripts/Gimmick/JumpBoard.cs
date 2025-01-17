using UnityEngine;

public class JumpBoard : MonoBehaviour
{
    [SerializeField] float jumpBoardForce = 10.0f; // 初期値を大きめに設定
    [SerializeField] Transform param_direction;
    [SerializeField] float heightForce = 1f;
    [SerializeField] float forwardForce = 1f;
    private Vector3 _direction;
    private CC.Basic playerController;

    private void Start()
    {
        if (param_direction != null)
        {
            _direction = param_direction.forward;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerController = other.GetComponent<CC.Basic>();
            Debug.Log("Player triggered the jump board!");

            Rigidbody playerRb = other.gameObject.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                if (param_direction == null)
                    _direction = playerController.GetPlayerMovementParams().xzPlainVel.normalized * jumpBoardForce
                        + heightForce * Vector3.up * jumpBoardForce;
                else
                    _direction = _direction.normalized * jumpBoardForce * forwardForce + Vector3.up * jumpBoardForce * heightForce;
                playerRb.AddForce(_direction, ForceMode.VelocityChange);

                SoundManager.Instance?.PlaySE(SESoundData.SE.SE_JumpBoardS);

            }
            else
            {
                Debug.LogWarning("Player does not have a Rigidbody component!");
            }
        }
    }
}