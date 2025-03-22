using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance; //建一个单例
    public Slider hp;
    private float currentHP;
    
    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        currentHP = 70f;
        SetHPValue(currentHP);
    }

    /// <summary>
    /// 设置血量
    /// </summary>
    /// <param name="new_hp">新的生命值</param>
    public void SetHPValue(float new_hp)
    {
        currentHP = new_hp;
        hp.value = new_hp;
    }
}
