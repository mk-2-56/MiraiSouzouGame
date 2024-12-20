using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using Cinemachine;


namespace AU
{
    using UnityEditor;

    [CustomEditor(typeof(PlayerManager))]
    public class PlayerManagerUI : Editor
    {

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button(new GUIContent("JoinPlayer")))
            {
                this.target.GetType().GetMethod("JoinPlayer").Invoke(target, null);
            }
        }
    }


    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] GameObject respawnPos;
        [SerializeField] GameObject param_playerPrefab;
        [SerializeField] GameObject _uiCanvasPrefab;
        [SerializeField] GameObject GameUIManager;
        
        [SerializeField] List<Color> p_playerColors = new();

        private GameObject _uiCanvasInstance;
        private TrackPositionManager _rTrackManager;

        public void OnPlayerJoined(PlayerInput input)
        {
            _curentPlayerCount++;
            GameObject player = input.gameObject;

            {
                float radius = 15.0f;
                RaycastHit hit;
                Vector3 pos = transform.position;
                Quaternion rot = transform.rotation;
                if (respawnPos != null)
                    pos = respawnPos.transform.position;
                rot = respawnPos.transform.rotation;

                if (Physics.SphereCast(pos + radius * Vector3.up, radius, Vector3.down, out hit, 100f, LayerMask.GetMask("Terrian")))
                    pos = hit.point;

                if (Physics.CheckSphere(pos,30f, LayerMask.GetMask("PlayerControlled")))
                {  pos.x += 10.0f; }
                player.transform.position = pos;
                player.transform.rotation = rot;

                if(_curentPlayerCount - 1 < p_playerColors.Count)
                { 
                    Color pcColor = p_playerColors[_curentPlayerCount - 1];

                    GameObject model = player.transform.Find("Facing/Cog/AnimationController/Character_V2").gameObject;
                    model.transform.Find("Raincoat").GetComponent<SkinnedMeshRenderer>().material.SetColor("_BASE_COLOR", pcColor);
                    model.transform.Find("L_boots").GetComponent<SkinnedMeshRenderer>().material.SetColor("_BASE_COLOR", pcColor);
                    model.transform.Find("R_boots").GetComponent<SkinnedMeshRenderer>().material.SetColor("_BASE_COLOR", pcColor);
                }
            }



            //Camera生成
            GameObject camera = _rCameraManager.SpawnGameCamera(player);
            //Canvas生成&初期化処理
            _uiCanvasInstance = Instantiate(_uiCanvasPrefab);
            Canvas canvas = _uiCanvasInstance.GetComponent<Canvas>();
            canvas.worldCamera = camera.transform.Find("Camera").GetComponent<Camera>();
            canvas.planeDistance = 1f;

            PlayerCanvasController controller = player.GetComponent<PlayerCanvasController>();
            controller.Canvas = _uiCanvasInstance;
            controller.Initialized();
            controller.playerHub = player.GetComponent<CC.Hub>();
            //プレイヤーDictionaryにプレイヤーを追加
            _players.Add(_curentPlayerCount, player);
            SetPlayerControl(false);
            if (_curentPlayerCount > 1)
                _rCameraManager.AdjustGameCamera(_curentPlayerCount);//画面分割
            GameUIManager.GetComponent<GameUIManager>().AddPlayerIcon(player.transform.GetChild(1).GetChild(0).GetChild(0));
        }

        public void JoinPlayer()
        {
            if (_curentPlayerCount > 1)
                return;

            _rInputManager.JoinPlayer();
        }

        public void ClearResetPlayers()
        {
            foreach (KeyValuePair<int, GameObject> entry in _players)
            {
                int key = entry.Key;
                if (entry.Value)
                {
                    Destroy(entry.Value);
                }
                if (_gameCameras[entry.Value])
                    Destroy(_gameCameras[entry.Value]);
            }
            _curentPlayerCount = 0;
            _players.Clear();
            _gameCameras.Clear();
        }

        public void RemovePlayer(int index)
        {
            if (_curentPlayerCount < 1)
                return;

            if (_players.ContainsKey(index))
            {
                if (_gameCameras.ContainsKey(_players[index]))
                {
                    Destroy(_gameCameras[_players[index]]);
                    _gameCameras.Remove(_players[index]);
                }
                _players.Remove(index);
                Destroy(_players[index]);
                _curentPlayerCount--;
                if (_curentPlayerCount < 2)
                    _rCameraManager.AdjustGameCamera(_curentPlayerCount);
            }
        }

        CameraManager _rCameraManager;
        PlayerInputManager _rInputManager;

        Dictionary<int, GameObject> _players = new Dictionary<int, GameObject>();
        Dictionary<GameObject, GameObject> _gameCameras = new Dictionary<GameObject, GameObject>();

        int _curentPlayerCount = 0;


        private void Start()
        {
            _rCameraManager = FindObjectOfType<CameraManager>();
            _rInputManager = GetComponent<PlayerInputManager>();
            _rTrackManager = ScriptableObject.CreateInstance<TrackPositionManager>();

        }

        public void Initialized()
        {
/*            cameraManager = FindObjectOfType<CameraManager>();
*/        }

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.V))
            {
                _rCameraManager.AdjustGameCamera(_curentPlayerCount);
            }
        }

        private void FixedUpdate()
        {
            _rTrackManager.UpdatePositions(_players);
        }
        private void OnDestroy()
        {
        }

        public bool SetPlayerControl(bool flag)
        {
            UnityEngine.Debug.Log("SetPlayerControl");
            bool playerSpawned = false;
            foreach (KeyValuePair<int, GameObject> playerEntry in _players)
            {
                GameObject player = playerEntry.Value;
                CC.Hub hubComponent = player.GetComponent<CC.Hub>();
                if (hubComponent != null)
                {
                    hubComponent.disableInput = !flag;
                    UnityEngine.Debug.Log("Player " + playerEntry.Key + " control " + (flag ? "enabled" : "disabled"));
                    playerSpawned = true;
                }
                else
                {
                    UnityEngine.Debug.LogWarning("Player " + playerEntry.Key + " does not have a Hub component.");
                }
            }
            return playerSpawned;
        }

        public int GetPlayerCount()
        {
            return _players.Count;
        }

    }
}

