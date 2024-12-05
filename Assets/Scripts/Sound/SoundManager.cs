using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    public float masterVolume;
    public float bgmMasterVolume;
    public float seMasterVolume;

    //
    [SerializeField] private List<BGMSoundData> bgmSoundDatas;
    [SerializeField] private List<SESoundData> seSoundDatas;
    [SerializeField] private AudioSource curBgmAudioSource;
    [SerializeField] private AudioSource curSeAudioSource;

    private GameObject playerManager;
    //���ꏈ��
    private List<AudioSource> bgmAudioSources = new List<AudioSource>();
    private List<AudioSource> seAudioSources = new List<AudioSource>();
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

        Initialized();
    }

    private void Update()
    {
        playerManager = GameObject.Find("PlayerManager");
        if (playerManager == null) return;
        else
        {
/*            playerManager.GetComponent
*/        }
    }
    public void Initialized()
    {
        if (SoundManager.Instance == null) return;
        UnityEngine.Debug.Log("SoundManager Initialized");
        masterVolume = 1;
        bgmMasterVolume = 1;
        seMasterVolume = 1;

        // AudioSource�v�[���p�̐e�I�u�W�F�N�g���쐬
        GameObject bgmParent = new GameObject("BGM_AudioSources");
        GameObject seParent = new GameObject("SE_AudioSources");
        bgmParent.transform.parent = this.transform;
        seParent.transform.parent = this.transform;

        for (int i = 0; i < audioSourceMax; i++)
        {
            // BGM�pAudioSource�𐶐����ăv�[���ɒǉ�
            AudioSource tempBGM = bgmParent.AddComponent<AudioSource>();
            tempBGM.clip = null;
            tempBGM.loop = false;
            tempBGM.volume = 0;
            tempBGM.playOnAwake = false;
            tempBGM.mute = false;
            bgmAudioSources.Add(tempBGM);

            // SE�pAudioSource�𐶐����ăv�[���ɒǉ�
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

    public void PlayBGM(BGMSoundData.BGM bgmType, bool stopPrevious)
    {
        BGMSoundData bgmData = bgmSoundDatas.Find(data => data.bgm == bgmType);

        if (bgmData != null)
        {
            if (stopPrevious && curBgmAudioSource != null && curBgmAudioSource.isPlaying)
            {
                curBgmAudioSource.Stop(); // �O��BGM���~
            }

            foreach (AudioSource bgmAC in bgmAudioSources)
            {
                if (!bgmAC.isPlaying)
                {
                    bgmAC.clip = bgmData.audioClip;
                    bgmAC.volume = bgmData.volume * bgmMasterVolume * masterVolume;
                    bgmAC.loop = bgmData.loop;
                    bgmAC.Play();
                    curBgmAudioSource = bgmAC;
                    return;
                }
            }

            Debug.LogWarning("���ׂĂ�BGM AudioSource���g�p���ł��B�V����BGM���Đ��ł��܂���B");
        }
        else
        {
            Debug.LogWarning($"�w�肳�ꂽBGM ({bgmType}) ��������܂���B");
        }
    }

    public void PlayBGM(BGMSoundData.BGM bgmType)
    {
        // �Ή�����BGM�f�[�^���擾
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

    public AudioSource PlaySE(SESoundData.SE seType)
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
                    return seAC;
                }
            }

            Debug.LogWarning("���ׂĂ�SE AudioSource���g�p���ł��B�V����SE���Đ��ł��܂���B");
        }
        else
        {
            Debug.LogWarning($"�w�肳�ꂽSE ({seType}) ��������܂���B");
        }
        return null;
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

    //����̈ʒu�ŃT�E���h���Đ�
    public void PlaySoundAtPosition(AudioClip clip, Vector3 position, float volume = -1f)
    {
        if (clip == null)
        {
            Debug.LogWarning("�Đ�����AudioClip���w�肳��Ă��܂���I");
            return;
        }

        float finalVolume = (volume < 0) ? masterVolume : volume;

        //�w�肵���ʒu�ŃT�E���h���Đ�
        AudioSource.PlayClipAtPoint(clip, position, finalVolume);
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

    public void FadeOutAllBGM(float fadeDuration = 1f)
    {
        foreach (AudioSource bgmAC in bgmAudioSources)
        {
            if (bgmAC != null && bgmAC.isPlaying)
            {
                StartCoroutine(FadeOutBGMCoroutine(bgmAC, fadeDuration));
            }
        }
    }

    private IEnumerator FadeOutBGMCoroutine(AudioSource bgmAudioSource, float duration)
    {
        float startVolume = bgmAudioSource.volume; // �t�F�[�h�J�n���̉���
        float timeElapsed = 0f;

        // �t�F�[�h�A�E�g����
        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            bgmAudioSource.volume = Mathf.Lerp(startVolume, 0, timeElapsed / duration);
            yield return null;
        }

        // �t�F�[�h�A�E�g�������������~
        bgmAudioSource.Stop();
        bgmAudioSource.volume = startVolume; // �{�����[�������ɖ߂��Ď���Đ����Ɏg�p
    }

    // �t�F�[�h�A�E�g����
    public void FadeOutAllSounds(float fadeDuration, System.Action onComplete = null)
    {
        StartCoroutine(FadeOutCoroutine(fadeDuration, onComplete));
    }

    private IEnumerator FadeOutCoroutine(float fadeDuration, System.Action onComplete)
    {
        float startTime = Time.time;

        // ���݂̉��ʂ��擾
        float bgmStartVolume = bgmMasterVolume * masterVolume;
        float seStartVolume = seMasterVolume * masterVolume;

        while (Time.time < startTime + fadeDuration)
        {
            float elapsed = Time.time - startTime;
            float progress = elapsed / fadeDuration;

            // ���ʂ����X�Ɍ���
            float newVolume = Mathf.Lerp(bgmStartVolume, 0, progress);
            SetBGMVolume(newVolume / masterVolume); // bgmMasterVolume���X�V
            SetSEVolume(newVolume / masterVolume);  // seMasterVolume���X�V

            yield return null; // ���̃t���[���܂őҋ@
        }

        // �ŏI�I�ɉ��ʂ��[���ɐݒ�
        SetBGMVolume(0);
        SetSEVolume(0);

        // �Đ����~
        StopAllSounds();

        // �t�F�[�h�A�E�g�������̃R�[���o�b�N
        onComplete?.Invoke();
    }

    public void StopAllSounds()
    {
        // �S�Ă�BGM AudioSource���~
        foreach (AudioSource bgmAC in bgmAudioSources)
        {
            bgmAC.Stop();
        }

        // �S�Ă�SE AudioSource���~
        foreach (AudioSource seAC in seAudioSources)
        {
            seAC.Stop();
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
        BGM_Wind,
        BGM_Maguma1,
        BGM_Maguma2,
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
        SE_BoostTile,
        SE_BoostRing,
        SE_WindTrigger,
        SE_WindOnBigJump,
        SE_VolcanicEruption,
        SE_MAX, // ���ꂪ���x���ɂȂ�
    }

    public SE se;
    public AudioClip audioClip;
    [Range(0, 1)]
    public float volume = 1;
}