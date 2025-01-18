using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSystem : MonoBehaviour
{
    public static AudioSystem instance;
    [SerializeField] private AudioClip uiIntro;
    [SerializeField] private AudioClip BgSound;

    private AudioSource audio_controller;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        audio_controller = GetComponent<AudioSource>();
        PlaySound(BgSound);
    }

    private void PlaySound(AudioClip sound)
    {
        audio_controller.loop = false;

        audio_controller.clip = sound;
        audio_controller.loop = true;

        audio_controller.Play();
    }
}
