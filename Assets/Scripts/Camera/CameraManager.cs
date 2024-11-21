using AU;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject cineCamera; // イベント用カメラ
    [SerializeField] private GameObject cineCamera2; // イベント用カメラ
    //PlayerManager↓
    [SerializeField] GameObject param_cameraPrefab;
    [SerializeField] private PlayerManager playerManager;

    Dictionary<GameObject, GameObject> _gameCameras = new Dictionary<GameObject, GameObject>();

    private Camera activeCamera;
    // Start is called before the first frame update

    public void Initialized()
    {
        param_cameraPrefab.SetActive(false);
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
}
