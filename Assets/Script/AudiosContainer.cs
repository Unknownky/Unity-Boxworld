using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudiosContainer : MonoBehaviour
{
    [Header("Audios")]
    [SerializeField]private AudioClip[] audioClips;
    [SerializeField]private Toggle[] toggles;
    [SerializeField] private AudioSource mainAudioSource;

    [Header("SaveConfirmBox")]
    [SerializeField]private GameObject confirmationPromote;//该物体用于常见游戏中的保存加载显示

    private AudioSource audioSource;
    private int togglesCount;
    private int _clipsIndex = 2;
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        togglesCount = toggles.Length;
        LoadMusicIndex();
    }

    public void MusicHallPopout()
    {
        if (mainAudioSource.isPlaying)
            mainAudioSource.Stop();
    }

    public void MusicHallClose()
    {
        if (!mainAudioSource.isPlaying)
            mainAudioSource.Play();
        if(audioSource.isPlaying)
            audioSource.Stop();
    }

    public void SetPlayMusic(int clipsIndex)//通过Toggle设置音乐
    {
        if (audioSource.isPlaying)
            audioSource.Stop();

        _clipsIndex = clipsIndex;

    }

    public void OneToggleOn(int clipsIndex)
    {
        for (int i = 0; i < togglesCount; i++)
        {
            // 设置 isOn 属性
            if(i!=clipsIndex)
                toggles[i].isOn = (i == clipsIndex);
        }
    }

    public void TryMusic()//单纯地播放和关闭音乐,每次只播放一个音乐
    {
        bool canPlay = false;
        for (int i = 0; i < togglesCount; i++)
        {
            if (toggles[i].isOn)
                canPlay = true;
        }
        if (audioSource.isPlaying)
            audioSource.Stop();
        else if(canPlay)
            audioSource.PlayOneShot(audioClips[_clipsIndex]);
        
    }

    void LoadMusicIndex()//加载玩家偏好音乐选项
    {
        if (PlayerPrefs.HasKey("MusicIndex"))
            _clipsIndex = PlayerPrefs.GetInt("MusicIndex");

        toggles[_clipsIndex].isOn = true;
        audioSource.clip = audioClips[_clipsIndex];//设置默认播放的音乐

        OneToggleOn(_clipsIndex);

    }
    public void MusicIndexApply()//使用PlayerPrefs存储玩家偏好
    {
        PlayerPrefs.SetInt("MusicIndex", _clipsIndex);
        StartCoroutine(ConfirmationBox());
    }

    IEnumerator ConfirmationBox()
    {
        confirmationPromote.SetActive(true);
        yield return new WaitForSeconds(2);
        confirmationPromote.SetActive(false);
    }
}
