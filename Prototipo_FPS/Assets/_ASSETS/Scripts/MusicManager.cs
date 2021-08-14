using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : PersistentSingleton<MusicManager>
{
    private Dictionary<string, AudioClip> m_soundFXDictionary = null;
    private Dictionary<string, AudioClip> m_soundMusicDictionary = null;
    private AudioSource m_backgroundMusic = null;
    private AudioSource m_sfxMusic = null;

    private float m_musicVolume;

    public float MusicVolume
    {
        get
        {
            return m_musicVolume;
        }

        set
        {
            value = Mathf.Clamp(value, 0, 1);
            m_backgroundMusic.volume = m_musicVolume;
            m_musicVolume = value;
        }
    }

    public float MusicVolumeSave
    {
        get
        {
            return m_musicVolume;
        }

        set
        {
            value = Mathf.Clamp(value, 0, 1);
            m_backgroundMusic.volume = m_musicVolume;
            PlayerPrefs.SetFloat(AppPlayerPrefKeys.MUSIC_VOLUME, value);
            m_musicVolume = value;
        }
    }

    private float m_sfxVolume;

    public override void Awake()
    {
        base.Awake();

        m_backgroundMusic = CreateAudioSource("Music", true);
        m_sfxMusic = CreateAudioSource("Sfx", false);
    }


    private AudioSource CreateAudioSource(string name, bool isLoop)
    {
        GameObject temporaryAudioHost = new GameObject(name);
        AudioSource audioSource = temporaryAudioHost.AddComponent<AudioSource>() as AudioSource;
        audioSource.playOnAwake = false;
        audioSource.loop = isLoop;
        audioSource.spatialBlend = 0.0f;
        temporaryAudioHost.transform.SetParent(this.transform);
        return audioSource;
    }
}
