using System.Collections.Generic;
using UnityEngine;

public class MeteorsManager : MonoBehaviour
{
    [Header("Meteor Settings")]
    [SerializeField] private ObjectPool meteorPool;         // 隕石のオブジェクトプール
    [SerializeField] private Transform[] spawnPositions;    // スポーン地点を指定するTransform配列
    [SerializeField] private float spawnHeight = 50f;       // スポーンする高さ
    [SerializeField] private float fallSpeed = 10f;         // 落下速度
    [SerializeField] private float groundY = 0f;            // 地面の高さ（隕石が非表示になる高さ）
    private List<GameObject> activeMeteors = new List<GameObject>(); // 現在アクティブな隕石リスト

    private void Update()
    {
        // アクティブな隕石を処理
        for (int i = activeMeteors.Count - 1; i >= 0; i--)
        {
            GameObject meteor = activeMeteors[i];
            meteor.transform.position += Vector3.down * fallSpeed * Time.deltaTime;

            // 隕石が地面に到達したらプールに戻す
            if (meteor.transform.position.y <= groundY)
            {
                ResetMeteor(meteor);
            }
        }
    }

    public void ActivateMeteors()
    {
        // 各スポーン地点に隕石を配置してアクティブ化する
        foreach (Transform spawnPoint in spawnPositions)
        {
            Vector3 spawnPosition = spawnPoint.position;
            spawnPosition.y += spawnHeight; // スポーン高さを追加

            GameObject meteor = meteorPool?.GetObject(spawnPosition, Quaternion.identity);
            activeMeteors.Add(meteor); // アクティブな隕石リストに追加
            //markManager.AddMark(meteor.transform);
        }
    }

    /// <summary>
    /// 隕石を非アクティブ化してプールに戻す
    /// </summary>
    private void ResetMeteor(GameObject meteor)
    {
        //if (markManager != null)
        //{
        //    Mark mark = meteor.GetComponentInChildren<Mark>();
        //    if (mark != null)
        //    {
        //        markManager.RemoveMark(mark);
        //    }
        //}

        meteor.SetActive(false); // 非アクティブ化
        activeMeteors.Remove(meteor); // リストから削除
        meteorPool?.ReturnObject(meteor); // プールに戻す
    }

    // シーンビューにスポーン位置を視覚化
    private void OnDrawGizmos()
    {
        if (spawnPositions == null) return;

        Gizmos.color = Color.red;
        foreach (Transform spawnPoint in spawnPositions)
        {
            if (spawnPoint != null)
            {
                Gizmos.DrawSphere(new Vector3(spawnPoint.position.x, spawnPoint.position.y + spawnHeight, spawnPoint.position.z), 1.0f);
                Gizmos.DrawLine(spawnPoint.position, new Vector3(spawnPoint.position.x, spawnPoint.position.y + spawnHeight, spawnPoint.position.z));
            }
        }
    }
}