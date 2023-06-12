using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public AudioSource audioSource;//声明开始即播放的音乐
    //public AudioSource[] audioSources;//用于音乐角
    public void StartGame()//关闭音乐开始游戏
    {
        audioSource.Pause();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);//进入下一个场景，即为开始游戏
    }

    public void Quit()
    {
        Application.Quit();//退出游戏
    }
}
