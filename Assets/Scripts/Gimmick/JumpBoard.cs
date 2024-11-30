using UnityEngine;

public class JumpBoard : MonoBehaviour
{
    [SerializeField] float jumpBoardForce = 10.0f; // �����l��傫�߂ɐݒ�
    private CC.Basic playerController;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerController = other.GetComponent<CC.Basic>();
            Debug.Log("Player triggered the jump board!");

            Rigidbody playerRb = other.gameObject.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                // �W�����v�͂��v���C���[�ɓK�p
                playerRb.AddForce((
                    playerController.GetPlayerMovementParams().xzPlainVel.normalized * jumpBoardForce 
                    + 2 * Vector3.up * jumpBoardForce), ForceMode.VelocityChange);
            }
            else
            {
                Debug.LogWarning("Player does not have a Rigidbody component!");
            }
        }
    }
}