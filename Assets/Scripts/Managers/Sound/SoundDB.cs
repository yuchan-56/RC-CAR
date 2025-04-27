using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "SoundDB", menuName = "Scriptable Objects/SoundDB")]
public class SoundDB : ScriptableObject
{
    public SoundData[] bgmList;
    public SoundData[] sfxList;
}
