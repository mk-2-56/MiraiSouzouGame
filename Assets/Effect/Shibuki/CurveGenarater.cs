using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
[ExecuteInEditMode]
public class CurveGenerator : MonoBehaviour
{
    public VisualEffect visualEffect;
    public Vector3 startPoint;
    public Vector3 endPoint;
    public Vector3 controlPoint;
    public int segmentCount = 20;
    void Start()
    {
        visualEffect = gameObject.GetComponent<VisualEffect>();

        UpdateVFXProperties();
    }
    void Update()
    {
        UpdateVFXProperties();
    }
    void UpdateVFXProperties()
    {
        

        if (visualEffect != null)
        { 
            visualEffect.SetVector3("Start", startPoint); 
            visualEffect.SetVector3("End", endPoint);
            visualEffect.SetVector3("Control", controlPoint); 

        }
    }

    void OnDrawGizmos()
    {
        if (startPoint != null && endPoint != null && controlPoint != null)
        {

            // ラインの色を設定
            Gizmos.color = Color.green;

            Vector3 startPoint_R = startPoint;
            Vector3 endPoint_R = endPoint;
            Vector3 controlPoint_R = controlPoint;






            Vector3 startPoint_L = startPoint;
            Vector3 endPoint_L = endPoint;
            Vector3 controlPoint_L = controlPoint;




            startPoint_L.x *= -1;
            endPoint_L.x *= -1;
            controlPoint_L.x *= -1;


            startPoint_R = transform.TransformPoint(startPoint_R);
            endPoint_R = transform.TransformPoint(endPoint_R);
            controlPoint_R = transform.TransformPoint(controlPoint_R);


            startPoint_L = transform.TransformPoint(startPoint_L);
            endPoint_L = transform.TransformPoint(endPoint_L);
            controlPoint_L = transform.TransformPoint(controlPoint_L);



            // カーブの描画
            Vector3 previousPoint = startPoint_R;
            for (int i = 1; i <= segmentCount; i++)
            {
                float t = i / (float)segmentCount;
                Vector3 currentPoint = CurveUtility.SampleCurveBezier(startPoint_R, endPoint_R, controlPoint_R, t);
                Gizmos.DrawLine(previousPoint, currentPoint);
                previousPoint = currentPoint;

            }



            previousPoint = startPoint_L;
            for (int i = 1; i <= segmentCount; i++)
            {
                float t = i / (float)segmentCount;
                Vector3 currentPoint = CurveUtility.SampleCurveBezier(startPoint_L, endPoint_L, controlPoint_L, t);
                Gizmos.DrawLine(previousPoint, currentPoint);
                previousPoint = currentPoint;

            }

            // ラインの色を設定
            Gizmos.color = Color.blue;
            //ライン描画
            Gizmos.DrawLine(startPoint_R, controlPoint_R);
            Gizmos.DrawLine(controlPoint_R, endPoint_R);

            //ライン描画
            Gizmos.DrawLine(startPoint_L, controlPoint_L);
            Gizmos.DrawLine(controlPoint_L, endPoint_L);

        }
    }



}

