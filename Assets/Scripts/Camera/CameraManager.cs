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
    public void SetActiveCamera(Cinemachine.CinemachineVirtualCamera camera)
    {
        if (activeCamera != null)
        {
            activeCamera.Priority = 0;
        }

        activeCamera = camera;
        activeCamera.Priority = 10; // 優先順位を上げる
    }

    public abstract void SpawnGameCamera(GameObject player);
    public abstract void AdjustGameCamera(int currentPlayerCount);

}