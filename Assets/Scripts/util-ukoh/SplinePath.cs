using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using UnityEditor;

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
