using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
#if UNITY_EDITOR
using UnityEditor;


[CustomEditor(typeof(SplinePath))]
public class SplinePathGUI : Editor
{


    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if( EditorGUILayout.LinkButton("Generate"))
        { 
            
        }
    }
}
#endif


public class SplinePath : MonoBehaviour
{
    [SerializeField] SplineContainer param_spline;
    //MeshRenderer _renderer;
    [SerializeField] Mesh param_mesh;
    Mesh _resultMesh;

    void Init()
    { 
        Vector3[] vertices = param_mesh.vertices;
    }

    void GenerateMesh()
    { 

    }
}
