using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed; //移动速度
    private Vector2 input; //两个方向的向量
    private Animator animator; //动画组件
    public LayerMask solidObjectsLayer; //实体对象层
    public new Rigidbody2D rigidbody2D; //刚体组件
    private SpriteRenderer CharacterSpriteRenderer; //人物精灵组件
    public Transform HandAim; //手部的变换组件
    public Camera mainCamera; //主相机

    public GameObject bulletPrefab;  // 子弹预制体
    public Transform muzzle;        // 枪口位置
    private float bulletSpeed = 20f; // 子弹速度
    private float fireRate = 0.2f;   // 射击间隔（秒）
    private float nextFireTime = 0f;


    private void Start()
    {
        //初始化
        animator = GetComponent<Animator>();
        CharacterSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public Vector3 FaceDir()//获取面向的方向的函数
    {
        return new Vector3(animator.GetFloat("MoveX"), animator.GetFloat("MoveY"));
    }

    // Update is called once per frame
    public void Update()//自由移动时使用的帧
    {
        Move(); //移动相关函数，主要负责移动
        Hand(); //手部相关函数，负责武器瞄准，攻击等
    }


    private void Move()
    {
        //这部分用于判断是否有输入，返回的值是-1、0、1。类似的函数还有GetAxis，它会逐渐变化。
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        //如果有移动输入，则角色开始移动
        if (input != Vector2.zero)
        {
            //将动画用的移动参数同步过去
            animator.SetBool("IsMoving", true);

            //算出目标位置
            Vector2 targetPos = transform.position;//目标位置，先获取当前位置，再根据这个当前位置变动，应该是Vector3类型
            targetPos.x += input.x;
            targetPos.y += input.y;

            //先判断是否可以行走，再移动到目标位置
            if (IsWalkable(transform.position, targetPos))
            {
                // 根据输入方向翻转Sprite
                if (input.x > 0)
                {
                    CharacterSpriteRenderer.flipX = false; // 向右（默认方向）
                }
                else if (input.x < 0)
                {
                    CharacterSpriteRenderer.flipX = true; // 向左（翻转Sprite）
                }
                rigidbody2D.velocity = input * moveSpeed;
            }
        }
        else
        {
            //将动画用的移动参数同步过去
            animator.SetBool("IsMoving", false);
            rigidbody2D.velocity = Vector2.zero;
        }
    }


    private void Hand()
    {
        Vector3 mouseScreenPos = Input.mousePosition; //获取鼠标的屏幕位置
        Vector3 HandscreenPos = mainCamera.WorldToScreenPoint(HandAim.position); // 获取手部的屏幕位置
        Vector3 aimDirection = (mouseScreenPos - HandscreenPos).normalized; // 得到鼠标相对于手部的位置
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg; // 获取欧拉角
        if(angle > 90 || angle < -90) //使手翻转
        {
            HandAim.localScale = new Vector3(1, -1, 1);
        }
        else
        {
            HandAim.localScale = new Vector3(1, 1, 1);
        }
        HandAim.eulerAngles = new Vector3(0, 0, angle); //使手旋转

        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(Shoot());
            nextFireTime = Time.time + fireRate;
        }
    }

    IEnumerator Shoot()
    {
        // 实例化子弹
        GameObject bullet = Instantiate(bulletPrefab, muzzle.position, muzzle.rotation);
        bullet.SetActive(true);
        // 设置子弹方向（根据枪口朝向）
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.velocity = muzzle.right * bulletSpeed;

        // 可选：播放射击音效
        // AudioSource.PlayClipAtPoint(shootSound, muzzle.position);
        yield return new WaitForSecondsRealtime(3f);
        Destroy(bullet);
    }



    private bool IsWalkable(Vector3 startPos, Vector3 targetPos)//判断这一次行走是否可行
    {
        if (Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer) != null)//如果面前一格有实体或者有NPC，则不能移动
        {
            return false;
        }
        return true;
    }

}
