using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public AudioSource audioSource;//������ʼ�����ŵ�����
    public GameObject GameObject;
    //public AudioSource[] audioSources;//�������ֽ�


    public void StartGame()//�ر����ֿ�ʼ��Ϸ
    {
        audioSource.Pause();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);//������һ����������Ϊ��ʼ��Ϸ
    }

    //public void MusicHall()//����ѡ�񲥷����ֵ����
    //{



    //}
    public void Quit()
    {
        Application.Quit();//�˳���Ϸ
    }
}