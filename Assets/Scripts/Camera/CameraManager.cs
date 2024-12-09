using AU;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CameraManager : MonoBehaviour
{
    protected Cinemachine.CinemachineVirtualCamera activeCamera;

    // 基底クラスで仮の初期化ロジック
    public virtual void Initialized()
    {
        UnityEngine.Debug.Log("Initializing Camera Manager");
    }

    // 共通のカメラ切り替えロジック
    public void SetCineCamera(Cinemachine.CinemachineVirtualCamera camera , bool isUse)
    {
        if (activeCamera != null)
        {
            activeCamera.Priority = 0;
            //isUseがFalse（現ActiveCineCameraをDisableするならReturn
            activeCamera.enabled = isUse;
            if (isUse == false) return;
        }
        //isUseがTrue（現ActiveCineCameraを変える処理を行う
        activeCamera = camera;
        activeCamera.enabled = true;
        activeCamera.Priority = 10; // 優先順位を上げる

    }

    public abstract void SpawnGameCamera(GameObject player);
    public abstract void AdjustGameCamera(int currentPlayerCount);

}