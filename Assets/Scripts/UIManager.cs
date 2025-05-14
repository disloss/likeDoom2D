using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance; //����UIManager�ĵ���
    public Slider HPSlider; //��ȡHP�Ļ�����
    public Text nameText; //��ȡ���Ƶ��ı���

    private float maxHP = 100; //���Ѫ��
    private float currentHP = 50; //��ǰѪ��
    private string playerName = "˧�������"; //�������


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SetHP(currentHP);
        SetName(playerName);
    }

    public void SetName(string newName) //����������
    {
        playerName = newName;
        nameText.text = playerName;
    }

    public void SetHP(float newHP) //�����µ�Ѫ��
    {
        if (newHP >= 0 && newHP <= maxHP) //�����Ѫ���Ƿ�Ϸ�
        {
            currentHP = newHP;
            float percentage = currentHP / maxHP;
            HPSlider.value = percentage;
        }
        else return;
    }


}
