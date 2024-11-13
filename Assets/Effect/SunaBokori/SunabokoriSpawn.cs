using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class SunabokoriSpawn : MonoBehaviour
{
    public GameObject vfxObj;

    // Start is called before the first frame update
    void Start()
    {
        // 一旦最初に発生するようにしてます（高橋）
        vfxObj.GetComponent<VisualEffect>().SendEvent("Spawn");

    }

    // Update is called once per frame
    void Update()
    {
        // vfxObj.GetComponent<VisualEffect>().SendEvent("Spawn");
    }
}
