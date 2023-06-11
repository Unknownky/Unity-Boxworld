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
        image = GetComponent<Image>();//安到当前的图片上
                                      // 如果Button组件没有赋值，就从当前物体上获取
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


