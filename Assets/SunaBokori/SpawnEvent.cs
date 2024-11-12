using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SpawnEvent : MonoBehaviour
{
    public VisualEffect vfx;
    // Start is called before the first frame update
    void Start()
    {
        if (vfx == null)
        {
            vfx = GetComponent<VisualEffect>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            vfx.SendEvent("Spawn");

        }
    }

    public void SendEvent()
    {
        // VFX Graphにイベントを送信
        vfx.SendEvent("Spawn");

    }
}
