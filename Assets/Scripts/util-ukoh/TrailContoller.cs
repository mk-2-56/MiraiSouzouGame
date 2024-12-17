using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;


public class TrailContoller : MonoBehaviour
{
    [SerializeField] float param_minSpeed;
    [SerializeField] float param_maxSpeed;
    [SerializeField] float param_maxTrailTime = 5.0f;
    
    VisualEffect  _wave;
    TrailRenderer _trail;
    Material _wind;

    MeshRenderer  _windRenderer;

    int _spawnRate = 256;

    public void HandleSpeedEffect(float speed)
    { 
        float d = speed - param_minSpeed;
        float intensity = AU.Math.SmoothStep(speed, param_minSpeed, param_maxSpeed);

        _trail.emitting = d > 0;
        _trail.time = intensity * param_maxTrailTime;
        Color white = Color.white;
        white.a = Mathf.Max(0.2f, intensity);
        _trail.startColor = _trail.endColor = white;

        _wave.SetFloat("Intensity", intensity);
        _wave.SetInt("Spawn", (int)(_spawnRate * Mathf.Clamp(d, 0, 1)));

        _windRenderer.enabled = d > 0;
        _wind.SetFloat("_Intensity", intensity);
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

        { 
            GameObject temp = transform.Find("WindEffect").gameObject;
            temp.SetActive(true);
            _windRenderer = temp.GetComponent<MeshRenderer>();
            _wind         = _windRenderer.material;
        }
    }

    // Update is called once per frame
}
