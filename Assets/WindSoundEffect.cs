using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSoundEffect : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float param_minSpeed;
    [SerializeField] float param_maxSpeed;

    [SerializeField] AudioSource windAudioSource;
    [SerializeField] float mutiplier = 1.0f;
    private float curVolume = 0.0f;
    void Start()
    {
        transform.parent.GetComponent<PlayerEffectDispatcher>().SpeedE += UpdateWindSound;
        windAudioSource.volume = curVolume;
        SoundManager.Instance?.PlayBGM(windAudioSource);

    }

    public void UpdateWindSound(float playerSpeed)
    {

        float range = param_maxSpeed - param_minSpeed;
        float d = (playerSpeed - param_minSpeed);
        float k = Mathf.Clamp(d / range, 0, 1);

        float intensity = 3 * k * k - 2 * k * k * k;

        curVolume = intensity;
        windAudioSource.volume = curVolume;
    }

}
