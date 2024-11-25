using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager2 : MonoBehaviour
{

    [SerializeField] private Camera mainCamera; // ���C���J����
    [SerializeField] private Camera eventCamera; // �C�x���g�p�J����

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
    {//�J������ݒ�
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
