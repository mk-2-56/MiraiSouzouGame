using AU;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class GameCameraManager : CameraManager
{
    // Start is called before the first frame update
    [SerializeField] private CinemachineBrain cinemachineBrain;
    [SerializeField] private List<CinemachineVirtualCamera> virtualCameras;
    [SerializeField] private float[] switchTimes; // 切り替えのタイミング（秒）

    //PlayerManager↓
    [SerializeField] GameObject param_cameraPrefab;
    [SerializeField] private PlayerManager playerManager;

    Dictionary<GameObject, GameObject> _gameCameras = new Dictionary<GameObject, GameObject>();

    // Start is called before the first frame update

    public override void Initialized()
    {
        base.Initialized();

        if (cinemachineBrain == null)
        {
            UnityEngine.Debug.Log("Brain not found!");
        }
        if (virtualCameras == null) return;
        for (int i = 0; i < virtualCameras.Count; i++)
        {
            virtualCameras[i].Priority = 0;
        }

        // 最初のカメラをアクティブに設定
        SetActiveCamera(virtualCameras[0]);

        StartCoroutine(SwitchVCameras());

        param_cameraPrefab?.SetActive(false);
        DontDestroyOnLoad(this);
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
    }

    public override void AdjustGameCamera(int curPlayerCount)
    {
        if (curPlayerCount <= 1) return;

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
            Vector2 pos = new Vector2(0.0f, 0.5f) * i;
            Vector2 size = new Vector2(1.0f, 0.5f);
            cam.rect = new Rect(pos, size);
            i++;
        }

    }
    private IEnumerator SwitchVCameras()
    {
        for (int i = 0; i < virtualCameras.Count; i++)
        {
            ResetVCamerasPriority();
            SetActiveCamera(virtualCameras[i]);

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




