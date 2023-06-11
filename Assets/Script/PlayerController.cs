using JetBrains.Annotations;
using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{

    Vector2 moveDir;
    public LayerMask detectLayer;//只检测wall层
    public LayerMask boxLayer;//只检测Box层
    private int Mancd = 0;//储存人物状态;0:normal; 1:black; 2:white
    public string BlackBoxTag;
    public string WhiteBoxTag;
    private string Line = "a";//表示全部的方向
    private Animator animator;
    private new Rigidbody2D rigidbody2D;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();//给组件赋值避免无法运行
    }
    // Update is called once per frame
    void Update()
    {
        //暂且使用Vector2
        moveDir = Vector2.zero;
        //进行移动操作
        if (Input.GetKeyDown(KeyCode.RightArrow))
            moveDir = Vector2.right;
        

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            moveDir = Vector2.left;

        if (Input.GetKeyDown(KeyCode.DownArrow))
            moveDir = Vector2.down;

        if (Input.GetKeyDown(KeyCode.UpArrow))
            moveDir = Vector2.up;

        //if(Input.GetKeyDown(KeyCode.P))
        //GUI部分
        //if(Input.GetKeyDown(KeyCode.Escape))

        if (Input.GetKeyDown(KeyCode.E))//改变人物状态
        {
            Check();
        }

        //没有明显转向
        //if (moveDir == Vector2.right)//使用欧拉角进行朝向的转变
        //    rigidbody2D.transform.eulerAngles = Vector3.right;
        //else if (moveDir == Vector2.left)
        //    rigidbody2D.transform.eulerAngles = Vector3.left;

        //限定运动轴
        if (Line == "x" && (moveDir == Vector2.up || moveDir == Vector2.down))
            moveDir = Vector2.zero;

        if(Line == "y" && (moveDir == Vector2.right || moveDir == Vector2.left))
            moveDir = Vector2.zero;


        if (moveDir != Vector2.zero)//向量不为零时
        {
            if (CanMoveToDir(moveDir))
            {
                Move(moveDir);
            }
        }

        moveDir = Vector2.zero;
    }
    bool CanMoveToDir(Vector2 dir)
    {
        RaycastHit2D hitblow = Physics2D.Raycast(transform.position, dir, 0.2f, detectLayer); // 发射射线检测当前地面
        RaycastHit2D hitblow2 = Physics2D.Raycast(transform.position, dir, 1.0f, detectLayer); // 发射射线检测前面的地面，只检测Wall层
                                                                                               //2D向量转为3D，还不太清楚为什么
                                                                                               //检测地面用于判断移动，检测箱子用于进一步的判断

        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 1.5f, boxLayer); // 发射射线检测，只检测box层

        //对于特殊状态的运动判定：黑状态，同原逻辑；白状态，逻辑稍微改变；normal：不调用Box的方法
        if(!hitblow && !hitblow2)
        {
            string blowtag = hitblow.collider != null ? hitblow.collider.tag : null;//地面标签
            string blowtag2 = hitblow2.collider != null ? hitblow2.collider.tag : null;//检测地面标签
            //找了挺久的，需要先检查是否为null,三目运算符

            if (blowtag == blowtag2)//如果检测地面标签与当前地面标签相同
            {            
                if (Mancd == 0 && !hit)
                    return true;

                //hit.collider.TryGetComponent<Box>(out var box);
                if (Mancd==1)//黑，与原逻辑同
                {
                    if (hit.collider != null && hit.collider.TryGetComponent<Box>(out var box))
                    {
                        return box.canMoveToDir(dir, 1);//移动箱子
                        //移动人
                    }
                }

                if (Mancd == 2)//白,说明有箱子并且移动方向有效
                {//先得到该箱体
                    RaycastHit2D hitback = Physics2D.Raycast(transform.position, dir*-1, 1.5f, boxLayer); // 发射射线检测，只检测box层
                    if (hitback.collider != null && hitback.collider.TryGetComponent<Box>(out var boxback))
                    { 
                        boxback.canMoveToDir(dir, 2);//调用box的方法，具体box的方法进行判断,箱子和人物是同一个方向运动
                        Move(dir);//移动人
                    }
                }
            }
            else//地面标签不同，那么不移动
            {
                return false;
            }
        }

        return false;
    }


    void Move(Vector2 dir)
    {
        transform.Translate(dir);
    }

    void Check()
    {
        if(Mancd != 0)//正常状态直接转变
        {
            Mancd = 0;
            animator.SetInteger("State", 0);
            Line = "a";
            return;
        }

        Vector2[] Direction = new Vector2[4] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
        foreach(Vector2 dir in Direction)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 1.5f, boxLayer); // 发射射线检测，只检测box层
            if (hit)
            {
                if (hit.collider != null && hit.collider.TryGetComponent<Box>(out var box))
                {
                    string boxtag = hit.collider.gameObject.tag;
                    if (boxtag == BlackBoxTag)
                    {
                        Mancd = 1;//调整状态的同时改变Line
                        animator.SetInteger("State", 1);
                        LimitLine(dir);
                        return;//避免后面的判断，也就是单一判断，之后如果有时间再改**********
                    }
                    else if(boxtag == WhiteBoxTag)
                    {
                        Mancd = 2;
                        animator.SetInteger("State", 2);
                        LimitLine(dir);
                        return;
                    }
                }
        
            }//打到箱子了
        }
    }

    void LimitLine(Vector2 dir)//限定运动的轴
    {
        if (dir == Vector2.down || dir == Vector2.up)
            Line = "y";

        if (dir == Vector2.right || dir == Vector2.left)
            Line = "x";
    }
}
