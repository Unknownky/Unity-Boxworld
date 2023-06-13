using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlatformSwitch : MonoBehaviour
{
    [Header("Image Switch")]
    [SerializeField] private Image nowImage;
    [SerializeField] private Sprite[] sprites;

    [Header("UI Switch")]
    [SerializeField] private GameObject[] UIgameobjects;

    public enum Platforms{
        PC = 0,
        Phone = 1,
        All = 2,
    }

    public static Platforms _platform { get; set; }

    private void Start()
    {
        SwitchPlatform();
        Debug.Log(_platform);
        SwitchMenuUI();
    }

    private void SwitchPlatform()
    {
        if (Screen.width >= Screen.height)
        {
            _platform = Platforms.PC;
        }
        else
        {
            _platform = Platforms.Phone;
        }
    }

    private void SwitchMenuUI()
    {
        for (int i = 0; i < (int)Platforms.All; i++)
        {
            UIgameobjects[i].SetActive(i==(int)_platform);
        }
    }

}
