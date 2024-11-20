using System.Collections.Generic;
using UnityEngine;

public class MarkManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private ObjectPool markPool; // マーク用のオブジェクトプール
    [SerializeField] private Transform worldSpaceCanvas; // マークを表示するWorld Space Canvas
    [SerializeField] private Camera mainCamera; // プレイヤーカメラ

    private List<Mark> activeMarks = new List<Mark>(); // アクティブなマークリスト

    private void Start()
    {
        // カメラの自動設定
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            if (mainCamera == null)
            {
                Debug.LogError("Main Camera is not assigned and could not be found!");
                return;
            }
        }

        // World Space Canvas のチェック
        if (worldSpaceCanvas == null)
        {
            Debug.LogError("World Space Canvas is not assigned!");
        }
    }

    private void Update()
    {
        // アクティブなマークを更新
        for (int i = activeMarks.Count - 1; i >= 0; i--)
        {
            Mark mark = activeMarks[i];
            if (mark != null)
            {
                mark.UpdateMark();
            }
            else
            {
                activeMarks.RemoveAt(i); // Nullがあればリストから削除
            }
        }
    }

    /// <summary>
    /// マークをターゲットに追加する
    /// </summary>
    /// <param name="target">ターゲットのTransform</param>
    /// <returns>追加したMarkオブジェクト</returns>
    public Mark AddMark(Transform target)
    {
        if (markPool == null)
        {
            Debug.LogError("MarkPool is not assigned!");
            return null;
        }

        GameObject markObject = markPool.GetObject();
        if (markObject == null)
        {
            Debug.LogWarning("No available objects in the pool!");
            return null;
        }

        markObject.transform.SetParent(worldSpaceCanvas, false);
        // ターゲットの上に配置
        Vector3 targetWorldPosition = target.position;
        markObject.transform.position = mainCamera.WorldToScreenPoint(targetWorldPosition); // ワールド座標 → スクリーン座標

        // スケールをリセット
        markObject.transform.localScale = Vector3.one;

        // カメラ方向に向ける（必要であれば）
        markObject.transform.LookAt(Camera.main.transform);


        Mark mark = markObject.GetComponent<Mark>();
        if (mark == null)
        {
            Debug.LogError("The object from the pool does not have a Mark component!");
            markPool.ReturnObject(markObject);
            return null; // エラー時は早期リターン
        }

        mark.Initialize(target, mainCamera); // マークを初期化
        activeMarks.Add(mark); // リストに追加

        return mark;
    }

    /// <summary>
    /// マークを削除してプールに戻す
    /// </summary>
    /// <param name="mark">削除するMarkオブジェクト</param>
    public void RemoveMark(Mark mark)
    {
        if (mark == null || !activeMarks.Contains(mark)) return;

        mark.ResetMark(); // マークをリセット
        markPool.ReturnObject(mark.gameObject); // プールに返却
        activeMarks.Remove(mark); // アクティブリストから削除
    }

   
}