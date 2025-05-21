using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor.Timeline;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector2 input; //�������������
    private Animator animator; //�������
    public LayerMask solidObjectsLayer; //ʵ������
    private new Rigidbody2D rigidbody2D; //�������
    private SpriteRenderer CharacterSpriteRenderer; //���ﾫ�����
    private Transform handAim; //�ֲ��ı任���
    public Camera mainCamera; //�����
    public GameObject bulletPrefab; // �ӵ�Ԥ����
    private Transform muzzle; // ǹ��λ��
    public AudioClip shootSound; //�����Ч

    public float moveSpeed = 5; //�ƶ��ٶ�
    private float bulletSpeed = 60f; // �ӵ��ٶ�
    private float currentHP = 50; //��ǰѪ��
    private float maxHP = 100; //���Ѫ��

    private void Start()
    {
        //��ʼ��
        animator = GetComponent<Animator>();
        CharacterSpriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        handAim = transform.Find("Hand");
        muzzle = transform.Find("Hand/muzzle");

    }

    public Vector3 FaceDir()//��ȡ����ķ���ĺ���
    {
        return new Vector3(animator.GetFloat("MoveX"), animator.GetFloat("MoveY"));
    }

    public void Update()//�����ƶ�ʱʹ�õ�֡
    {
        Hand(); //�ֲ���غ���������������׼��������
        Move(); //�ƶ���غ�������Ҫ�����ƶ�
    }


    private void Move()
    {
        //�ⲿ�������ж��Ƿ������룬���ص�ֵ��-1��0��1�����Ƶĺ�������GetAxis�������𽥱仯��
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        //������ƶ����룬���ɫ��ʼ�ƶ�
        if (input != Vector2.zero)
        {
            //�������õ��ƶ�����ͬ����ȥ
            animator.SetBool("IsMoving", true);

            //���Ŀ��λ��
            Vector2 targetPos = transform.position;//Ŀ��λ�ã��Ȼ�ȡ��ǰλ�ã��ٸ��������ǰλ�ñ䶯��Ӧ����Vector3����
            targetPos.x += input.x;
            targetPos.y += input.y;

            
            // �������뷽��תSprite
            if (input.x > 0)
            {
                CharacterSpriteRenderer.flipX = false; // ���ң�Ĭ�Ϸ���
            }
            else if (input.x < 0)
            {
                CharacterSpriteRenderer.flipX = true; // ���󣨷�תSprite��
            }
            rigidbody2D.velocity = input * moveSpeed;
        }
        else
        {
            //�������õ��ƶ�����ͬ����ȥ
            animator.SetBool("IsMoving", false);
            rigidbody2D.velocity = Vector2.zero;
        }

        //�����Ұ��¿ո���ʼ����
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 aimDirection = input.normalized; //��ȡ��ǰǰ���ķ���
            transform.DOMove(transform.position + (5f * aimDirection), 0.3f).SetEase(Ease.InOutSine); //�����ɣ�
        }
    }


    private void Hand()
    {
        Vector3 mouseScreenPos = Input.mousePosition; //��ȡ������Ļλ��
        Vector3 HandscreenPos = mainCamera.WorldToScreenPoint(handAim.position); // ��ȡ�ֲ�����Ļλ��
        Vector3 aimDirection = (mouseScreenPos - HandscreenPos).normalized; // �õ����������ֲ���λ��
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg; // ��ȡŷ����
        if(angle > 90 || angle < -90) //ʹ�ַ�ת
        {
            handAim.localScale = new Vector3(1, -1, 1);
        }
        else
        {
            handAim.localScale = new Vector3(1, 1, 1);
        }
        handAim.eulerAngles = new Vector3(0, 0, angle); //ʹ����ת

        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(Shoot());
        }
    }

    IEnumerator Shoot()
    {
        // ʵ�����ӵ�
        GameObject bullet = Instantiate(bulletPrefab, muzzle.position, muzzle.rotation);
        //bullet.SetActive(true);
        // �����ӵ�������ٶȣ�����ǹ�ڳ���
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.velocity = muzzle.right * bulletSpeed;

        // ��ѡ�����������Ч
         AudioSource.PlayClipAtPoint(shootSound, muzzle.position);
        yield return new WaitForSecondsRealtime(3f);
        Destroy(bullet);
    }



    public void HealthChange(float changeHP)
    {
        currentHP = Mathf.Min(currentHP + changeHP, maxHP);
        if(currentHP <= 0)
        {
            Debug.Log("�������");
        }
        UIManager.Instance.SetHP(currentHP);
    }

}
