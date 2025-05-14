using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance; //设置UIManager的单例
    public Slider HPSlider; //获取HP的滑动条
    public Text nameText; //获取名称的文本框

    private float maxHP = 100; //最大血量
    private float currentHP = 50; //当前血量
    private string playerName = "帅气的玩家"; //玩家姓名


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SetHP(currentHP);
        SetName(playerName);
    }

    public void SetName(string newName) //设置新名称
    {
        playerName = newName;
        nameText.text = playerName;
    }

    public void SetHP(float newHP) //设置新的血量
    {
        if (newHP >= 0 && newHP <= maxHP) //检查新血量是否合法
        {
            currentHP = newHP;
            float percentage = currentHP / maxHP;
            HPSlider.value = percentage;
        }
        else return;
    }


}
