using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CurveGenerator))]
public class CurveEditor : Editor
{
    void OnSceneGUI()
    {
        CurveGenerator curveGenerator = (CurveGenerator)target;
        EditorGUI.BeginChangeCheck();

        // 各ポイントにハンドルを表示
        if (curveGenerator.startPoint != null && curveGenerator.endPoint != null && curveGenerator.controlPoint != null)
        {
            Vector3 newStartPoint = Handles.PositionHandle(curveGenerator.transform.TransformPoint(curveGenerator.startPoint), Quaternion.identity);
            Vector3 newEndPoint = Handles.PositionHandle(curveGenerator.transform.TransformPoint(curveGenerator.endPoint), Quaternion.identity);
            Vector3 newControlPoint = Handles.PositionHandle(curveGenerator.transform.TransformPoint(curveGenerator.controlPoint), Quaternion.identity);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(curveGenerator, "Move Point");
                curveGenerator.startPoint = curveGenerator.transform.InverseTransformPoint(newStartPoint);
                curveGenerator.endPoint = curveGenerator.transform.InverseTransformPoint(newEndPoint);
                curveGenerator.controlPoint = curveGenerator.transform.InverseTransformPoint(newControlPoint);
                EditorUtility.SetDirty(curveGenerator);
            }
        }
    }
}
