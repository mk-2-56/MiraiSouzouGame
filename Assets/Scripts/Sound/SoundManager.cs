using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource curBgmAudioSource;
    [SerializeField] private AudioSource curSeAudioSource;

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
        for (int i = 0; i < bgmSoundDatas.Count; i++)
        {
            curBgmAudioSource.volume = bgmSoundDatas[i].volume * seMasterVolume * masterVolume;
        }

        for (int i = 0; i < seSoundDatas.Count; i++)
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

        if (data == null)
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

    public void PlayBGM(BGMSoundData.BGM bgmType)
    {
        // �Ή�����BGM�f�[�^���擾
        BGMSoundData bgmData = bgmSoundDatas.Find(data => data.bgm == bgmType);

        if (bgmData != null)
        {
            // AudioClip�Ɖ��ʂ�ݒ肵�čĐ�
            curBgmAudioSource.clip = bgmData.audioClip;
            curBgmAudioSource.volume = bgmData.volume * bgmMasterVolume * masterVolume;
            curBgmAudioSource.loop = bgmData.loop;
            curBgmAudioSource.Play();
        }
        else
        {
            Debug.LogWarning($"�w�肳�ꂽBGM ({bgmType}) ��������܂���B");
        }
    }

    public void PlaySE(AudioSource seSource)
    {
        curSeAudioSource = seSource;
        curSeAudioSource.volume *= seMasterVolume * masterVolume;
        curSeAudioSource.PlayOneShot(seSource.clip);
    }

    public void PlaySE(AudioSource seSource, SESoundData.SE seNum)
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

    public void PlaySE(SESoundData.SE seType)
    {
        // �Ή�����BGM�f�[�^���擾
        SESoundData seData = seSoundDatas.Find(data => data.se == seType);

        if (seData != null)
        {
            // AudioClip�Ɖ��ʂ�ݒ肵�čĐ�
            curSeAudioSource.clip = seData.audioClip;
            curSeAudioSource.volume = seData.volume * seMasterVolume * masterVolume;
            curSeAudioSource.Play();
        }
        else
        {
            Debug.LogWarning($"�w�肳�ꂽSE ({seType}) ��������܂���B");
        }
    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        UpdateVolumes();
    }
    public void SetBGMVolume(float volume)
    {
        bgmMasterVolume = Mathf.Clamp01(volume);
        UpdateVolumes();
    }

    public void SetSEVolume(float volume)
    {
        seMasterVolume = Mathf.Clamp01(volume);
        UpdateVolumes();
    }

    private void UpdateVolumes()
    {
        // ���ݍĐ�����BGM�̉��ʂ��X�V
        if (curBgmAudioSource != null && curBgmAudioSource.isPlaying)
        {
            BGMSoundData currentBGM = bgmSoundDatas.Find(data => data.audioClip == curBgmAudioSource.clip);
            if (currentBGM != null)
            {
                curBgmAudioSource.volume = currentBGM.volume * bgmMasterVolume * masterVolume;
            }
        }

        // ���ʉ��pAudioSource�̉��ʂ��X�V�i�K�v�Ȃ畡���Ή��j
        if (curSeAudioSource != null)
        {
            curSeAudioSource.volume = seMasterVolume * masterVolume;
        }
    }
}

[System.Serializable]
public class BGMSoundData
{
    public enum BGM
    {
        BGM_Title,
        BGM_Tutorial,
        BGM_Game, // ���ꂪ���x���ɂȂ�
        BGM_Result,
        BGM_Max,
    }

    public BGM bgm;
    public AudioClip audioClip;
    [Range(0, 1)]
    public float volume = 1;
    public bool loop = false;
}

[System.Serializable]
public class SESoundData
{
    public enum SE
    {
        SE_Click,
        SE_Select,
        SE_Cancel,
        SE_Button,
        SE_SceneSwith,
        SE_SceneLoading,
        SE_COIN,
        SE_Metour,
        SE_Pubble,
        SE_JumpBoardB,
        SE_JumpBoardS,
        SE_MAX, // ���ꂪ���x���ɂȂ�
    }

    public SE se;
    public AudioClip audioClip;
    [Range(0, 1)]
    public float volume = 1;
}