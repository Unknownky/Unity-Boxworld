using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GradientController : MonoBehaviour
{
    private Image image;
    private Material Mater;
    private TMP_Text Text;
    private Button Button;
    private float countTime;
    public float timelep;
    private float i;

    private GameManager gameManager;
    // ����һ����ɫ�������
    private Gradient gradient;

    void Start()
    {
        gameManager = GameObject.Find("Manager").GetComponent<GameManager>();
        //��ͣ��ɫ�ű�
        gameManager.GamePause();

        i = 0;
        countTime = 0;
        image = GetComponent<Image>();
        Mater = image.material;
        Text = GetComponentInChildren<TMP_Text>();
        Button = GetComponent<Button>();
        Button.enabled = false;
        Invoke("ShowText", 2);
        // ��ʼ����ɫ�������,�Դ��Ľ�����
        gradient = new Gradient();
        // ������ɫ����Ĺؼ��㣬����������������һ���Ǻ�ɫ��һ���ǰ�ɫ
        gradient.colorKeys = new GradientColorKey[] {
            new GradientColorKey(Color.black, 0f),
            new GradientColorKey(Color.white, 1f)
        };
        // ����͸���Ƚ���Ĺؼ��㣬�������������������ǲ�͸��
        gradient.alphaKeys = new GradientAlphaKey[] {
            new GradientAlphaKey(1f, 0f),
            new GradientAlphaKey(1f, 1f)
        };
    }

    void Update()
    {
        if (i < 1f)
        {
            Gradient();
        }
    }
    void Gradient()
    {
        countTime += Time.deltaTime;
        if (countTime > timelep)
        {
            countTime = 0;
            // ʹ��Evaluate��������i��ֵ��ȡ��Ӧ����ɫ
            Mater.color = gradient.Evaluate(i);
            // ��i��0.01f�Ĳ�����������i�ﵽ1ʱ����ɫ���ǰ�ɫ
            i += 0.01f;
        }
    }
    void ShowText()
    {
        Text.enabled = true;//��ʾ��Ӧ��Text
        Button.enabled = true;
    }

    private void OnDisable()
    {
        Mater.color = gradient.Evaluate(0);
    }
}

