using UnityEditor;
using UnityEditor.Splines;
using UnityEngine.Splines;

[CustomEditor(typeof(SplineWall))]
public class SplineWallEditor : Editor
{
    void OnEnable()
    {
        Spline.Changed += OnSplineChanged;
        EditorSplineUtility.AfterSplineWasModified += OnSplineModified;
        SplineContainer.SplineAdded += OnContainerSplineSetModified;
        SplineContainer.SplineRemoved += OnContainerSplineSetModified;
    }

    void OnDisable()
    {
        Spline.Changed -= OnSplineChanged;
        EditorSplineUtility.AfterSplineWasModified -= OnSplineModified;
        SplineContainer.SplineAdded -= OnContainerSplineSetModified;
        SplineContainer.SplineRemoved -= OnContainerSplineSetModified;
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        base.OnInspectorGUI();
        if (EditorGUI.EndChangeCheck())
        {
            if (target is SplineWall)
            {
               (target as SplineWall)?.PrepareComponents();
               (target as SplineWall)?.Rebuild();
            }
        }
    }

    private void OnSplineChanged(Spline spline, int knotIndex, SplineModification modificationType)
    {
        OnSplineModified();
    }

    private void OnSplineModified(Spline spline)
    {
        OnSplineModified();
    }

    private void OnContainerSplineSetModified(SplineContainer container, int spline)
    {
        OnSplineModified();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void OnSplineModified()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode)
        {
            // �v���C���[�h���Ȃ牽�����Ȃ�
            return;
        }

        // �{���͑Ώۂ�Spline���ҏW���ꂽ�Ƃ��������b�V�����Čv�Z�����������
        if (target is SplineWall component)
        {
            component.PrepareComponents();
            component.Rebuild();
        }
    }
}