using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

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
    [SerializeField] Transform param_direction;
    [SerializeField] GameObject param_VFXprefab;
    [SerializeField] float param_VFXfollowFade = 0.7f;

    Vector3 _direction;
    Rigidbody _other;
    VisualEffect _effct;

    float minAcc = 10.0f;

    private float HeightAdjust = 6.0f;

    private void Start()
    {
        if (param_direction == null)
            param_direction = transform;
        _direction = param_direction.forward;

        _effct = Instantiate(param_VFXprefab, transform.positionÅ@+ new Vector3(0.0f, HeightAdjust, 0.0f), transform.rotation).GetComponent<VisualEffect>();
        _effct.enabled = false;
    }

    void Burst()
    {
        float accMag = Mathf.Max(minAcc, param_targetSpeed - Vector3.Dot(_other.velocity, _direction));
        _other.AddForce(_direction * accMag, ForceMode.VelocityChange);
    }

    void Acclerate(Rigidbody other, Quaternion terrrianRot)
    {

        Vector3 terrianAcc = terrrianRot * _direction;
        float accMag = Mathf.Max(minAcc, param_targetSpeed - Vector3.Dot(other.velocity, terrianAcc));
        Vector3 acc = other.velocity.normalized * param_targetSpeed;
        other.AddForce(accMag * terrianAcc, ForceMode.Acceleration);
    }

    void StartAccelerate(CC.Hub other)
    {
        other.FixedEvent += Acclerate;
    }
    void EndAccelerate(CC.Hub other)
    {
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

        //Sound
        if (param_direction == null)
        {
            SoundManager.Instance?.PlaySE(SESoundData.SE.SE_BoostTile);
        }
        else
        {
            SoundManager.Instance?.PlaySE(SESoundData.SE.SE_BoostTile);
        }
        //Effect
        _effct.enabled = true;
        _effct.transform.rotation = Quaternion.LookRotation(other.attachedRigidbody.velocity, Vector3.up);
        StartCoroutine(EffectUpdate(other.transform, other.transform.position + new Vector3(0.0f, HeightAdjust, 0.0f) + other.transform.forward * param_targetSpeed * Time.fixedDeltaTime));
        _effct.Play();
        other.GetComponent<PlayerCanvasController>().EnableFocusEffect();//èWíÜê¸
    }


    private void OnTriggerExit(Collider other)
    {
        if (param_mode == BoostMode.Continous)
            EndAccelerate(other.GetComponent<CC.Hub>());
    }
    void EndEffect()
    {
        _effct.transform.SetParent(transform);
        _effct.enabled = false;
        _effct.transform.position = transform.position;
    }

    IEnumerator EffectUpdate(Transform tar, Vector3 start)
    {
        float t = 1.0f;
        StartCoroutine(AU.Fader.FadeOut(t, value => { t = value; }, param_VFXfollowFade));
        while (t > 0.0f)
        {
            _effct.transform.position = Vector3.Lerp(start, tar.position, Mathf.Min(t + 0.2f, 1.0f));
            AU.Debug.Log(t, AU.LogTiming.Fixed);
            yield return new WaitForFixedUpdate();
        }
        EndEffect();
    }
}

