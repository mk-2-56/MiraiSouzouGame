using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource curBgmAudioSource;
    [SerializeField] AudioSource curSeAudioSource;

    [SerializeField] List<BGMSoundData> bgmSoundDatas;
    [SerializeField] List<SESoundData> seSoundDatas;

    public static SoundManager Instance { get; private set; }
    public float masterVolume;
    public float bgmMasterVolume;
    public float seMasterVolume;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        Initialized();
    }

    public void Initialized()
    {
        masterVolume = 1;
        bgmMasterVolume = 1;
        seMasterVolume = 1;
    }

    private void Update()
    {
        for(int i = 0; i < bgmSoundDatas.Count; i++)
        {
            curBgmAudioSource.volume = bgmSoundDatas[i].volume * seMasterVolume * masterVolume;
        }

        for(int i = 0;i < seSoundDatas.Count; i++)
        {

        }
    }

    public void PlayBGM(AudioSource audioSource)
    {
        curBgmAudioSource = audioSource;
        curBgmAudioSource.volume *= bgmMasterVolume * masterVolume;
        curBgmAudioSource.Play();
    }

    public void PlayBGM(AudioSource bgmSource, BGMSoundData.BGM bgmNum)
    {
        BGMSoundData data = bgmSoundDatas.Find(data => data.bgm == bgmNum);

        if(data == null)
        {
            data = new BGMSoundData();
            data.bgm = bgmNum;
            data.audioClip = bgmSource.clip;
            data.volume = bgmSource.volume;
            bgmSoundDatas.Add(data);
        }

        curBgmAudioSource = bgmSource;
        curBgmAudioSource.volume = data.volume * bgmMasterVolume * masterVolume;
        curBgmAudioSource.Play();
    }

    public void PlaySE(AudioSource seSource)
    {
        curSeAudioSource = seSource;
        curSeAudioSource.volume *= seMasterVolume * masterVolume;
        curSeAudioSource.PlayOneShot(seSource.clip);
    }

    public void PlaySE(AudioSource seSource,SESoundData.SE seNum)
    {
        SESoundData data = seSoundDatas.Find(data => data.se == seNum);

        if (data == null)
        {
            data = new SESoundData();
            data.se = seNum;
            data.audioClip = seSource.clip;
            data.volume = seSource.volume;
            seSoundDatas.Add(data);
        }
        
        curSeAudioSource = seSource;
        curSeAudioSource.volume = data.volume * seMasterVolume * masterVolume;
        curSeAudioSource.PlayOneShot(data.audioClip);
    }

}

[System.Serializable]
public class BGMSoundData
{
    public enum BGM
    {
        BGM_Title,
        BGM_Tutorial,
        BGM_Game, // ‚±‚ê‚ªƒ‰ƒxƒ‹‚É‚È‚é
        BGM_Result,
        BGM_Max,
    }

    public BGM bgm;
    public AudioClip audioClip;
    [Range(0, 1)]
    public float volume = 1;
}

[System.Serializable]
public class SESoundData
{
    public enum SE
    {
        SE_Button,
        SE_COIN,
        SE_Metour,
        SE_Pubble,
        SE_JumpBoardB,
        SE_JumpBoardS,
        SE_MAX, // ‚±‚ê‚ªƒ‰ƒxƒ‹‚É‚È‚é
    }

    public SE se;
    public AudioClip audioClip;
    [Range(0, 1)]
    public float volume = 1;
}