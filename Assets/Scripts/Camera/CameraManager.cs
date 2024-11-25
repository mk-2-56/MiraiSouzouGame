using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager2 : MonoBehaviour
{

    [SerializeField] private Camera mainCamera; // メインカメラ
    [SerializeField] private Camera eventCamera; // イベント用カメラ

    private Camera activeCamera;
    // Start is called before the first frame update

    public void Initialized()
    {
        if(mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        SetActiveCamera(mainCamera);
        Debug.Log("CameraManager initialized");
    }
    private void SetActiveCamera(Camera camera)
    {//カメラを設定
        if(activeCamera != null)
        {
            activeCamera.gameObject.SetActive(false);
        }

        //
        activeCamera = camera;
        activeCamera.gameObject.SetActive(true);
    }

    public void SetMainCamera()
    {
        SetActiveCamera(mainCamera);
    }

    public void SetEventCamera()
    {
        SetActiveCamera(eventCamera);
    }
}
