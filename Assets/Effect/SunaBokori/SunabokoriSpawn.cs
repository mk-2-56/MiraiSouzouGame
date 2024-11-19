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
        // àÍíUç≈èâÇ…î≠ê∂Ç∑ÇÈÇÊÇ§Ç…ÇµÇƒÇ‹Ç∑ÅiçÇã¥Åj
        vfxObj.GetComponent<VisualEffect>().SendEvent("Spawn");

    }

    // Update is called once per frame
    void Update()
    {
        // vfxObj.GetComponent<VisualEffect>().SendEvent("Spawn");
    }
}
