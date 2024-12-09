using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PostEffectCotrol : MonoBehaviour
{
    [SerializeField] private FullScreenPassRendererFeature postProcess;

    // Start is called before the first frame update
    void Start()
    {
        postProcess.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.K))
        {
            postProcess.SetActive(true);
        }
    }

    void OnApplicationQuit()
    {
        postProcess.SetActive(false);

    }
}
