using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostTile : MonoBehaviour
{
    enum BoostMode
    { 
        Burst,
        Continous,
    }

    [SerializeField] BoostMode param_mode;
    [SerializeField] Transform param_direction;
    [SerializeField] float     param_targetSpeed;
    [SerializeField] float     param_acc;
    [SerializeField] float     param_lingering = 3.0f;

    Vector3 _direction;
    Rigidbody _other;

    float minAcc = 10.0f;

    private void Start()
    {
        if(param_direction == null)
        param_direction = transform;
        _direction = param_direction.forward;
    }

    void Burst()
    { 
        float accMag = Mathf.Max( minAcc , param_targetSpeed - Vector3.Dot(_other.velocity, _direction));
        _other.AddForce(_direction * accMag, ForceMode.VelocityChange);
    }

    void Acclerate(Rigidbody other, Quaternion terrrianRot)
    {
        Vector3 terrianAcc = terrrianRot * _direction;
        float accMag = Mathf.Min(param_acc, Mathf.Max(minAcc, param_targetSpeed - Vector3.Dot(other.velocity, terrianAcc)));
        Vector3 acc = other.velocity.normalized * accMag;
        other.AddForce(accMag * terrianAcc, ForceMode.Acceleration);
    }

    void StartAccelerate(CC.Hub other)
    { 
        other.FixedEvent += Acclerate;
    }
    IEnumerator EndAccelerate(CC.Hub other)
    {
        yield return new WaitForSeconds(param_lingering);
        other.FixedEvent -= Acclerate;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name + "Entered");

        _other = other.GetComponent<Rigidbody>();
        switch(param_mode)
        {
        case BoostMode.Burst:
                Burst();
                break;

        case BoostMode.Continous:
                StartAccelerate(other.GetComponent<CC.Hub>());
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(param_mode == BoostMode.Continous)
            StartCoroutine(EndAccelerate(other.GetComponent<CC.Hub>()));
    }
}
