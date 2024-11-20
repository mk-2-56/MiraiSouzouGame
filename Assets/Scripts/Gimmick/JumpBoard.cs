using UnityEngine;

public class JumpBoard : MonoBehaviour
{
    [SerializeField] float jumpBoardForce = 10.0f; // 初期値を大きめに設定
    private CCT_Basic playerController;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerController = other.GetComponent<CCT_Basic>();
            Debug.Log("Player triggered the jump board!");

            Rigidbody playerRb = other.gameObject.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                // ジャンプ力をプレイヤーに適用
                playerRb.AddForce((playerRb.velocity * jumpBoardForce +  Vector3.up * jumpBoardForce), ForceMode.Impulse);
            }
            else
            {
                Debug.LogWarning("Player does not have a Rigidbody component!");
            }
        }
    }
}