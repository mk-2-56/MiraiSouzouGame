using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EffectAnmator : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private Material material;
    public float index;
    void Start()
    {
        index = 0;
    }

    // Update is called once per frame
    void Update()
    {
        index += Time.deltaTime * 60.0f;
        material.SetFloat("_AnimIndex",index);
    }
}
