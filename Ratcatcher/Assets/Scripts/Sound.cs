using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    // the audio clip
    public AudioClip clip;
    // source of audio, can be hidden since we only use the clip
    [HideInInspector]
    public AudioSource source;

    // volume with range
    [Range(0f,1f)]
    public float volume;

    // name of the sound
    public string name;

    public bool loop;
}