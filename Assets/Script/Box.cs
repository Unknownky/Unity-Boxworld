using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Box : MonoBehaviour
{
    public LayerMask OriginalLayer;//������������⵽Target��Ӱ��ԭ�߼�
    private string currentTag;

    private float _scale;
    private void Start()
    {
        _scale = PlayerController._scale;
        Debug.Log(_scale);
        currentTag = gameObject.tag;//��ȡ��ǰ�����Tag
    }
    public bool canMoveToDir(Vector2 dir, int Mancd)//�������˶�������ͬ
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position+_scale * 0.45f * (Vector3)dir, dir, 0.5f*_scale, OriginalLayer); // ����������ǰ���
        //����ʹ����Composite Collider 2D�ڲ�Ӧ�ü�ⲻ��,���Լ����жϣ�������Ȼ��Ҫ��߽���ж�
        //Debug.DrawRay(transform.position + (Vector3)dir * 0.5f, dir * 0.5f, Color.red, 1f);//���������в��

        //�޷����뽻���棬�޷�ͬʱ����������,
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

            FindObjectOfType<GameManager>().GameOver();//�ж���Ϸ�Ƿ����S
        }
    }
    private void OnTriggerExit2D(Collider2D collision)//�Ƴ���ǰ�����ʱ������ʹ�������״̬�ı�
    {
        if(collision.CompareTag("BlackTarget") || collision.CompareTag("WhiteTarget"))
        {
            if (currentTag == "WhiteBox" && collision.CompareTag("WhiteTarget"))
                FindObjectOfType<GameManager>().Wbox = false;
            if(currentTag == "BlackBox" && collision.CompareTag("BlackTarget"))
                FindObjectOfType<GameManager>().Bbox= false;
        }//�Ƴ�����Ҫ�ж��Ƿ���Ϸ����
    }

}
