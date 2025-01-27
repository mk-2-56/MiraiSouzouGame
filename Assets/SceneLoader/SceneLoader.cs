using AU;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//#if UNITY_EDITOR
//using UnityEditor;

//namespace AU
//{
//    using UnityEditor;

//    [CustomEditor(typeof(Temp))]

//    public class TempInspector : Editor
//    {

//        string coinLable = "Max: 0\tCollected: 0";

//        public override void OnInspectorGUI()
//        {
//            DrawDefaultInspector();

//            if (GUILayout.Button(new GUIContent("load")))
//            {
//                this.target.GetType().GetMethod("LoadGame").Invoke(target, null);
//            }
//            if (GUILayout.Button(new GUIContent("unload")))
//            {
//                this.target.GetType().GetMethod("UnloadGame").Invoke(target, null);
//            }

//            if (GUILayout.Button(new GUIContent("coin" + coinLable)))
//            {
//                int max, collected;
//                Coin.GetCoinNumber(out max, out collected);
//                coinLable = "Max: " + max.ToString() + "\tCollected: " + collected.ToString();
//            }
//        }
//    }
//#endif


public class SceneLoader : MonoBehaviour
{
    [SerializeField] private GameObject outUI;
    [SerializeField] private GameObject ui;

    [SerializeField] Slider slider;
    [SerializeField] string NextScene;
    private bool gameEnd;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "GameBase")
        {
            Scene result = SceneManager.GetSceneByName("Result");
            if (result.isLoaded) return;
            SceneManager.LoadScene("Tutorial", LoadSceneMode.Additive);
            SceneManager.LoadScene("Game", LoadSceneMode.Additive);
        }

    }
    private void Update()
    {
        if(SceneManager.GetActiveScene().name == "Title")
        {
            if ((Input.GetKeyDown("joystick button 0")) || (Input.GetKeyDown("joystick button 1")) || (Input.GetKeyDown("joystick button 7")))
            {
                Load(NextScene);
            }
        }

        else if(SceneManager.GetActiveScene().name == "Result")
        {
            if ((Input.GetKeyDown("joystick button 0")) || (Input.GetKeyDown("joystick button 1")) || (Input.GetKeyDown("joystick button 7")))
            {
                Load(NextScene);
            }
        }
        

    }
    public void Load(string sceneName)
    {
        
        StartCoroutine(LoadScene());

    }

    public void LoadAdditive(string sceneName)
    {

        StartCoroutine(LoadSceneAdditive(sceneName));

    }
    //public void Load()
    //{

    //    StartCoroutine(LoadScene());

    //}
    IEnumerator LoadScene()
    {
        // BGMをフェードアウト
        SoundManager.Instance?.FadeOutAllSounds(1f);

        SoundManager.Instance?.PlaySE(SESoundData.SE.SE_SceneSwith);
        SoundManager.Instance?.PlaySE(SESoundData.SE.SE_SceneLoading);

        outUI.SetActive(true);


        yield return new WaitForSeconds((1.0f/60.0f)*80.0f);
        ui.SetActive(true);
        

        UnityEngine.Debug.Log("NextSceneis" + NextScene);
        if (SceneManager.GetSceneByName(NextScene) == null)
        {
            UnityEngine.Debug.Log("NextSceneisNull");
            yield return null;
        }


        AsyncOperation aSync = SceneManager.LoadSceneAsync(NextScene);
        SoundManager.Instance?.SetMasterVolume(1);
        SoundManager.Instance?.SetBGMVolume(1);
        SoundManager.Instance?.SetSEVolume(1);
        while (!aSync.isDone)
        {
            slider.value = aSync.progress;

            yield return null;
        }

    }

    IEnumerator LoadSceneAdditive(string sceneName)
    {
        // BGMをフェードアウト
        SoundManager.Instance?.FadeOutAllSounds(1); // 1.5秒でフェードアウト

        SoundManager.Instance?.PlaySE(SESoundData.SE.SE_SceneSwith);
        SoundManager.Instance?.PlaySE(SESoundData.SE.SE_SceneLoading);

        outUI.SetActive(true);


        yield return new WaitForSeconds((1.0f / 60.0f) * 80.0f);
        ui.SetActive(true);


        UnityEngine.Debug.Log("NextSceneis" + sceneName);
        if (SceneManager.GetSceneByName(sceneName) == null)
        {
            UnityEngine.Debug.Log("NextSceneisNull");
            yield return null;
        }


        AsyncOperation aSync = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        SoundManager.Instance?.SetMasterVolume(1);
        SoundManager.Instance?.SetBGMVolume(1);
        SoundManager.Instance?.SetSEVolume(1);
        while (!aSync.isDone)
        {
            slider.value = aSync.progress;

            yield return null;
        }

    }

    public void SetGameEnd(bool end)
    {
        //if (SceneManager.GetActiveScene().name == "Game")
        {
            Load(NextScene);
            //SceneManager.UnloadSceneAsync("Game");
        }
    }
}
