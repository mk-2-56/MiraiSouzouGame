using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AU
{ 

    public class Math
    {
        static public float SmoothStep(float x, float min, float max)
        {

            float range = max - min;
            float d = (x - min);
            float k = Mathf.Clamp(d / range, 0, 1);

            return 3 * k * k - 2 * k * k * k;
        }
    }
}