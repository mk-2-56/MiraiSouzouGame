using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera mainCamera;
    public Camera eventCamera;

    private Camera activeCamera;
    // Start is called before the first frame update
    void Start()
    {
        SetActiveCamera(mainCamera);
    }

    private void SetActiveCamera(Camera camera)
    {
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
