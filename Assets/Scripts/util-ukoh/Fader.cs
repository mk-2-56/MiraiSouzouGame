using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AU
{
    public class Fader : MonoBehaviour
    {
        static public IEnumerator FadeIn(float v, System.Action<float> setter, float duration)
        {
            float increment = 1.0f / duration;

            for (float f = v; f < 1; f += increment * Time.deltaTime)
            {
                setter(Mathf.Min(f, 1));
                yield return new WaitForFixedUpdate();
            }
        }
        static public IEnumerator FadeOut(float v, System.Action<float> setter, float duration)
        {
            float increment = 1.0f / duration;

            for (float f = v; f > 0; f -= increment * Time.deltaTime)
            {

                setter(Mathf.Max(f, 0));
                yield return new WaitForFixedUpdate();
            }
        }
    }
}
