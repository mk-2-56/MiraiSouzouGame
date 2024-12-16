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

    Vector3 _direction;
    Rigidbody _other;
    VisualEffect _effct;

    float minAcc = 10.0f;

    private void Start()
    {
        if (param_direction == null)
            param_direction = transform;
        _direction = param_direction.forward;

        _effct = Instantiate(param_VFXprefab, transform.position, transform.rotation).GetComponent<VisualEffect>();
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

        _effct.transform.position = other.transform.position;
        _effct.transform.rotation = Quaternion.LookRotation(other.attachedRigidbody.velocity, Vector3.up);
        _effct.Play();
    }

    private void OnTriggerExit(Collider other)
    {
        if (param_mode == BoostMode.Continous)
            EndAccelerate(other.GetComponent<CC.Hub>());
    }
}

