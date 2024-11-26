using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace AU
{
    using Unity.VisualScripting;
    using UnityEditor;

    public class PlayerManager : MonoBehaviour
    {
        private  CameraManager cameraManager;

        public void OnPlayerJoined(PlayerInput input)
        {
            _curentPlayerCount++;
            GameObject player = input.gameObject;
            player.transform.position = transform.position;
            player.transform.rotation = transform.rotation;

            cameraManager.SpawnGameCamera(player);
            player.AddComponent<TargetManager>();

            if (_curentPlayerCount > 1)
                cameraManager.AdjustGameCamera(_curentPlayerCount);
        }

        public void SpawnPlayer()
        {
            if (_curentPlayerCount > 1)
                return;

            _curentPlayerCount++;
            GameObject playerCharacter = Instantiate(param_playerPrefab, transform.position, transform.rotation);
            playerCharacter.SetActive(true);
            _players.Add(_curentPlayerCount, playerCharacter);

            cameraManager.SpawnGameCamera(playerCharacter);
            if (_curentPlayerCount > 1)
                cameraManager.AdjustGameCamera(_curentPlayerCount);
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
                    cameraManager.AdjustGameCamera(_curentPlayerCount);
            }
        }

        [SerializeField] GameObject param_playerPrefab;
        Dictionary<int, GameObject> _players = new Dictionary<int, GameObject>();
        Dictionary<GameObject, GameObject> _gameCameras = new Dictionary<GameObject, GameObject>();

        int _curentPlayerCount = 0;

        void Start()
        {
            param_playerPrefab.SetActive(false);

            DontDestroyOnLoad(this);
        }

        public void Initialized()
        {
            cameraManager = FindObjectOfType<CameraManager>();

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnDestroy()
        {
        }

    }
}
