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
    [SerializeField]private GameObject confirmationPromote;//���������ڳ�����Ϸ�еı��������ʾ

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

    public void SetPlayMusic(int clipsIndex)//ͨ��Toggle��������
    {
        if (audioSource.isPlaying)
            audioSource.Stop();

        _clipsIndex = clipsIndex;

    }

    public void OneToggleOn(int clipsIndex)
    {
        for (int i = 0; i < togglesCount; i++)
        {
            // ���� isOn ����
            if(i!=clipsIndex)
                toggles[i].isOn = (i == clipsIndex);
        }
    }

    public void TryMusic()//�����ز��ź͹ر�����,ÿ��ֻ����һ������
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

    void LoadMusicIndex()//�������ƫ������ѡ��
    {
        if (PlayerPrefs.HasKey("MusicIndex"))
            _clipsIndex = PlayerPrefs.GetInt("MusicIndex");

        toggles[_clipsIndex].isOn = true;
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
        confirmationPromote.SetActive(true);
        yield return new WaitForSeconds(2);
        confirmationPromote.SetActive(false);
    }
}
