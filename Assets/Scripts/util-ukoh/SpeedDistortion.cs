using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SpeedDistortion : MonoBehaviour
{
    [SerializeField] CCT_Basic _rPCController;
    Volume volume;
    ChromaticAberration distortion;

    // Start is called before the first frame update
    void Start()
    {
        volume = GetComponent<Volume>();
        ChromaticAberration tmp;
        if (volume.profile.TryGet<ChromaticAberration>(out tmp))
        {
            distortion = tmp;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float intensity = Mathf.Max(_rPCController.Speed - 70, 0) / (_rPCController.Speed - 30);
        distortion.intensity.value = intensity;
    }
}
