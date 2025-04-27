using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "SoundData", menuName = "Scriptable Objects/SoundData")]
public class SoundData : ScriptableObject
{
    public string soundName
    {
        get => this.name;
    }


    public AudioClip audioClip;


    [Range(0f, 2f)] public float volume = 1f;


    [Range(0f, 2f)] public float pitch = 1f;

}
