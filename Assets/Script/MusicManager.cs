using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;//用MusicManager实例一个instance来避免音乐暂停播放
    public AudioSource audioSource;

    private void Awake()
    {
        if (instance == null)//
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
