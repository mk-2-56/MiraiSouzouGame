using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// プレイヤー追跡Dashの標的管理
/// 
/// </summary>
public class TargetManager : MonoBehaviour
{
    public Dictionary<GameObject, Transform> GetTargetList()
    { 
        return _targets;
    }

    public bool FindBestTarget(Vector3 positionPC, Vector3 inputDIreciton, out Vector3 target)
    {//プレイヤーの操作方向に一致するtargetを優先して検索
        float closiestDot = 0f;
        target = Vector3.zero;
        Vector3 closiest = Vector3.zero;
        bool findCandidate = false;

        foreach (Transform xform in _targets.Values)
        {
            Vector3 pos = xform.position;
            float a = Vector3.Dot((pos - positionPC).normalized, inputDIreciton);
            if (a > closiestDot)
            {
                findCandidate = true;
                closiestDot = a;
                closiest = pos;
            }
        }
        //} );

        if (findCandidate)
            target = closiest;

        return findCandidate;
    }

    Dictionary<GameObject, Transform> _targets = new Dictionary<GameObject, Transform>();

    void AddTarget(GameObject obj)
    {
        if(!_targets.ContainsKey(obj))
            _targets.Add(obj, obj.transform);
    }

    void RemoveTarget(GameObject obj)
    {
        if(_targets.ContainsKey(obj))
        _targets.Remove(obj);
    }

    //void Update()
    //{
        
        //AU.Debug.Log("Active Targets: " + _targets.Count.ToString(), AU.LogTiming.Update);
        //foreach (GameObject obj in _targets.Keys)
        //{
            //AU.Debug.Log(obj.name, AU.LogTiming.Update);
        //}
    //}

    //LayerでTrigger可能な対象を制限する
    private void OnTriggerEnter(Collider other)
    {
        //途中追加:Dash機能破棄したため、もう一人のプレイヤーを探索することに流用
        if(other.gameObject.layer == LayerMask.NameToLayer("Targets"))
            AddTarget(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        RemoveTarget(other.gameObject);
    }
}
