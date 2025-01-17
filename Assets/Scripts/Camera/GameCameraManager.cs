using AU;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Drawing;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.XR;
using static UnityEditor.PlayerSettings;



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
    public GameObject mainCamera;
    [SerializeField] private bool _skipOpening  = false;
    public bool SkipOpening // プロパティ
    {
        get { return _skipOpening; }  // 通称ゲッター。呼び出した側がscoreを参照できる
        set { _skipOpening = value; } // 通称セッター。value はセットする側の数字などを反映する
    }
    [SerializeField] private PlayerManager _playerManager;
    [SerializeField] private GameUIManager _gameUIManager;
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

        if (!_skipOpening)
        {
            // 最初のカメラをアクティブに設定
            if (virtualCameras[0] != null)
            {
                SetCineCamera(virtualCameras[0], true);
                if (virtualCameras.Count > 1) StartCoroutine(SwitchVCameras());
            }
            param_cameraPrefab?.SetActive(false);
        }
        else
        {
            ResetVCamerasPriority();
            DisableAllVCamera();
            param_cameraPrefab?.SetActive(true);
        }

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


    public override GameObject SpawnGameCamera(GameObject gameObject)//->playerCharacter
    {
        GameObject camera = Instantiate(param_cameraPrefab,
            gameObject.transform.position, gameObject.transform.rotation);

        var tmanager = gameObject.AddComponent<TargetManager>();
        camera.GetComponent<TargetUI>().SetTargetManager(tmanager);

        var tmp = camera.GetComponent<GameCamera>();
        tmp.SetPlayerReference(gameObject);
        camera.SetActive(_skipOpening);
        _gameCameras.Add(gameObject, camera);
        return camera;

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

    public void SetAllGameCamera(bool isEnable)
    {
        foreach (GameObject cam in _gameCameras.Values)
        {
            cam.gameObject.SetActive(isEnable);
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
        _gameUIManager.StartCount();

        SetAllGameCamera(true);
    }

    private void ResetVCamerasPriority()
    {
        foreach (var camera in virtualCameras)
        {
            camera.Priority = 0;
        }
    }

    private void DisableAllVCamera()
    {
        foreach (var camera in virtualCameras)
        {
            camera.enabled = false;
        }
    }
}




