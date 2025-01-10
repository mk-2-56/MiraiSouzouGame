using UnityEngine;
using Cinemachine;
using System.Collections.Generic;
using UnityEngine.VFX;

public class TrackFollower : MonoBehaviour
{
    [SerializeField] private CinemachineSmoothPath trackPath; // �g���b�N
    [SerializeField] private float trackSpeed;         // �ړ����x
    private Dictionary<GameObject, CinemachineDollyCart> activeCarts = new Dictionary<GameObject, CinemachineDollyCart>();
    private Dictionary<GameObject, Quaternion> initialRotations = new Dictionary<GameObject, Quaternion>();
    private Dictionary<GameObject, Vector3> endDirs = new Dictionary<GameObject, Vector3>();
    private AudioSource curAucioSource = null;
    private void Update()
    {
        // �S�Ă�DollyCart���`�F�b�N
        List<GameObject> cartsToRemove = new List<GameObject>();

        foreach (var entry in activeCarts)
        {
            GameObject player = entry.Key;
            CinemachineDollyCart dollyCart = entry.Value;

            // �v���C���[�̌������Œ�
            if (initialRotations.ContainsKey(player))
            {
                player.transform.rotation = initialRotations[player];
            }

            // �I�_�ɓ��B����������
            if (dollyCart.m_Position >= dollyCart.m_Path.PathLength)
            {
                curAucioSource?.Stop();
                // �I���̕������擾
                Vector3 endDir = dollyCart.m_Path.EvaluateTangentAtUnit(
                    dollyCart.m_Path.PathLength, // �ŏI�ʒu
                    CinemachinePathBase.PositionUnits.Distance
                );

                endDir = endDir.normalized; // ���K��
                if (!endDirs.ContainsKey(player))
                {
                    endDirs[player] = endDir; // �I�����̐i�s������ۑ�
                }

                dollyCart.m_Speed = 0;
                Destroy(dollyCart);
                cartsToRemove.Add(player);

                // Kinematic���I�t�ɖ߂��i�K�v�Ȃ�j
                Rigidbody rb = player.GetComponent<Rigidbody>();
                if (rb)
                {
                    rb.isKinematic = false; 
                }
            }

            // �X�v���C���ړ��I������i�s�����ɉ����Ĉړ����p��
            if (endDirs.ContainsKey(player))
            {
                AddEndTrackVec(player, endDirs[player]);
            }
        }

        // �I�_���B�����̏���
        foreach (var player in cartsToRemove)
        {
            activeCarts.Remove(player);
            initialRotations.Remove(player);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !activeCarts.ContainsKey(other.gameObject))
        {
            SoundManager.Instance?.PlaySE(SESoundData.SE.SE_WindTrigger);

            GameObject player = other.gameObject;
            //Animation Set
            player.transform.Find("Facing/Cog/AnimationController").GetComponent<PlayerAnimationControl>().DispatchBigJump();
            
            player.transform.Find("Facing/Cog/EffectDispatcher/BigJumpWindEffect").GetComponent<VisualEffect>().Play() ;
            Rigidbody rb = player.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true; // ���������𖳌���
            }

            // �v���C���[�̏����������L�^
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

            curAucioSource = SoundManager.Instance?.PlaySE(SESoundData.SE.SE_WindOnBigJump);
            Debug.Log($"{player.name} started moving along the track!");
        }
    }

    // �I���̈ړ�����
    private void AddEndTrackVec(GameObject player, Vector3 direction)
    {
        float moveSpeed = trackSpeed; // �p���I�Ȉړ����x
        // �v���C���[��i�s�����Ɉړ�
        player.GetComponent<Rigidbody>().velocity = direction * moveSpeed;
    }
}