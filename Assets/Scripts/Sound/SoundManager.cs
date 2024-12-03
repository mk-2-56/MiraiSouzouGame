using System
    .Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundManager : MonoBehaviour
{
    [SerializeField] List<BGMSoundData> bgmSoundDatas;
    [SerializeField] List<SESoundData> seSoundDatas;

    [SerializeField] private AudioSource curBgmAudioSource;
    [SerializeField] private AudioSource curSeAudioSource;
    private List<AudioSource> bgmAudioSources = new List<AudioSource>();
    private List<AudioSource> seAudioSources = new List<AudioSource>();

    

    public static SoundManager Instance { get; private set; }
    public float masterVolume;
    public float bgmMasterVolume;
    public float seMasterVolume;
    private int audioSourceMax = 8;
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
    }

    public void Initialized()
    {
        if (SoundManager.Instance == null) return;
        masterVolume = 1;
        bgmMasterVolume = 1;
        seMasterVolume = 1;

        // AudioSourceプール用の親オブジェクトを作成
        GameObject bgmParent = new GameObject("BGM_AudioSources");
        GameObject seParent = new GameObject("SE_AudioSources");
        bgmParent.transform.parent = this.transform;
        seParent.transform.parent = this.transform;

        for (int i = 0; i < audioSourceMax; i++)
        {
            // BGM用AudioSourceを生成してプールに追加
            AudioSource tempBGM = bgmParent.AddComponent<AudioSource>();
            tempBGM.clip = null;
            tempBGM.loop = false;
            tempBGM.volume = 0;
            tempBGM.playOnAwake = false;
            tempBGM.mute = false;
            bgmAudioSources.Add(tempBGM);

            // SE用AudioSourceを生成してプールに追加
            AudioSource tempSE = seParent.AddComponent<AudioSource>();
            tempSE.clip = null;
            tempSE.loop = false;
            tempSE.volume = 0;
            tempSE.playOnAwake = false; 
            tempSE.mute = false;
            seAudioSources.Add(tempSE);
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
            foreach (AudioSource bgmAC in bgmAudioSources)
            {
                if (!bgmAC.isPlaying || bgmAC.clip == null)
                {
                    bgmAC.clip = bgmData.audioClip;
                    bgmAC.volume = bgmData.volume * bgmMasterVolume * masterVolume;
                    bgmAC.loop = bgmData.loop;
                    bgmAC.mute = false;
                    bgmAC.Play();
                    return;
                }
            }
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
        SESoundData seData = seSoundDatas.Find(data => data.se == seType);

        if (seData != null)
        {
            foreach (AudioSource seAC in seAudioSources)
            {
                if (!seAC.isPlaying || seAC.clip == null)
                {
                    seAC.clip = seData.audioClip;
                    seAC.volume = seData.volume * seMasterVolume * masterVolume;
                    seAC.mute = false;
                    seAC.Play();
                    return;
                }
            }

            Debug.LogWarning("すべてのSE AudioSourceが使用中です。新しいSEを再生できません。");
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
        Debug.Log($"SetBGMVolume called with value: {volume}");
        UpdateVolumes();
    }

    public void SetSEVolume(float volume)
    {
        seMasterVolume = Mathf.Clamp01(volume);
        UpdateVolumes();
    }

    private void UpdateVolumes()
    {
        foreach (AudioSource bgmAC in bgmAudioSources)
        {
            if (bgmAC.clip != null)
            {
                BGMSoundData bgmData = bgmSoundDatas.Find(data => data.audioClip == bgmAC.clip);
                if (bgmData != null)
                {
                    bgmAC.volume = bgmData.volume * bgmMasterVolume * masterVolume;
                }
            }
        }

        foreach (AudioSource seAC in seAudioSources)
        {
            if (seAC.clip != null)
            {
                SESoundData seData = seSoundDatas.Find(data => data.audioClip == seAC.clip);
                if (seData != null)
                {
                    seAC.volume = seData.volume * seMasterVolume * masterVolume;
                }
            }
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