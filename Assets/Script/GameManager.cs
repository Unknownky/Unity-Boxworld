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
    public Grid[] grids;
    private int CurrentGridIndex;
    public bool Wbox = false;
    public bool Bbox = false;
    

    [Header("Gradient")]
    public GameObject Gradient;

    private GameObject MusicKeep;
    private GameObject PlayerObject;
    private PlayerController PlayerController;
    private GameObject ButtonContainer = null;

    //�ڸýű��н�����Ϸ�������ж�����Ϸ�������л�����Ϸ��ͼ���л�����Ϸ���ֵĹ���
    private void Awake()
    {
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
        if (Input.GetKeyDown(KeyCode.R))//�ؿ���ǰ�ؿ�
            ResetScene();
        if(Input.GetKeyDown(KeyCode.C))
            SwitchMap();
    }

    public void SwitchMap()
    {
        grids[CurrentGridIndex].enabled = false;
        MoveMap(CurrentGridIndex, false);
        CurrentGridIndex = (CurrentGridIndex + 1) % 2;
        grids[CurrentGridIndex].enabled = true;
        MoveMap(CurrentGridIndex, true);
     //�л�Grid�����Ʒ����
        //Ϊ����bug��ͣ0.2��   
    }

    public void GameOver()
    {
        if (Bbox && Wbox)
        {
            PlayerObject = GameObject.Find("Player");
            PlayerController = PlayerObject.GetComponent<PlayerController>();
            PlayerController.enabled = false;
            ButtonContainer.SetActive(false);
            StartCoroutine(LoadNextStage()); //��ֻ����Ҫ��ʱ���ִ�в�ѯ
        }//��Ϸ����,������ʾ��ֱ�ӽ�����һ��
    }

    public void ResetScene()//���¼��ص�ǰ�ؿ�
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);//����ĳ����ŵĹؿ�(��ǰ�ؿ�)
        //SceneManager.GetActiveScene().buildIndex��ȡ��ǰ�ؿ�Index
    }

    //Э��
    IEnumerator LoadNextStage()//������Ϊһ���ӳ�ִ�еĻ��ƣ���ֻ����Ҫ��ʱ���ִ�в�ѯ��������һ����ö�ٵ�����
    {
        yield return new WaitForSeconds(1);//�ȴ�����
        if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 4);//�������˵�,���ҹر����֣�����MusicKeep����
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
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);//����ĳ����ŵĹؿ�(��ǰ�ؿ�)
    }

    public void GamePause()//��ͣ��ɫ�Ŀ���
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
        SceneManager.LoadScene(0);//�������˵�,���ҹر����֣�����MusicKeep����
        MusicKeep = GameObject.FindGameObjectWithTag("MusicKeep");
        if(MusicKeep!=null)
        {
            MusicKeep.GetComponent<AudioSource>().Pause();
            Destroy(MusicKeep);
        }
    }
    void MoveMap(int index, bool mode)//�ƶ�Ī�����ɵķ���,mode���嵱ǰ��ͼ�Ƿ񼤻�
    {
        if (mode)
            grids[index].transform.position = Vector3.zero;
        else
            grids[index].transform.position = Vector2.up*10f;
    }
}
