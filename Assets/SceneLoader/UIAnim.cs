using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIAnim : MonoBehaviour
{
    private float AnimIndex;
    [SerializeField] private Material targetMaterial;


    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.PlaySE(SESoundData.SE.SE_SceneSwith);
        AnimIndex = 0;
        targetMaterial.SetFloat("_AnimIndex", 0);

    }

    // Update is called once per frame
    void Update()
    {
        AnimIndex += Time.deltaTime * 60.0f;
        targetMaterial.SetFloat("_AnimIndex", AnimIndex);

    }
}
