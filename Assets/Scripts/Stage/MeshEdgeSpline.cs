using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Splines;

public class MeshEdgeSpline : MonoBehaviour
{
    [SerializeField] float makeWidthShorter;
    private List<Vector3> leftEdgePoints = new List<Vector3>(); // ���[�̃|�C���g���i�[���郊�X�g
    private List<Vector3> rightEdgePoints = new List<Vector3>(); // �E�[�̃|�C���g���i�[���郊�X�g
    private RamSpline ramSpline; // RamSpline�̃C���X�^���X

    private void CalculateEdgePoints()
    {
        leftEdgePoints.Clear();
        rightEdgePoints.Clear();

        if (ramSpline == null || ramSpline.points == null || ramSpline.points.Count == 0)
        {
            Debug.LogWarning("RamSpline��controlPoints���X�g����ł��B");
            return;
        }

        var controlPoints = ramSpline.points;
        float meshWidth = ramSpline.width - makeWidthShorter;

        for (int i = 0; i < controlPoints.Count - 1; i++)
        {
            Vector3 currentPoint = controlPoints[i];
            Vector3 nextPoint = controlPoints[i + 1];

            Vector3 direction = (nextPoint - currentPoint).normalized;
            Vector3 perpendicular = new Vector3(-direction.z, 0, direction.x).normalized;

            Vector3 leftEdge = currentPoint + perpendicular * (meshWidth / 2);
            Vector3 rightEdge = currentPoint - perpendicular * (meshWidth / 2);

            leftEdgePoints.Add(leftEdge);
            rightEdgePoints.Add(rightEdge);
        }
    }

    public void CreateSplinesInHierarchy()
    {
        // RamSpline�R���|�[�l���g���擾
        ramSpline = GetComponent<RamSpline>();
        if (ramSpline == null)
        {
            Debug.LogError("RamSpline�R���|�[�l���g��������܂���ł����B");
            return;
        }

        CalculateEdgePoints();

        // ���[�̃X�v���C���R���e�i���q�G�����L�[�ɐ���
        var leftSplineObj = new GameObject("LeftEdgeSpline");
        var leftSplineContainer = leftSplineObj.AddComponent<SplineContainer>();
        Spline leftSpline = new Spline();

        foreach (var point in leftEdgePoints)
        {
            leftSpline.Add(new BezierKnot(point));
        }
        leftSplineContainer.Spline = leftSpline;

        // �E�[�̃X�v���C���R���e�i���q�G�����L�[�ɐ����i�t���j
        var rightSplineObj = new GameObject("RightEdgeSpline");
        var rightSplineContainer = rightSplineObj.AddComponent<SplineContainer>();
        Spline rightSpline = new Spline();

        for (int i = rightEdgePoints.Count - 1; i >= 0; i--)
        {
            rightSpline.Add(new BezierKnot(rightEdgePoints[i]));
        }
        rightSplineContainer.Spline = rightSpline;

        leftSplineObj.transform.parent = this.transform;
        rightSplineObj.transform.parent = this.transform;
    }
}

[CustomEditor(typeof(MeshEdgeSpline))]
public class MeshEdgeSplineEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MeshEdgeSpline splineScript = (MeshEdgeSpline)target;
        if (GUILayout.Button("���[�̃X�v���C�����쐬����"))
        {
            splineScript.CreateSplinesInHierarchy();
        }
    }
}
