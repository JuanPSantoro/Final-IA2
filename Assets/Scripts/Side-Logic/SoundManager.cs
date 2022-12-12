using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource _source;

    [SerializeField]
    private AudioSource _bgmSource;

    [SerializeField]
    private AudioClipSO _bgmClip;

    public static SoundManager instance;
    void Start()
    {
        if (instance != null)
            Destroy(gameObject);

        instance = this;
        _source.loop = true;
        _bgmSource.loop = true;
        _bgmSource.clip = _bgmClip.clip;
        _bgmSource.volume = _bgmClip.volume;
        _bgmSource.Play();

    }

    public void Play(AudioClipSO audioClip)
    {
        _source.volume = audioClip.volume;
        _source.clip = audioClip.clip;
        _source.Play();
    }

    public void PlayOnPosition(AudioClipSO audioClip, Vector3 position)
    {
        _source.transform.position = position;
        Play(audioClip);
    }

    public void Stop()
    {
        _source.Stop();
    }

}
