using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class MonsterController : MonoBehaviour
{
    private new Rigidbody2D rigidbody2D; //刚体组件
    private Animator animator; //动画组件
    public Transform playerTransform; //玩家的transform组件
    private SpriteRenderer CharacterSpriteRenderer; //人物精灵组件

    public float moveSpeed = 5; //移动速度


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
            CharacterSpriteRenderer.flipX = false; // 向右（默认方向）
        }
        else if (aimDirection.x < 0)
        {
            CharacterSpriteRenderer.flipX = true; // 向左（翻转Sprite）
        }
        rigidbody2D.velocity = aimDirection * moveSpeed;
    }

}
