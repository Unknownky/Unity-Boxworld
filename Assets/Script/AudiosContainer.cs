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
    [SerializeField]private GameObject confirmationPromote;//���������ڳ�����Ϸ�еı��������ʾ
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

    public void SetPlayMusic(int clipsIndex)//ͨ��Toggle��������
    {
        if (audioSource.isPlaying)
            audioSource.Stop();

        _clipsIndex = clipsIndex;

    }

    public void OneToggleOn(int clipsIndex)
    {
        for (int i = 0; i < _togglesCount; i++)
        {
            // ���� isOn ����
            if(i!=clipsIndex)
                _toggles[i].isOn = (i == clipsIndex);
        }
    }

    public void TryMusic()//�����ز��ź͹ر�����,ÿ��ֻ����һ������
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

    void LoadMusicIndex()//�������ƫ������ѡ��
    {
        if (PlayerPrefs.HasKey("MusicIndex"))
            _clipsIndex = PlayerPrefs.GetInt("MusicIndex");

        _toggles[_clipsIndex].isOn = true;
        audioSource.clip = audioClips[_clipsIndex];//����Ĭ�ϲ��ŵ�����

        OneToggleOn(_clipsIndex);

    }
    public void MusicIndexApply()//ʹ��PlayerPrefs�洢���ƫ��
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
