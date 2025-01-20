using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.Pool;


public class BoostTile : MonoBehaviour
{
    enum BoostMode
    {
        Burst,
        Continous,
    }

    [SerializeField] BoostMode param_mode;
    [SerializeField] float param_targetSpeed;
    [SerializeField] float param_acc;
    [SerializeField] float param_duration;
    [SerializeField] Transform param_direction;

    [SerializeField] GameObject param_effectPrefab;
    [SerializeField] float param_effectFollowFade = 0.7f;

    Vector3 _direction;
    Rigidbody _other;
    VisualEffect _effct;

    float minAcc = 10.0f;

    static List<VisualEffect> _effects = new List<VisualEffect>();

    private void Start()
    {
        if (param_direction == null)
            param_direction = transform;
        _direction = param_direction.forward;
    }

    void Burst()
    {
        float accMag = Mathf.Max(minAcc, param_targetSpeed - Vector3.Dot(_other.velocity, _direction));
        _other.AddForce(_direction * accMag, ForceMode.VelocityChange);
    }
    void Acclerate(Rigidbody other, CC.PlayerMovementParams param)
    {
        float accMag = Mathf.Min(param_acc, Mathf.Max(0, param_targetSpeed - param.xzSpeed));
        Vector3 acc = other.velocity.normalized * accMag;
        other.AddForce(acc, ForceMode.Acceleration);
    }

    void StartAccelerate(CC.Hub other)
    {
        other.FixedEvent += Acclerate;
        StartCoroutine("EndAccelerate", other);
    }

    IEnumerable EndAccelerate(CC.Hub other)
    {
        yield return new WaitForSeconds(param_duration);
        other.FixedEvent -= Acclerate;
    }



    private void OnTriggerEnter(Collider other)
    {
        _other = other.GetComponent<Rigidbody>();
        switch (param_mode)
        {
            case BoostMode.Burst:
                Burst();
                break;

            case BoostMode.Continous:
                StartAccelerate(other.GetComponent<CC.Hub>());
                break;
        }
        StartEffect(other);
    }

    void StartEffect(Collider other)
    {
        VisualEffect effect = null;
        foreach (VisualEffect ins in _effects)
        {
            if (ins.enabled == false)
            {
                effect = ins;
                break;
            }
        }

        if (effect == null)
        {
            effect = AddEffect(param_effectPrefab);
        }

        effect.enabled = true;
        effect.transform.rotation = Quaternion.LookRotation(other.attachedRigidbody.velocity, Vector3.up);
        StartCoroutine(EffectUpdate(effect,
            other.transform.position + other.transform.forward * param_targetSpeed * Time.fixedDeltaTime,
            other.transform));
        effect.Play();
    }

    void EndEffect(VisualEffect effect)
    {
        effect.transform.SetParent(transform);
        effect.enabled = false;
        effect.transform.position = transform.position;
    }

    IEnumerator EffectUpdate(VisualEffect effect, Vector3 start, Transform tar)
    {
        float t = 1.0f;
        StartCoroutine(AU.Fader.FadeOut(t, value => { t = value; }, param_effectFollowFade));
        while (t > 0)
        {
            effect.transform.position = Vector3.Lerp(start, tar.position, Mathf.Min(t + 0.2f, 1.0f));
            yield return new WaitForFixedUpdate();
        }
        EndEffect(effect);
    }

    static VisualEffect AddEffect(GameObject prefab)
    {
        VisualEffect newIns = Instantiate(prefab).GetComponent<VisualEffect>();
        newIns.enabled = false;
        _effects.Add(newIns);
        return newIns;
    }
}
