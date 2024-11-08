using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CurveUtility
{
    public static Vector3 SampleCurveBezier(Vector3 start, Vector3 end, Vector3 control, float t)
    {
        Vector3 Q0 = Vector3.Lerp(start, control, t);
        Vector3 Q1 = Vector3.Lerp(control, end, t);
        Vector3 Q2 = Vector3.Lerp(Q0, Q1, t);
        return Q2;
    }

}
