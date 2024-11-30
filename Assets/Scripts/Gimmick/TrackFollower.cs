using UnityEngine;
using Cinemachine;
using System.Collections.Generic;
using AU;

public class TrackFollower : MonoBehaviour
{
    [SerializeField] private CinemachineSmoothPath trackPath; // �g���b�N
    [SerializeField] private float trackSpeed = 5.0f;         // �ړ����x
    private Dictionary<GameObject, CinemachineDollyCart> activeCarts = new Dictionary<GameObject, CinemachineDollyCart>();
    private Dictionary<GameObject, Quaternion> initialRotations = new Dictionary<GameObject, Quaternion>();


    private void Update()
    {
        // �S�Ă�DollyCart���`�F�b�N
        List<GameObject> cartsToRemove = new List<GameObject>();

        foreach (var entry in activeCarts)
        {
            GameObject player = entry.Key;
            CinemachineDollyCart dollyCart = entry.Value;

            // �I�_�ɓ��B����������
            if (dollyCart.m_Position >= dollyCart.m_Path.PathLength)
            {
                dollyCart.m_Speed = 0; // DollyCart���~
                Destroy(dollyCart);    // DollyCart�R���|�[�l���g���폜
                cartsToRemove.Add(player); // �폜�\��̃v���C���[���L�^
                UnityEngine.Debug.Log($"{player.name} reached the end of the track.");
            }
            else
            {
                // �v���C���[�̉�]���ێ�
                if (initialRotations.ContainsKey(player))
                {
                    player.transform.rotation = initialRotations[player];
                }
            }
        }

        // �I�_���B�����v���C���[����������폜
        foreach (var player in cartsToRemove)
        {
            activeCarts.Remove(player);
        }

        /* if (dollyCart.m_Position >= dollyCart.m_Path.PathLength)
         {
             GameObject player = other.gameObject;

             // DollyCart���~���A�v���C���[����폜
             CinemachineDollyCart dollyCart = activeCarts[player];
             dollyCart.m_Speed = 0;
             Destroy(dollyCart);

             activeCarts.Remove(player);
         }*/

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !activeCarts.ContainsKey(other.gameObject))
        {
            GameObject player = other.gameObject;

            // �v���C���[�̏�����]��ۑ�
            if (!initialRotations.ContainsKey(player))
            {
                initialRotations[player] = player.transform.rotation;
            }

            // �v���C���[��DollyCart��ǉ�
            CinemachineDollyCart dollyCart = player.AddComponent<CinemachineDollyCart>();
            dollyCart.m_Path = trackPath;        // �g���b�N��ݒ�
            dollyCart.m_Position = 0;           // �g���b�N�̊J�n�ʒu
            dollyCart.m_Speed = trackSpeed;     // �g���b�N��̈ړ����x
            dollyCart.m_UpdateMethod = CinemachineDollyCart.UpdateMethod.FixedUpdate;

            activeCarts[player] = dollyCart;

            UnityEngine.Debug.Log($"{player.name} started moving along the track!");
        }
    }

}