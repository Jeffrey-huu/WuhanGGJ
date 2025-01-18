using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSystem : MonoBehaviour
{
    public static AudioSystem instance;

    [SerializeField] private AudioClip introSound;
    [SerializeField] private AudioClip bubbleIdleSound;
    [SerializeField] private AudioClip bubbleFastIdleSound;
    [SerializeField] private AudioClip bubbleNearBurstSound;
    [SerializeField] private AudioClip successSound;
    [SerializeField] private AudioClip failSound;

    private AudioSource audio_controller;

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
        audio_controller = GetComponent<AudioSource>();
        introSound.LoadAudioData();
        bubbleIdleSound.LoadAudioData();
        bubbleFastIdleSound.LoadAudioData();
        bubbleNearBurstSound.LoadAudioData();
        successSound.LoadAudioData();
        failSound.LoadAudioData();
        PlayIntroSound();
    }

    private void PlaySound(AudioClip sound)
    {
        audio_controller.loop = false;

        audio_controller.clip = sound;
        audio_controller.loop = true;

        audio_controller.Play();
    }

    public void PlayIntroSound()
    {
        if (audio_controller.clip == introSound)
        {
            return;
        }
        PlaySound(introSound);
    }

    public void PlayBubbleIdleSound()
    {
        if (audio_controller.clip == bubbleIdleSound)
        {
            return;
        }
        PlaySound(bubbleIdleSound);
    }

    public void PlayBubbleFastIdleSound()
    {
        if (audio_controller.clip == bubbleFastIdleSound)
        {
            return;
        }
        PlaySound(bubbleFastIdleSound);
    }

    public void PlayBubbleNearBurstSound()
    {
        if (audio_controller.clip == bubbleNearBurstSound)
        {
            return;
        }

        PlaySound(bubbleNearBurstSound);
    }

    public void PlayGameSuccessfulSound()
    {
        if (audio_controller.clip == successSound)
        {
            return;
        }
        PlaySound(successSound);
    }

    public void PlayGameFailedSound()
    {
        if (audio_controller.clip == failSound)
        {
            return;
        }
        PlaySound(failSound);
    }
}
