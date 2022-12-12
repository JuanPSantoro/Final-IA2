using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Audio", menuName = "Audio/Audio Clip", order = 58)]

public class AudioClipSO : ScriptableObject
{
    public AudioClip clip;
    [Range(0,1)]
    public float volume = 1;
}
