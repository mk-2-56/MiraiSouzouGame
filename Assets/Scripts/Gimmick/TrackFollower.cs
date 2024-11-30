using UnityEngine;
using Cinemachine;
using System.Collections.Generic;
using AU;

public class TrackFollower : MonoBehaviour
{
    [SerializeField] private CinemachineSmoothPath trackPath; // �g���b�N
    [SerializeField] private float trackSpeed = 5.0f;         // �ړ����x
    private Dictionary<GameObject, CinemachineDollyCart> activeCarts = new Dictionary<GameObject, CinemachineDollyCart>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !activeCarts.ContainsKey(other.gameObject))
        {
            GameObject player = other.gameObject;

            // �v���C���[��DollyCart��ǉ�
            CinemachineDollyCart dollyCart = player.AddComponent<CinemachineDollyCart>();
            dollyCart.m_Path = trackPath;        // �g���b�N��ݒ�
            dollyCart.m_Position = 0;            // �g���b�N�̊J�n�ʒu
            dollyCart.m_Speed = trackSpeed;      // �g���b�N��̈ړ����x
            dollyCart.m_UpdateMethod = CinemachineDollyCart.UpdateMethod.FixedUpdate;

            activeCarts[player] = dollyCart;

/*            UnityEngine.Debug.Log($"{player.name} started moving along the track!");
*/        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && activeCarts.ContainsKey(other.gameObject))
        {
            GameObject player = other.gameObject;

            // DollyCart���~���A�v���C���[����폜
            CinemachineDollyCart dollyCart = activeCarts[player];
            dollyCart.m_Speed = 0;
            Destroy(dollyCart);

            activeCarts.Remove(player);

/*            Debug.Log($"{player.name} stopped moving along the track!");
*/        }
    }
}