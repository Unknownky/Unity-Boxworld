using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;//��MusicManagerʵ��һ��instance������������ͣ����

    [Header("Audios")]
    [SerializeField]private AudioSource audioSource;
    [SerializeField] private AudioClip[] audioClips;

    private int _audioClipIndex = 2;
    private void Awake()
    {
        if (instance == null)//
        {
            instance = this;
            if (PlayerPrefs.HasKey("MusicIndex"))
            {
                _audioClipIndex = PlayerPrefs.GetInt("MusicIndex");
            }
            audioSource.clip = audioClips[_audioClipIndex];
            audioSource.Play();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
