using AU;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;



namespace GameCameraMode
{
    enum SplitMode
    {
        Horizontal,
        Vertical,
    }
}
public class GameCameraManager : CameraManager
{
    // Start is called before the first frame update
    [SerializeField] private Camera mainCamera;
    [SerializeField] private CinemachineBrain cinemachineBrain;
    [SerializeField] private List<CinemachineVirtualCamera> virtualCameras;
    [SerializeField] private Cinemachine.CinemachineDollyCart dollyCart1;
    [SerializeField] private Cinemachine.CinemachineSmoothPath dollyPath1;

    [SerializeField] private float[] switchTimes; // 切り替えのタイミング（秒）
        
    [SerializeField] private GameObject startPos;
    [SerializeField] private GameCameraMode.SplitMode param_splitMode;

    //PlayerManager↓
    [SerializeField] GameObject param_cameraPrefab;
    [SerializeField] private PlayerManager playerManager;

    Dictionary<GameObject, GameObject> _gameCameras = new Dictionary<GameObject, GameObject>();

    private float lastSwitchTime = 4f;
    // Start is called before the first frame update

    public override void Initialized()
    {
        base.Initialized();

        if (cinemachineBrain == null)
        {
            UnityEngine.Debug.Log("Brain not found!");
        }

        for (int i = 0; i < virtualCameras.Count; i++)
        {
            if (virtualCameras[i] == null) continue;
            virtualCameras[i].Priority = 0;
            virtualCameras[i].m_LookAt = startPos.GetComponent<Transform>();
        }

        // 最初のカメラをアクティブに設定
        if (virtualCameras[0] != null)
        {
            SetCineCamera(virtualCameras[0], true);
            if(virtualCameras.Count > 1) StartCoroutine(SwitchVCameras());
        }

        param_cameraPrefab?.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            param_splitMode++;
            if (param_splitMode > GameCameraMode.SplitMode.Vertical)
                param_splitMode = 0;
        }

        if (virtualCameras[0] == activeCamera)
        {
            if(dollyCart1.m_Position >= dollyPath1.PathLength)
            {
                SetCineCamera(virtualCameras[1],true);
            }
        }

        if(virtualCameras[virtualCameras.Count - 1].enabled == true)
        {
            lastSwitchTime -= Time.deltaTime;
            if(lastSwitchTime < 0)
            {
                ResetVCamerasPriority();

            }
        }

    }


    public override void SpawnGameCamera(GameObject gameObject)//->playerCharacter
    {
        GameObject camera = Instantiate(param_cameraPrefab,
            gameObject.transform.position, gameObject.transform.rotation);

        var tmanager = gameObject.AddComponent<TargetManager>();
        camera.GetComponent<TargetUI>().SetTargetManager(tmanager);

        var tmp = camera.GetComponent<GameCamera>();
        tmp.SetPlayerReference(gameObject);
        camera.SetActive(true);
        _gameCameras.Add(gameObject, camera);



/*        camera.GetComponent<Camera>().Render
*/

    }

    public override void AdjustGameCamera(int curPlayerCount)
    {
        Vector2 pos = new Vector2(0.0f, 0.0f);
        Vector2 size = new Vector2(0.0f, 0.0f);
        Vector2 posPerPlayer = new Vector2();
        Vector2 sizePerPlayer = new Vector2();

        posPerPlayer = sizePerPlayer = new Vector2(1.0f, 1.0f) / curPlayerCount;
        if (param_splitMode == GameCameraMode.SplitMode.Horizontal)
        {
            posPerPlayer.y = 0;
            sizePerPlayer.y = 1;
        }
        else
        {
            posPerPlayer.x = 0;
            sizePerPlayer.x = 1;
        }

        int i = 0;
        foreach (KeyValuePair<GameObject, GameObject> item in _gameCameras)
        {
            if (!item.Key)
            {
                Destroy(item.Value);
                _gameCameras.Remove(item.Key);
                return;
            }

            Camera cam = item.Value.transform.Find("Camera").GetComponent<Camera>();

            pos += posPerPlayer * i;
            size = sizePerPlayer;
            cam.rect = new Rect(pos, size);
            i++;
        }
    }
    private IEnumerator SwitchVCameras()
    {
        for (int i = 0; i < virtualCameras.Count; i++)
        {
            ResetVCamerasPriority();
            SetCineCamera(virtualCameras[i], true);

            UnityEngine.Debug.Log($"Camera {i + 1} is now active");

            if (i < switchTimes.Length)
            {
                yield return new WaitForSeconds(switchTimes[i]);
            }
        }
    }

    private void ResetVCamerasPriority()
    {
        foreach (var camera in virtualCameras)
        {
            camera.Priority = 0;
        }
    }
}




