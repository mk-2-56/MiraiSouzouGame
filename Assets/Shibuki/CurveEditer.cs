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
            Vector3 newStartPoint = Handles.PositionHandle(curveGenerator.startPoint+curveGenerator.transform.position, Quaternion.identity);
            Vector3 newEndPoint = Handles.PositionHandle(curveGenerator.endPoint + curveGenerator.transform.position, Quaternion.identity);
            Vector3 newControlPoint = Handles.PositionHandle(curveGenerator.controlPoint + curveGenerator.transform.position, Quaternion.identity);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(curveGenerator, "Move Point");

                curveGenerator.startPoint = newStartPoint - curveGenerator.transform.position;
                curveGenerator.endPoint = newEndPoint - curveGenerator.transform.position;
                curveGenerator.controlPoint = newControlPoint - curveGenerator.transform.position;

                EditorUtility.SetDirty(curveGenerator);
            }
        }
    }
}
