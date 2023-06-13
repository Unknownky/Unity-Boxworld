using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudiosContainer : MonoBehaviour
{
    [Header("Audios")]
    [SerializeField]private AudioClip[] audioClips;
    [SerializeField]private Toggle[] togglesPC;
    [SerializeField] private Toggle[] togglesPhone;
    [SerializeField] private AudioSource mainAudioSource;

    [Header("SaveConfirmBox")]
    [SerializeField]private GameObject confirmationPromote;//该物体用于常见游戏中的保存加载显示
    [SerializeField] private GameObject confirmationPromotePhone;

    private Toggle[] _toggles;
    private AudioSource audioSource;
    private int _togglesCount;
    private int _clipsIndex = 2;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        Debug.Log("AudioContainer"+PlatformSwitch._platform);
        if (PlatformSwitch._platform == PlatformSwitch.Platforms.PC)
            _toggles = togglesPC;
        else if (PlatformSwitch._platform == PlatformSwitch.Platforms.Phone)
            _toggles = togglesPhone;

        _togglesCount = _toggles.Length;
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
        for (int i = 0; i < _togglesCount; i++)
        {
            // 设置 isOn 属性
            if(i!=clipsIndex)
                _toggles[i].isOn = (i == clipsIndex);
        }
    }

    public void TryMusic()//单纯地播放和关闭音乐,每次只播放一个音乐
    {
        bool canPlay = false;
        for (int i = 0; i < _togglesCount; i++)
        {
            if (_toggles[i].isOn)
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

        _toggles[_clipsIndex].isOn = true;
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
        confirmationPromotePhone.SetActive(true);
        confirmationPromote.SetActive(true);
        yield return new WaitForSeconds(2);
        confirmationPromote.SetActive(false);
        confirmationPromotePhone.SetActive(false);
    }
}
