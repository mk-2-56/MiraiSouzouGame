using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSoundEffect : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] AudioSource windAudioSource;
    [SerializeField] float mutiplier = 1.0f;
    private float curVolume = 0.0f;
    void Start()
    {
        transform.parent.GetComponent<PlayerEffectDispatcher>().SpeedE += UpdateWindSound;
        windAudioSource.volume = curVolume;
        SoundManager.Instance.PlayBGM(windAudioSource);

    }

    public void UpdateWindSound(float playerSpeed)
    {
        curVolume = Mathf.Clamp01(playerSpeed * mutiplier);
        windAudioSource.volume = curVolume;
    }

}
