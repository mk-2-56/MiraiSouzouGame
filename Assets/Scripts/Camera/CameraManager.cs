using AU;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class CameraManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private CinemachineBrain cinemachineBrain;
    [SerializeField] private List<CinemachineVirtualCamera> VirtualCameras;
    [SerializeField] private float[] switchTimes; // 切り替えのタイミング（秒）

    //PlayerManager↓
    [SerializeField] GameObject param_cameraPrefab;
    [SerializeField] private PlayerManager playerManager;

    Dictionary<GameObject, GameObject> _gameCameras = new Dictionary<GameObject, GameObject>();

    private Camera activeCamera;
    // Start is called before the first frame update

    public void Initialized()
    {
        if (cinemachineBrain == null)
        {
            UnityEngine.Debug.Log("Brain not found!");
        }
        if (VirtualCameras == null) return;
        for (int i = 0; i < VirtualCameras.Count; i++)
        {//一番目から優先順位をつける
            /*            VirtualCameras[i].Priority = VirtualCameras.Count - i + 1;
            */
            VirtualCameras[i].Priority = 0;
        }
        VirtualCameras[0].Priority = 10;

        StartCoroutine(SwitchVCameras());


        param_cameraPrefab?.SetActive(false);
        DontDestroyOnLoad(this);
    }

    public void SpawnGameCamera(GameObject gameObject)//->playerCharacter
    {
        GameObject camera = Instantiate(param_cameraPrefab,
            gameObject.transform.position, gameObject.transform.rotation);

        var tmp = camera.GetComponent<GameCamera>();
        tmp.SetPlayerReference(gameObject);
        camera.SetActive(true);
        _gameCameras.Add(gameObject, camera);
    }

    public void AdjustGameCamera(int curPlayerCount)
    {
        if (curPlayerCount <= 1)
        {
            return;
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
            Vector2 pos = new Vector2(0.0f, 0.5f) * i;
            Vector2 size = new Vector2(1.0f, 0.5f);
            cam.rect = new Rect(pos, size);
            i++;
        }

    }
    private IEnumerator SwitchVCameras()
    {
        for (int i = 0; i < VirtualCameras.Count; i++)
        {
            // 全カメラのPriorityをリセット
            ResetVCamerasPriority();

            // 現在のカメラをアクティブ化
            VirtualCameras[i].Priority = 10;

            UnityEngine.Debug.Log($"カメラ{i + 1}がアクティブになりました");

            // 次の切り替え時間を待つ
            if (i < switchTimes.Length)
                yield return new WaitForSeconds(switchTimes[i]);
        }
    }


    private void ResetVCamerasPriority()
    {
        foreach (var cam in VirtualCameras)
        {
            cam.Priority = 0;
        }
    }
}




