using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputLogger : MonoBehaviour
{
    MsgBuffer debugText;
    // Start is called before the first frame update
    void Start()
    {
        debugText = AU.Debug.GetMsgBuffer();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
