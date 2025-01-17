using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EnergyShield : MonoBehaviour
{
    [SerializeField] private float shakeStrength = 0.03f;
    [SerializeField] private float shakeDuration = 0.25f;
    [SerializeField] private float rippleCooldown = 0.4f;
    [SerializeField] private VisualEffect sparks;

    private Material material;
    private float rippleTime = 100.0f;
    private Coroutine shakeRoutine;
    private Vector3 originalPosition;

    private void Start()
    {
        material = GetComponent<Renderer>().material;
        sparks.enabled = false;
    }

    //public void GetHit(RaycastHit hit)
    //{
    //    if(rippleTime < rippleCooldown)
    //    {
    //        return;
    //    }

    //    material.SetVector("_RippleOrigin", hit.textureCoord);
    //    rippleTime = material.GetFloat("_RippleThickness") * -2.0f;

    //    if (shakeRoutine != null)
    //    {
    //        StopCoroutine(shakeRoutine);
    //        transform.position = originalPosition;
    //    }
    //    originalPosition = transform.position;
    //    shakeRoutine = StartCoroutine(Shake(hit));
    //}

    public void GetHit(ContactPoint contact)
    {
        if (rippleTime < rippleCooldown)
        {
            return;
        }

        // UVç¿ïWÇåvéZ
        Renderer rend = contact.thisCollider.GetComponent<Renderer>();
        if (rend == null || rend.material.mainTexture == null)
        {
            Debug.LogWarning("Renderer or Texture not found for UV mapping.");
            return;
        }

        MeshCollider meshCollider = contact.thisCollider as MeshCollider;
        if (meshCollider == null || meshCollider.sharedMesh == null)
        {
            Debug.LogWarning("MeshCollider or Mesh not found.");
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(contact.point + contact.normal * 0.01f, -contact.normal, out hit))
        {
            // UVç¿ïWÇéÊìæ
            Vector2 uv = hit.textureCoord;
            material.SetVector("_RippleOrigin", uv);
        }
        else
        {
            Debug.LogWarning("Failed to raycast for UV mapping.");
            return;
        }

        rippleTime = material.GetFloat("_RippleThickness") * -2.0f;

        if (shakeRoutine != null)
        {
            StopCoroutine(shakeRoutine);
            transform.position = originalPosition;
        }
        originalPosition = transform.position;
        shakeRoutine = StartCoroutine(Shake(hit));
    }



    private void Update()
    {
        rippleTime += Time.deltaTime;
        material.SetFloat("_RippleTime", rippleTime);
    }

    private IEnumerator Shake(RaycastHit hit)
    {
        sparks.transform.position = hit.point;
        sparks.transform.rotation = Quaternion.LookRotation(Vector3.up, hit.normal);
        sparks.enabled = true;
        sparks.Play();

        for(float t = 0.0f; t < shakeDuration; t += Time.deltaTime)
        {
            transform.position = originalPosition + Random.insideUnitSphere * shakeStrength;
            yield return null;
        }

        transform.position = originalPosition;
    }
}
