using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager: MonoBehaviour
{
    #region Singleton Init

    private void Awake()
    {
        InitAudioSource();
    }

    public void InitAudioSource()
    {
        if (bgmSource == null)
        {
            bgmSource = gameObject.AddComponent<AudioSource>();
            bgmSource.loop = true;
        }

        if (sfxSource == null)
            sfxSource = gameObject.AddComponent<AudioSource>();
    }
    #endregion

    #region Privates
    AudioSource bgmSource;
    AudioSource sfxSource;

    bool isBGMPlaying = false;

    #endregion

    #region Dependencies
    [Header("Sound Database")]
    [SerializeField] SoundDB soundDB;
    #endregion

    #region Sound Play Util


    public void PlayBGM(string bgmName)
    {
        SoundData bgm = FindSound(soundDB.bgmList, bgmName);
        isBGMPlaying = true;
    }


    public void StopBGM()
    {
        if (!isBGMPlaying)
            return;
        bgmSource.Stop();

        isBGMPlaying = false;
    }


    public void PlaySFX(string sfxName)
    {
        SoundData sfx = FindSound(soundDB.sfxList, sfxName);
        if (sfx == null)
        {
            Debug.Log($"Failed to find sound data_SFX : {sfxName}");
        }
        else
        {
            sfxSource.pitch = sfx.pitch;
            sfxSource.volume = sfx.volume;
            sfxSource.PlayOneShot(sfx.audioClip);

        }
    }

    private SoundData FindSound(SoundData[] soundList, string soundName)
    {
        foreach (SoundData sound in soundList)
        {
            if (sound.soundName == soundName)
            {
                return sound;
            }
        }
        return null;
    }


}
#endregion