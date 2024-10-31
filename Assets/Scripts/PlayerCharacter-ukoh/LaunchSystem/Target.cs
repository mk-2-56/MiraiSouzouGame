using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    TargetManager _manager;
    
    // Start is called before the first frame update
    void Start()
    {
        _manager = transform.parent.GetComponent<TargetManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        _manager.AddTarget(this.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        _manager.RemoveTarget(this.gameObject);
    }
}
