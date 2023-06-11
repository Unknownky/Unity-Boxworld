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
    // 创建一个颜色渐变对象
    private Gradient gradient;

    void Start()
    {
        gameManager = GameObject.Find("Manager").GetComponent<GameManager>();
        //关停角色脚本
        gameManager.GamePause();

        i = 0;
        countTime = 0;
        image = GetComponent<Image>();
        Mater = image.material;
        Text = GetComponentInChildren<TMP_Text>();
        Button = GetComponent<Button>();
        Button.enabled = false;
        Invoke("ShowText", 2);
        // 初始化颜色渐变对象,自带的渐变器
        gradient = new Gradient();
        // 设置颜色渐变的关键点，这里设置了两个，一个是黑色，一个是白色
        gradient.colorKeys = new GradientColorKey[] {
            new GradientColorKey(Color.black, 0f),
            new GradientColorKey(Color.white, 1f)
        };
        // 设置透明度渐变的关键点，这里设置了两个，都是不透明
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
            // 使用Evaluate方法根据i的值获取对应的颜色
            Mater.color = gradient.Evaluate(i);
            // 让i以0.01f的步长递增，当i达到1时，颜色就是白色
            i += 0.01f;
        }
    }
    void ShowText()
    {
        Text.enabled = true;//显示对应的Text
        Button.enabled = true;
    }

    private void OnDisable()
    {
        Mater.color = gradient.Evaluate(0);
    }
}

