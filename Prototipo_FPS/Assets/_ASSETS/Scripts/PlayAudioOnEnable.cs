using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayAudioOnEnable : MonoBehaviour
{
    private AudioSource m_cmpAudioSource = null;

    [SerializeField]
    private int m_waitTime = 0;
    [SerializeField]
    private List<AudioClip> m_audioList = null;

    private void Awake()
    {
        m_cmpAudioSource = GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        if(m_waitTime != 0)
        {
            StartCoroutine(CoroutinePlayAudio());
        }
        else
        {
            PlayAudio();
        }
    }


    private void PlayAudio()
    {
        if (m_audioList.Count == 0)
        {
            Debug.LogError(transform.name + " AUDIO LIST is empty");
            return;
        }

        int iRnd = Random.Range(0, m_audioList.Count);

        if (m_audioList[iRnd] == null)
        {
            Debug.LogError(transform.name + " AUDIO LIST index " + iRnd + " is NULL");
            return;
        }

        m_cmpAudioSource.clip = m_audioList[iRnd];
        m_cmpAudioSource.Play();
    }


    public IEnumerator CoroutinePlayAudio()
    {  
        yield return new WaitForSeconds(m_waitTime);

        StopCoroutine(CoroutinePlayAudio());
        PlayAudio();
    }
}
