using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public AudioSource audioSource;
    public AudioSource SFXSource;
    public List<AudioClip> clipList;

    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AudioPlay(string audioName)
    {
        audioSource.clip = clipList.Find(x => x.name == audioName);
        audioSource.Play();
    }

    public void AudioStop()
    {
        audioSource.Stop();
    }

    public void SFXPlay(string SFXName)
    {
        SFXSource.PlayOneShot(clipList.Find(x => x.name == SFXName));
    }

    public void ButtonClicked()
    {
        SoundManager.Instance.SFXPlay("Button Click");
    }

    public void ChangeVolume(float value)
    {
        audioSource.volume = value;
        SFXSource.volume = value;
    }
}