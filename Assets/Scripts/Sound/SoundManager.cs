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
        // 対応するBGMデータを取得
        BGMSoundData bgmData = bgmSoundDatas.Find(data => data.bgm == bgmType);

        if (bgmData != null)
        {
            // AudioClipと音量を設定して再生
            curBgmAudioSource.clip = bgmData.audioClip;
            curBgmAudioSource.volume = bgmData.volume * bgmMasterVolume * masterVolume;
            curBgmAudioSource.loop = bgmData.loop;
            curBgmAudioSource.Play();
        }
        else
        {
            Debug.LogWarning($"指定されたBGM ({bgmType}) が見つかりません。");
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
        // 対応するBGMデータを取得
        SESoundData seData = seSoundDatas.Find(data => data.se == seType);

        if (seData != null)
        {
            // AudioClipと音量を設定して再生
            curSeAudioSource.clip = seData.audioClip;
            curSeAudioSource.volume = seData.volume * seMasterVolume * masterVolume;
            curSeAudioSource.Play();
        }
        else
        {
            Debug.LogWarning($"指定されたSE ({seType}) が見つかりません。");
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
        // 現在再生中のBGMの音量を更新
        if (curBgmAudioSource != null && curBgmAudioSource.isPlaying)
        {
            BGMSoundData currentBGM = bgmSoundDatas.Find(data => data.audioClip == curBgmAudioSource.clip);
            if (currentBGM != null)
            {
                curBgmAudioSource.volume = currentBGM.volume * bgmMasterVolume * masterVolume;
            }
        }

        // 効果音用AudioSourceの音量も更新（必要なら複数対応）
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
        BGM_Game, // これがラベルになる
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
        SE_MAX, // これがラベルになる
    }

    public SE se;
    public AudioClip audioClip;
    [Range(0, 1)]
    public float volume = 1;
}