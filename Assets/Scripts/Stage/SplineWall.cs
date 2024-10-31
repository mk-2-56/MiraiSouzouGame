using System;
using System.Runtime.InteropServices;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Splines;

[StructLayout(LayoutKind.Sequential)]
struct VertexData
{
    public Vector3 Position { get; set; }
}

[RequireComponent(typeof(SplineContainer), typeof(MeshFilter))]
public class SplineWall : MonoBehaviour
{
    [SerializeField, Range(2, 100)] private int divided = 10;
    [SerializeField] private float height = 5.0f;

    private SplineContainer splineContainer;
    private Mesh mesh;
    private MeshFilter meshFilter;

    private void Reset()
    {
        PrepareComponents();
        Rebuild();
    }

    public void PrepareComponents()
    {
        splineContainer = GetComponent<SplineContainer>();
        meshFilter = GetComponent<MeshFilter>();
        mesh = new Mesh();
        meshFilter.sharedMesh = mesh;
    }

    public void Rebuild()
    {
        Spline spline = splineContainer.Spline;
        if (spline == null) return;

        mesh.Clear();
        var meshDataArray = Mesh.AllocateWritableMeshData(1);
        var meshData = meshDataArray[0];
        meshData.subMeshCount = 1;

        // 頂点数とインデックス数を計算する
        var vertexCount = 2 * (divided + 1);
        var indexCount = 6 * divided;

        // インデックスと頂点のフォーマットを指定する
        var indexFormat = IndexFormat.UInt32;
        meshData.SetIndexBufferParams(indexCount, indexFormat);
        meshData.SetVertexBufferParams(vertexCount, new VertexAttributeDescriptor[]
        {
            new(VertexAttribute.Position),
        });

        var vertices = meshData.GetVertexData<VertexData>();
        var indices = meshData.GetIndexData<UInt32>();

        for (int i = 0; i <= divided; ++i)
        {
            // 頂点座標を計算する
            spline.Evaluate((float)i / divided, out var position, out var direction, out var splineUp);
            var p0 = vertices[2 * i];
            var p1 = vertices[2 * i + 1];
            p0.Position = position;
            p1.Position = position + new float3(0, height, 0);
            vertices[2 * i] = p0;
            vertices[2 * i + 1] = p1;
        }

        for (int i = 0; i < divided; ++i)
        {
            indices[6 * i + 0] = (UInt32)(2 * i + 0);
            indices[6 * i + 1] = (UInt32)(2 * i + 1);
            indices[6 * i + 2] = (UInt32)(2 * i + 2);
            indices[6 * i + 3] = (UInt32)(2 * i + 1);
            indices[6 * i + 4] = (UInt32)(2 * i + 3);
            indices[6 * i + 5] = (UInt32)(2 * i + 2);
        }

        meshData.SetSubMesh(0, new SubMeshDescriptor(0, indexCount));

        Mesh.ApplyAndDisposeWritableMeshData(meshDataArray, mesh);
        mesh.RecalculateBounds();
    }
}