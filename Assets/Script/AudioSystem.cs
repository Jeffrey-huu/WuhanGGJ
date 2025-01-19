using UnityEngine;

public class AudioSystem : MonoBehaviour
{
    public static AudioSystem instance;

    [Header("BGM Clip")]
    [SerializeField] private AudioClip introSound;
    [SerializeField] private AudioClip bubbleIdleSound;
    [SerializeField] private AudioClip bubbleFastIdleSound;
    [SerializeField] private AudioClip bubbleNearBurstSound;
    [SerializeField] private AudioClip successSound;
    [SerializeField] private AudioClip failSound;

    [Header("FX Clip")]
    [SerializeField] private AudioClip nextTurn;
    [SerializeField] private AudioClip gameBegin;
    [SerializeField] private AudioClip longPress;
    [SerializeField] private AudioClip assetPutDown;
    [SerializeField] private AudioClip roundFinish;
    [SerializeField] private AudioClip bubbleBurst;

    // 添加两个音频源，一个用于BGM，一个用于音效
    private AudioSource bgmSource;
    private AudioSource sfxSource;

    // 音量控制
    [Range(0f, 1f)] public float musicVolume = 1f;  // 背景音乐音量
    [Range(0f, 1f)] public float sfxVolume = 1f;    // 音效音量

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            Debug.Log("AudioSystem already exists. Destroying the new instance.");
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("AudioSystem initialized successfully.");
        }
    }

    private void Start()
    {
        // 获取组件
        bgmSource = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();

        // 设置音量
        bgmSource.volume = musicVolume;
        sfxSource.volume = sfxVolume;

        LoadAllAudioClips();

        // 播放背景音乐
        PlayIntroSound();
    }

    // 对外调节音效音量的接口
    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        bgmSource.volume = musicVolume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        sfxSource.volume = sfxVolume;
    }

    // 获取当前音效音量和背景音乐音量
    public float GetMusicVolume()
    {
        return musicVolume;
    }

    public float GetSFXVolume()
    {
        return sfxVolume;
    }

    private void LoadAllAudioClips()
    {
        introSound.LoadAudioData();
        bubbleIdleSound.LoadAudioData();
        bubbleFastIdleSound.LoadAudioData();
        bubbleNearBurstSound.LoadAudioData();
        successSound.LoadAudioData();
        failSound.LoadAudioData();

        nextTurn.LoadAudioData();
        gameBegin.LoadAudioData();
        longPress.LoadAudioData();
        assetPutDown.LoadAudioData();
        roundFinish.LoadAudioData();
        bubbleBurst.LoadAudioData();
    }

    private void PlaySound(AudioSource source, bool loop, AudioClip sound)
    {
        source.loop = loop;
        source.clip = sound;
        source.Play();
    }

    private void StopPlaySound(AudioSource source)
    {
        source.Stop();
    }

    public void PlayNextTurnSound()
    {
        PlaySound(sfxSource, false, nextTurn);
    }

    public void PlayGameBeginSound()
    {
        PlaySound(sfxSource, false, gameBegin);
    }

    public void PlayAssetPutDownSound()
    {
        PlaySound(sfxSource, false, assetPutDown);
    }

    public void PlayBubbleBurstSound()
    {
        PlaySound(sfxSource, false, bubbleBurst);
    }

    public void PlayRoundFinishSound()
    {
        PlaySound(sfxSource, false, roundFinish);
    }

    public void PlayLongPressSound()
    {
        PlaySound(sfxSource, false, longPress);
    }
    public void StopPlayLongPressSound()
    {
        if (sfxSource.clip != longPress)
        {
            return;
        }
        StopPlaySound(sfxSource);
    }


    // 播放不同音效的方法
    public void PlayIntroSound()
    {
        if (bgmSource.clip == introSound) return;
        PlaySound(bgmSource, true, introSound);
    }

    public void PlayBubbleIdleSound()
    {
        if (bgmSource.clip == bubbleIdleSound) return;
        PlaySound(bgmSource, true, bubbleIdleSound);
    }

    public void PlayBubbleFastIdleSound()
    {
        if (bgmSource.clip == bubbleFastIdleSound) return;
        PlaySound(bgmSource, true, bubbleFastIdleSound);
    }

    public void PlayBubbleNearBurstSound()
    {
        if (bgmSource.clip == bubbleNearBurstSound) return;
        PlaySound(bgmSource, true, bubbleNearBurstSound);
    }

    public void PlayGameSuccessfulSound()
    {
        if (bgmSource.clip == successSound) return;
        PlaySound(bgmSource, true, successSound);
    }

    public void PlayGameFailedSound()
    {
        if (bgmSource.clip == failSound) return;
        PlaySound(bgmSource, true, failSound);
    }
}
