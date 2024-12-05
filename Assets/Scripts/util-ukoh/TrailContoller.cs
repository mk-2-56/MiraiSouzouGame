using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;


public class TrailContoller : MonoBehaviour
{
    [SerializeField] float param_minSpeed;
    [SerializeField] float param_maxSpeed;
    [SerializeField] float param_maxTime = 5.0f;

    VisualEffect  _wave;
    TrailRenderer _trail;

    int _spawnRate = 256;

    public void HandleSpeedEffect(float speed)
    { 
        float range = param_maxSpeed - param_minSpeed;
        float d = (speed - param_minSpeed);
        float k = Mathf.Clamp(d / range, 0, 1);

        float intensity = 3 * k * k - 2 * k * k * k;

        _trail.emitting = d > 0;
        _trail.time = intensity * param_maxTime;
        Color white = Color.white;
        white.a = Mathf.Max(0.2f, intensity);
        _trail.startColor = _trail.endColor = white;

        _wave.SetFloat("Intensity", intensity);
        _wave.SetInt("Spawn", _spawnRate * (int)Mathf.Clamp(d, 0, 1));
    }

    public void HandleLift()
    { 
        _spawnRate = 0;
    }
    public void HandleLand()
    {
        _spawnRate = 256;
    }

    // Start is called before the first frame update
    void Start()
    {
        if(param_minSpeed == 0)
        {
            param_minSpeed = transform.parent.parent.parent.parent.GetComponent<CC.Basic>().maxSpeed;
        }

        { 
            var temp = transform.parent.GetComponent<PlayerEffectDispatcher>();
            temp.SpeedE += HandleSpeedEffect;
            temp.LifetedE += HandleLift;
            temp.LandedE  += HandleLand;
        }
        _trail = GetComponent<TrailRenderer>();
        _wave  = GetComponent<VisualEffect>();
    }

    // Update is called once per frame
}
