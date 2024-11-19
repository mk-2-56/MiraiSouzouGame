using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public bool FindBestTarget(Vector3 positionPC, Vector3 inputDIreciton, out Vector3 target)
    {
        float closiestDot = 0f;
        target = Vector3.zero;
        Vector3 closiest = Vector3.zero;
        bool findCandidate = false;

        foreach (Vector3 pos in _targets.Values)
        //_targets.ForEach( pos => 
        {
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
    public void AddTarget(GameObject obj)
    {
        if(!_targets.ContainsKey(obj))
            _targets.Add(obj, obj.transform.position);
    }

    public void RemoveTarget(GameObject obj)
    {
        if(_targets.ContainsKey(obj))
        _targets.Remove(obj);
    }

    Dictionary<GameObject, Vector3> _targets = new Dictionary<GameObject, Vector3>();

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //foreach (GameObject obj in _targets.Keys)
        //{
        //}
    }
}
