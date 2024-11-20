using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace AU
{ 
    using UnityEditor;

    public class PlayerManager : MonoBehaviour
    {
        public void OnPlayerJoined(PlayerInput input)
        { 
            _curentPlayerCount++;
            GameObject player = input.gameObject;
            SpawnGameCamera(player);
            UnityEngine.Debug.Log("PlayerJoined");
            if (_curentPlayerCount > 1)
                AdjustGameCamera();
        }

        public void SpawnPlayer()
        {
            if (_curentPlayerCount > 1)
                return;

            _curentPlayerCount++;
            GameObject playerCharacter = Instantiate(param_playerPrefab, transform.position, transform.rotation);
            playerCharacter.SetActive(true);
            _players.Add(_curentPlayerCount, playerCharacter);

            SpawnGameCamera(playerCharacter);
            if (_curentPlayerCount > 1)
                AdjustGameCamera();
        }

        public void SpawnGameCamera(GameObject playerCharacter)
        {
            GameObject camera = Instantiate(param_cameraPrefab,
                playerCharacter.transform.position, playerCharacter.transform.rotation);

            var tmp = camera.GetComponent<GameCamera>();
            tmp.SetPlayerReference(playerCharacter); 
            camera.SetActive(true);
            _gameCameras.Add(playerCharacter, camera);
        }

        public void ClearResetPlayers()
        {
            foreach(KeyValuePair<int, GameObject> entry in _players)
            { 
                int key = entry.Key;
                if(entry.Value)
                { 
                    Destroy (entry.Value);
                }
                if(_gameCameras[entry.Value])
                    Destroy(_gameCameras[entry.Value]);
            }
            _curentPlayerCount = 0;
            _players.Clear();
            _gameCameras.Clear();
        }
        
        public void RemovePlayer(int index)
        { 
            if(_curentPlayerCount < 1)
                return;

            if(_players.ContainsKey(index))
            { 
                if(_gameCameras.ContainsKey(_players[index]))
                {
                    Destroy(_gameCameras[_players[index]]);
                    _gameCameras.Remove(_players[index]);
                }
                _players.Remove(index);
                Destroy(_players[index]);
                _curentPlayerCount--;
                if (_curentPlayerCount < 2)
                    AdjustGameCamera();
            }
        }

        [SerializeField] GameObject param_playerPrefab;
        [SerializeField] GameObject param_cameraPrefab;
    
        TargetManager    _manager;

        Dictionary<int, GameObject> _players = new Dictionary<int, GameObject>();
        Dictionary<GameObject, GameObject> _gameCameras = new Dictionary<GameObject, GameObject>();

        int _curentPlayerCount = 0;

        void Start()
        {
            param_playerPrefab.SetActive(false);
            param_cameraPrefab.SetActive(false);

            DontDestroyOnLoad(this);
        }
        // Update is called once per frame
        void Update()
        {
            
        }

        private void OnDestroy()
        {
        }

        private void AdjustGameCamera()
        {
            if(_curentPlayerCount > 1)
            { 
                int i = 0;
                foreach(KeyValuePair<GameObject, GameObject> item in _gameCameras)
                {
                    if (!item.Key)
                    {   
                        Destroy(item.Value);
                        _gameCameras.Remove(item.Key);
                        return;
                    }

                    Camera cam = item.Value.transform.Find("Camera").GetComponent<Camera>();
                    Vector2 pos  = new Vector2(0.0f, 0.5f) * i;
                    Vector2 size = new Vector2(1.0f, 0.5f);
                    cam.rect = new Rect(pos, size);
                    i++;
                }
            }
        }
    }
}
