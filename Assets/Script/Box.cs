using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Box : MonoBehaviour
{
    public LayerMask OriginalLayer;//声明检测层避免检测到Target而影响原逻辑
    private string currentTag;

    private float _scale;
    private void Start()
    {
        _scale = PlayerController._scale;
        Debug.Log(_scale);
        currentTag = gameObject.tag;//获取当前物体的Tag
    }
    public bool canMoveToDir(Vector2 dir, int Mancd)//与人物运动方向相同
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position+_scale * 0.45f * (Vector3)dir, dir, 0.5f*_scale, OriginalLayer); // 发射射线向前检测
        //由于使用了Composite Collider 2D内部应该检测不到,可以减少判断，但是仍然需要外边界的判断
        //Debug.DrawRay(transform.position + (Vector3)dir * 0.5f, dir * 0.5f, Color.red, 1f);//检测出错，进行查错

        //无法推入交接面，无法同时推两个箱子,
        if (!hit || Mancd==2)
        {
            transform.Translate(dir*_scale);
            return true;
        }
        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BlackTarget") || collision.CompareTag("WhiteTarget"))
        {
            if (currentTag == "WhiteBox" && collision.CompareTag("WhiteTarget"))
                FindObjectOfType<GameManager>().Wbox = true;

            if (currentTag == "BlackBox" && collision.CompareTag("BlackTarget"))
                FindObjectOfType<GameManager>().Bbox = true;

            FindObjectOfType<GameManager>().GameOver();//判断游戏是否结束S
        }
    }
    private void OnTriggerExit2D(Collider2D collision)//移出当前检测器时触发，使箱子完成状态改变
    {
        if(collision.CompareTag("BlackTarget") || collision.CompareTag("WhiteTarget"))
        {
            if (currentTag == "WhiteBox" && collision.CompareTag("WhiteTarget"))
                FindObjectOfType<GameManager>().Wbox = false;
            if(currentTag == "BlackBox" && collision.CompareTag("BlackTarget"))
                FindObjectOfType<GameManager>().Bbox= false;
        }//移出不需要判断是否游戏结束
    }

}
