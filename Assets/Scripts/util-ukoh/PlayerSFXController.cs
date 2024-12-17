using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSFXController : MonoBehaviour
{
    //[SerializeField] float param_minSpeed;
    //[SerializeField] float param_maxSpeed;

    [SerializeField] AudioClip param_driftClip;
    [SerializeField] AudioClip param_dashClip;
    [SerializeField] List<AudioClip> param_collideClips = new List<AudioClip>();

    AudioSource _source;
    Transform   _rRoot;

    //Temp
    float _duration = 1.2f;
    float _volume   = 1.0f;

    public void OnDriftStart()
    { 
        _source.clip = param_driftClip;
        _source.loop = true;
        _source.playOnAwake = false;
        _source.Play();

        _source.volume = 0;
        StartCoroutine(AU.Fader.FadeIn(_source.volume, value => { _source.volume = value; }, _duration));
    }
    public void OnDriftEnd()
    {
        _source.volume = 1;
        StartCoroutine(AU.Fader.FadeOut(_source.volume, value => { _source.volume = value; }, _duration));
        StartCoroutine(StopDriftClip(_duration));
    }

    IEnumerator StopDriftClip(float duration)
    {
        yield return new WaitForSeconds(duration);
        _source.playOnAwake = false;
        _source.loop = false;
        _source.Stop();
        _source.volume = 1;
    }

    public void OnDash()
    {
        _source.PlayOneShot(param_dashClip);
    }

    public void OnSpeed()
    {

    }
    public void OnCollide(Vector3 dv)
    {
        if (dv.y > 0.5f)
        {
            System.Random rand = new();
            _source.PlayOneShot(param_collideClips[rand.Next(param_collideClips.Count)]);
        }
    }

    PlayerEffectDispatcher _effectDispatcher;
    // Start is called before the first frame update
    void Start()
    {
        _source = GetComponent<AudioSource>();
        _effectDispatcher = transform.parent.GetComponent<PlayerEffectDispatcher>();
        _rRoot = transform.root;

        _effectDispatcher.DriftStartE += OnDriftStart;
        _effectDispatcher.DriftEndE   += OnDriftEnd;
        _effectDispatcher.DashE       += OnDash;
        _effectDispatcher.CollideE    += OnCollide;
    }

    private void FixedUpdate()
    {
    }

}
