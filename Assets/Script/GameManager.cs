using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("GameItems Settings")]
    public Grid[] grids;
    public bool Wbox = false;
    public bool Bbox = false;

    private int CurrentGridIndex;

    [Header("Gradient")]
    [SerializeField]private GameObject GradientPC;
    [SerializeField]private GameObject GradientPhone;

    [Header("PlatformSwitch")]
    [SerializeField] private GameObject[] uIGameobjects;
    [SerializeField] private GameObject[] rotationSwitch;
    [SerializeField] private float[] platformCorrespondZRotations;

    [SerializeField] private GameObject[] scaleSwitch;
    [SerializeField] private float[] platformCorrespondScale;

    private GameObject Gradient;

    private GameObject MusicKeep;
    private GameObject PlayerObject;
    private PlayerController PlayerController;
    private GameObject ButtonContainer = null;

    private PlatformSwitch.Platforms nowPlatform;
    //在该脚本中进行游戏结束的判定，游戏场景的切换，游戏地图的切换，游戏音乐的管理
    private void Awake()
    {
        nowPlatform = PlatformSwitch._platform;
        //调整UI以及需要转角度的物体，以及需要改变大小的物体
        UISwitch();

        //RotationSwitch();

        ScaleSwitch();
        //调整Gradient
        Gradient = PlatformSwitch._platform==PlatformSwitch.Platforms.PC? GradientPC:GradientPhone;
        PlayerObject = GameObject.Find("Player");
        ButtonContainer = GameObject.Find("Buttons");
    }

    void Start()
    {
        Gradient.SetActive(true);
        CurrentGridIndex = 0;
        grids[CurrentGridIndex].enabled = true;
        MoveMap(CurrentGridIndex, true);
        grids[CurrentGridIndex+1].enabled = false;
        MoveMap(CurrentGridIndex+1, false);

        //CloseTilemap();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))//重开当前关卡
            ResetScene();
        if(Input.GetKeyDown(KeyCode.C))
            SwitchMap();
    }


    private void UISwitch()
    {
        for (int i = 0; i < (int)PlatformSwitch.Platforms.All; i++)
        {
            uIGameobjects[i].SetActive(i == (int)nowPlatform);
        }
    }

    private void RotationSwitch()
    {
        for (int i = 0; i < rotationSwitch.Length; i++)
        {
            rotationSwitch[i].transform.rotation = Quaternion.Euler(0, 0, platformCorrespondZRotations[(int)nowPlatform]);
        }
    }

    private void ScaleSwitch()
    {
        float switchScale = platformCorrespondScale[(int)nowPlatform];
        for (int i = 0; i < scaleSwitch.Length; i++)
        {
            scaleSwitch[i].transform.localScale = new Vector3(switchScale,switchScale,switchScale);
        }
    }


    public void SwitchMap()
    {
        grids[CurrentGridIndex].enabled = false;
        MoveMap(CurrentGridIndex, false);
        CurrentGridIndex = (CurrentGridIndex + 1) % 2;
        grids[CurrentGridIndex].enabled = true;
        MoveMap(CurrentGridIndex, true);
     //切换Grid组而物品不变
        //为避免bug暂停0.2秒   
    }

    public void GameOver()
    {
        if (Bbox && Wbox)
        {
            PlayerObject = GameObject.Find("Player");
            PlayerController = PlayerObject.GetComponent<PlayerController>();
            PlayerController.enabled = false;
            ButtonContainer.SetActive(false);
            StartCoroutine(LoadNextStage()); //它只在需要的时候才执行查询
        }//游戏结束,给出提示，直接进入下一关
    }

    public void ResetScene()//重新加载当前关卡
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);//加载某个编号的关卡(当前关卡)
        //SceneManager.GetActiveScene().buildIndex获取当前关卡Index
    }

    //协程
    IEnumerator LoadNextStage()//这里作为一个延迟执行的机制，它只在需要的时候才执行查询，并返回一个可枚举的序列
    {
        yield return new WaitForSeconds(1);//等待两秒
        if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 4);//返回主菜单,并且关闭音乐，销毁MusicKeep物体
            MusicKeep = GameObject.FindGameObjectWithTag("MusicKeep");
            if (MusicKeep != null)
            {
                MusicKeep.GetComponent<AudioSource>().Pause();
                Destroy(MusicKeep);
            }
            else
            {
                print("MusicKeep is null");
            }
        }

        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);//加载某个编号的关卡(当前关卡)
    }

    public void GamePause()//关停角色的控制
    {
        PlayerController = PlayerObject.GetComponent<PlayerController>();
        PlayerController.enabled = false;
    }

    public void GameAwake()
    {
        Time.timeScale = 1;
        PlayerController = PlayerObject.GetComponent<PlayerController>();
        PlayerController.enabled = true;
    }

    public void BackToMainMenu()
    {
        Gradient.SetActive(false);
        SceneManager.LoadScene(0);//返回主菜单,并且关闭音乐，销毁MusicKeep物体
        MusicKeep = GameObject.FindGameObjectWithTag("MusicKeep");
        if(MusicKeep!=null)
        {
            MusicKeep.GetComponent<AudioSource>().Pause();
            Destroy(MusicKeep);
        }
    }
    void MoveMap(int index, bool mode)//移动莫名生成的方块,mode定义当前地图是否激活
    {
        if (mode)
            grids[index].transform.position = Vector3.zero;
        else
            grids[index].transform.position = Vector2.up*10f;
    }
}
