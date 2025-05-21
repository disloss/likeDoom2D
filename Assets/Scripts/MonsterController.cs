using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class MonsterController : MonoBehaviour
{
    private new Rigidbody2D rigidbody2D; //�������
    private Animator animator; //�������
    public Transform playerTransform; //��ҵ�transform���
    private SpriteRenderer CharacterSpriteRenderer; //���ﾫ�����

    public float moveSpeed = 5; //�ƶ��ٶ�


    private void Start()
    {
        CharacterSpriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector3 aimDirection = (playerTransform.position - transform.position).normalized;
        if (aimDirection.x > 0)
        {
            CharacterSpriteRenderer.flipX = false; // ���ң�Ĭ�Ϸ���
        }
        else if (aimDirection.x < 0)
        {
            CharacterSpriteRenderer.flipX = true; // ���󣨷�תSprite��
        }
        rigidbody2D.velocity = aimDirection * moveSpeed;
    }

}
