using JetBrains.Annotations;
using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    Vector2 moveDir;

    [Header("Boxs Detect")]
    public LayerMask detectLayer;//ֻ���wall��
    public LayerMask boxLayer;//ֻ���Box��
    public string BlackBoxTag = null;
    public string WhiteBoxTag = null;

    private int Mancd = 0;//��������״̬;0:normal; 1:black; 2:white
    private string Line = "a";//��ʾȫ���ķ���
    private Animator animator = null;
    private new Rigidbody2D rigidbody2D = null;


    private void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();//�������ֵ�����޷�����
    }
    // Update is called once per frame
    void Update()
    {
        PlayerMovement("None");
    }

    public void PlayerMovement(string clickstring)
    {
        //����ʹ��Vector2
        moveDir = Vector2.zero;

        //�����ƶ�����
        if (Input.GetKeyDown(KeyCode.RightArrow) || clickstring=="Right")
            moveDir = Vector2.right;


        if (Input.GetKeyDown(KeyCode.LeftArrow) || clickstring == "Left")
            moveDir = Vector2.left;

        if (Input.GetKeyDown(KeyCode.DownArrow) || clickstring == "Down")
            moveDir = Vector2.down;

        if (Input.GetKeyDown(KeyCode.UpArrow) || clickstring == "Up")
            moveDir = Vector2.up;

        if (Input.GetKeyDown(KeyCode.E))//�ı�����״̬
        {
            Check();
        }

        if (Line == "x" && (moveDir == Vector2.up || moveDir == Vector2.down))
            moveDir = Vector2.zero;

        if (Line == "y" && (moveDir == Vector2.right || moveDir == Vector2.left))
            moveDir = Vector2.zero;


        if (moveDir != Vector2.zero)//������Ϊ��ʱ
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
        RaycastHit2D hitblow = Physics2D.Raycast(transform.position, dir, 0.2f, detectLayer); // �������߼�⵱ǰ����
        RaycastHit2D hitblow2 = Physics2D.Raycast(transform.position, dir, 1.0f, detectLayer); // �������߼��ǰ��ĵ��棬ֻ���Wall��
                                                                                               //2D����תΪ3D������̫���Ϊʲô
                                                                                               //�����������ж��ƶ�������������ڽ�һ�����ж�

        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 1.5f, boxLayer); // �������߼�⣬ֻ���box��

        //��������״̬���˶��ж�����״̬��ͬԭ�߼�����״̬���߼���΢�ı䣻normal��������Box�ķ���
        if(!hitblow && !hitblow2)
        {
            string blowtag = hitblow.collider != null ? hitblow.collider.tag : null;//�����ǩ
            string blowtag2 = hitblow2.collider != null ? hitblow2.collider.tag : null;//�������ǩ
            //����ͦ�õģ���Ҫ�ȼ���Ƿ�Ϊnull,��Ŀ�����

            if (blowtag == blowtag2)//����������ǩ�뵱ǰ�����ǩ��ͬ
            {            
                if (Mancd == 0 && !hit)
                    return true;

                //hit.collider.TryGetComponent<Box>(out var box);
                if (Mancd==1)//�ڣ���ԭ�߼�ͬ
                {
                    if (hit.collider != null && hit.collider.TryGetComponent<Box>(out var box))
                    {
                        return box.canMoveToDir(dir, 1);//�ƶ�����
                        //�ƶ���
                    }
                }

                if (Mancd == 2)//��,˵�������Ӳ����ƶ�������Ч
                {//�ȵõ�������
                    RaycastHit2D hitback = Physics2D.Raycast(transform.position, dir*-1, 1.5f, boxLayer); // �������߼�⣬ֻ���box��
                    if (hitback.collider != null && hitback.collider.TryGetComponent<Box>(out var boxback))
                    { 
                        boxback.canMoveToDir(dir, 2);//����box�ķ���������box�ķ��������ж�,���Ӻ�������ͬһ�������˶�
                        Move(dir);//�ƶ���
                    }
                }
            }
            else//�����ǩ��ͬ����ô���ƶ�
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

    public void Check()
    {
        if(Mancd != 0)//����״ֱ̬��ת��
        {
            Mancd = 0;
            animator.SetInteger("State", 0);
            Line = "a";
            return;
        }

        Vector2[] Direction = new Vector2[4] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
        foreach(Vector2 dir in Direction)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 1.5f, boxLayer); // �������߼�⣬ֻ���box��
            if (hit)
            {
                if (hit.collider != null && hit.collider.TryGetComponent<Box>(out var box))
                {
                    string boxtag = hit.collider.gameObject.tag;
                    if (boxtag == BlackBoxTag)
                    {
                        Mancd = 1;//����״̬��ͬʱ�ı�Line
                        animator.SetInteger("State", 1);
                        LimitLine(dir);
                        return;//���������жϣ�Ҳ���ǵ�һ�жϣ�֮�������ʱ���ٸ�**********
                    }
                    else if(boxtag == WhiteBoxTag)
                    {
                        Mancd = 2;
                        animator.SetInteger("State", 2);
                        LimitLine(dir);
                        return;
                    }
                }
        
            }//��������
        }
    }

    void LimitLine(Vector2 dir)//�޶��˶�����
    {
        if (dir == Vector2.down || dir == Vector2.up)
            Line = "y";

        if (dir == Vector2.right || dir == Vector2.left)
            Line = "x";
    }
}
