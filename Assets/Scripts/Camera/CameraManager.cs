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
    public void SetCineCamera(Cinemachine.CinemachineVirtualCamera camera , bool isUse)
    {
        if (activeCamera != null)
        {
            activeCamera.Priority = 0;
            //isUse��False�i��ActiveCineCamera��Disable����Ȃ�Return
            activeCamera.enabled = isUse;
            if (isUse == false) return;
        }
        //isUse��True�i��ActiveCineCamera��ς��鏈�����s��
        activeCamera = camera;
        activeCamera.enabled = true;
        activeCamera.Priority = 10; // �D�揇�ʂ��グ��

    }

    public abstract void SpawnGameCamera(GameObject player);
    public abstract void AdjustGameCamera(int currentPlayerCount);

}