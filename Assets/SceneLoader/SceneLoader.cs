using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SceneLoader : MonoBehaviour
{
    [SerializeField] private GameObject outUI;
    [SerializeField] GameObject ui;
    [SerializeField] Slider slider;
    [SerializeField] string NextScene;
    private string sceneName;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Load();
        }

        

    }
    public void Load(string sceneName)
    {
        
        NextScene = sceneName;
        StartCoroutine(LoadScene());

    }
    public void Load()
    {

        StartCoroutine(LoadScene());

    }
    IEnumerator LoadScene()
    {
        SoundManager.Instance.PlaySE(SESoundData.SE.SE_SceneSwith);
        SoundManager.Instance.PlaySE(SESoundData.SE.SE_SceneLoading);

        outUI.SetActive(true);


        yield return new WaitForSeconds((1.0f/60.0f)*80.0f);
        ui.SetActive(true);
        

        Debug.Log("NextSceneis" + NextScene);
        if (SceneManager.GetSceneByName(NextScene) == null)
        {
            Debug.Log("NextSceneisNull");
            yield return null;
        }



        AsyncOperation aSync = SceneManager.LoadSceneAsync(NextScene);

        while (!aSync.isDone)
        {
            slider.value = aSync.progress;

            yield return null;
        }

    }

}
