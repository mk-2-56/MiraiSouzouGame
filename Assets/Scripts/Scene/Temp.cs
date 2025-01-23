using System.Collections;
using System.Collections.Generic;

using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEditor;


namespace AU
{
#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(Temp))]

    public class TempInspector : Editor
    {

        string coinLable = "Max: 0\tCollected: 0";

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button(new GUIContent("load")))
            {
                this.target.GetType().GetMethod("LoadGame").Invoke(target, null);
            }
            if (GUILayout.Button(new GUIContent("unload")))
            {
                this.target.GetType().GetMethod("UnloadGame").Invoke(target, null);
            }

            if (GUILayout.Button(new GUIContent("coin" + coinLable)))
            {
                int max, collected;
                Coin.GetCoinNumber(out max, out collected);
                coinLable = "Max: " + max.ToString() + "\tCollected: " + collected.ToString();
            }
        }
    }
#endif


    public class Temp : MonoBehaviour
    {
    
        // Start is called before the first frame update
        void Start()
        {
            LoadGame();
        }

        public void LoadGame()
        {
            SceneManager.LoadScene("Game", LoadSceneMode.Additive);
        }
        
        public void UnloadGame()
        {
            SceneManager.UnloadSceneAsync("Game");
        }
    }
}