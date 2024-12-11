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
        private GameObject _uiCanvasInstance;
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

                if (Physics.SphereCast(pos + radius * Vector3.up, radius, Vector3.down, out hit))
                    pos = hit.point;

                player.transform.position = pos;
                player.transform.rotation = rot;
            }

            _uiCanvasInstance = Instantiate(_uiCanvasPrefab);
            player.GetComponent<PlayerCanvasController>().Canvas = _uiCanvasInstance;

            _players.Add(_curentPlayerCount, player);
            _rCameraManager.SpawnGameCamera(player);

            if (_curentPlayerCount > 1)
                _rCameraManager.AdjustGameCamera(_curentPlayerCount);
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

        private void OnDestroy()
        {
        }

        public void SetPlayerControl(bool flag)
        {
            UnityEngine.Debug.Log("SetPlayerControl");

            foreach (KeyValuePair<int, GameObject> playerEntry in _players)
            {
                GameObject player = playerEntry.Value;
                CC.Hub hubComponent = player.GetComponent<CC.Hub>();
                if (hubComponent != null)
                {
                    hubComponent.disableInput = !flag;
                    UnityEngine.Debug.Log("Player " + playerEntry.Key + " control " + (flag ? "enabled" : "disabled"));
                }
                else
                {
                    UnityEngine.Debug.LogWarning("Player " + playerEntry.Key + " does not have a Hub component.");
                }
            }
        }

        public int GetPlayerCount()
        {
            return _players.Count;
        }
    }
}

