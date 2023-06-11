using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MusicController : MonoBehaviour
{
    public AudioSource audioSource;
    public Sprite playSprite;
    public Sprite pauseSprite;
    public Button button;

    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();//������ǰ��ͼƬ��
                                      // ���Button���û�и�ֵ���ʹӵ�ǰ�����ϻ�ȡ
        if (button == null)
        {
            button = GetComponent<Button>();
        }
    }

    public void ToggleMusic()
    {
        if (audioSource == null)
            audioSource = GameObject.FindGameObjectWithTag("MusicKeep").GetComponent<AudioSource>();

        if (audioSource.isPlaying)
        {
            audioSource.loop = false;
            audioSource.Pause();
            image.sprite = playSprite;
        }
        else
        {
            audioSource.loop = true;
            audioSource.Play();
            image.sprite = pauseSprite;
        }
    }
}


