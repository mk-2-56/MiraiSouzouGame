using AU;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CameraManager : MonoBehaviour
{
    protected Cinemachine.CinemachineVirtualCamera activeCamera;

    // ���N���X�ŉ��̏��������W�b�N
    public virtual void Initialized()
    {
        UnityEngine.Debug.Log("Initializing Camera Manager");
    }

    // ���ʂ̃J�����؂�ւ����W�b�N
    public void SetActiveCamera(Cinemachine.CinemachineVirtualCamera camera)
    {
        if (activeCamera != null)
        {
            activeCamera.Priority = 0;
        }

        activeCamera = camera;
        activeCamera.Priority = 10; // �D�揇�ʂ��グ��
    }

    public abstract void SpawnGameCamera(GameObject player);
    public abstract void AdjustGameCamera(int currentPlayerCount);

}