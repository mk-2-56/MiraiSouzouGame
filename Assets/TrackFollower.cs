using AU;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackFollower : MonoBehaviour
{
    // Start is called before the first frame update
    private uint followerIndex = 0;
    private bool isFollower = false;
    private 
    void Start()
    {
        followerIndex = 0;
        isFollower = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void StartSplineMovement()
    {
        //if (_dollyCarts.TryGetValue(playerId, out CinemachineDollyCart dollyCart))
        //{
        //    // �v���C���[�̑���𖳌���
        //    var playerController = _players[playerId].GetComponent<PlayerController>();
        //    if (playerController != null)
        //    {
        //        playerController.enabled = false;
        //    }

        //    // Dolly Cart�𓮍�J�n
        //    dollyCart.m_Speed = dollySpeed;

        //    // �ړ����̃`�F�b�N���J�n
        //    StartCoroutine(CheckSplineEnd(playerId));
        //    Debug.Log($"�v���C���[{playerId}���X�v���C���ړ����J�n���܂����I");
        //}
    }
}
