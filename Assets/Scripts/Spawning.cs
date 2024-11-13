using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawning : MonoBehaviour
{
    [SerializeField] GameObject prefab;

    TargetManager _manager;

    void Start()
    {
        Invoke("Spawn", 0.5f);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    void Spawn()
    { 
        GameObject playerCharacter = Instantiate(prefab, transform);
        playerCharacter.transform.SetParent(null);
    }
}
