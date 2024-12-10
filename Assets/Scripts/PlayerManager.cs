using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using Cinemachine;
namespace AU
{
    using CC;
    using Unity.VisualScripting;
    using UnityEditor;
    using UnityEditor.Purchasing;

    public class PlayerManager : MonoBehaviour
    {
        private CameraManager cameraManager;
        //[SerializeField] List<CinemachineSmoothPath> dollyPaths;
        //List<CinemachineDollyCart>  dollyCarts;
        //Dictionary<int, List<CinemachineDollyCart>> _playerDollyCarts;
        //[SerializeField] private float dollySpeed;
        [SerializeField] GameObject respawnPos;
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

            cameraManager.SpawnGameCamera(player);
            _players.Add(_curentPlayerCount, player);

            if (_curentPlayerCount > 1)
                cameraManager.AdjustGameCamera(_curentPlayerCount);
        }

        public void SpawnPlayer()
        {
            if (_curentPlayerCount > 1)
                return;

            _curentPlayerCount++;

            GameObject playerCharacter;// = Instantiate(param_playerPrefab, transform.position, transform.rotation);

            if (respawnPos) playerCharacter = Instantiate(param_playerPrefab, transform.position, transform.rotation);
            else playerCharacter = Instantiate(param_playerPrefab, respawnPos.transform.position, respawnPos.transform.rotation);

            //{//CineMachine処理
            //    foreach (CinemachineSmoothPath path in dollyPaths)
            //    {
            //        CinemachineDollyCart dollyCart = playerCharacter.AddComponent<CinemachineDollyCart>();
            //        // Dolly Cartの設定
            //        dollyCart.m_Path = path; // スプラインを設定
            //        dollyCart.m_Position = 0; // スプラインの開始位置
            //        dollyCart.m_Speed = 0; // 移動速度を設定
            //        dollyCart.m_UpdateMethod = CinemachineDollyCart.UpdateMethod.FixedUpdate;
            //        dollyCarts.Add(dollyCart);
            //    }
            //    _playerDollyCarts[_curentPlayerCount] = dollyCarts;
            //    playerCharacter.GetComponent<TrackFollower>()?.Initialized();
            //}
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

        public int _curentPlayerCount = 0;

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

            if (Input.GetKeyDown(KeyCode.V))
            {
                cameraManager.AdjustGameCamera(_curentPlayerCount);
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
              SetPlayerControl(false);

            }
            UnityEngine.Debug.Log("players" + _players.Count);



        }

        private void OnDestroy()
        {
        }

        //public List<CinemachineSmoothPath> GetPath()
        //{
        //    return dollyPaths;
        //}

        //public float GetDollySpeed()
        //{
        //    return dollySpeed;
        //}

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
    }

}

