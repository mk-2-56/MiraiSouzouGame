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
            _currentPlayerCount++;
            GameObject player = input.gameObject;
            LayerMask mask = LayerMask.GetMask("Terrian");
            RaycastHit hit;
            Physics.SphereCast(transform.position, 10.0f, Vector3.down, out hit, 100.0f, mask);
            player.transform.position = hit.point;
            player.transform.rotation = transform.rotation;

            GameObject camera = SpawnGameCamera(player);

            var tmanager = player.AddComponent<TargetManager>();
            camera.GetComponent<TargetUI>().SetTargetManager(tmanager);

            if (_currentPlayerCount > 1)
                AdjustGameCamera();
        }

        public void SpawnPlayer()
        {
            if (_currentPlayerCount > 1)
                return;

            _currentPlayerCount++;
            GameObject playerCharacter = Instantiate(param_playerPrefab, transform.position, transform.rotation);
            playerCharacter.SetActive(true);
            _players.Add(_currentPlayerCount, playerCharacter);

            SpawnGameCamera(playerCharacter);
            if (_currentPlayerCount > 1)
                AdjustGameCamera();
        }

        public GameObject SpawnGameCamera(GameObject playerCharacter)
        {
            GameObject camera = Instantiate(param_cameraPrefab,
                playerCharacter.transform.position, playerCharacter.transform.rotation);

            var tmp = camera.GetComponent<GameCamera>();

            tmp.SetPlayerReference(playerCharacter);
            camera.SetActive(true);
            _gameCameras.Add(playerCharacter, camera);
            return camera;
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
            _currentPlayerCount = 0;
            _players.Clear();
            _gameCameras.Clear();
        }
        
        public void RemovePlayer(int index)
        { 
            if(_currentPlayerCount < 1)
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
                _currentPlayerCount--;
                if (_currentPlayerCount < 2)
                    AdjustGameCamera();
            }
        }

        [SerializeField] GameObject param_playerPrefab;
        [SerializeField] GameObject param_cameraPrefab;

        //Key:      playerindex,  Value: PlayerGameObject
        Dictionary<int, GameObject> _players = new Dictionary<int, GameObject>();
        //Key: PlayerGameObject,  Value: CameraGameObject
        Dictionary<GameObject, GameObject> _gameCameras = new Dictionary<GameObject, GameObject>();

        int _currentPlayerCount = 0;

        void Start()
        {
            param_playerPrefab.SetActive(false);
            param_cameraPrefab.SetActive(false);
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
            if(_currentPlayerCount > 1)
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
