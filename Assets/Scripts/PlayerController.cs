using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed; //�ƶ��ٶ�
    private Vector2 input; //�������������
    private Animator animator; //�������
    public LayerMask solidObjectsLayer; //ʵ������
    public new Rigidbody2D rigidbody2D; //�������
    private SpriteRenderer CharacterSpriteRenderer; //���ﾫ�����
    public Transform HandAim; //�ֲ��ı任���
    public Camera mainCamera; //�����

    public GameObject bulletPrefab;  // �ӵ�Ԥ����
    public Transform muzzle;        // ǹ��λ��
    private float bulletSpeed = 20f; // �ӵ��ٶ�
    private float fireRate = 0.2f;   // ���������룩
    private float nextFireTime = 0f;


    private void Start()
    {
        //��ʼ��
        animator = GetComponent<Animator>();
        CharacterSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public Vector3 FaceDir()//��ȡ����ķ���ĺ���
    {
        return new Vector3(animator.GetFloat("MoveX"), animator.GetFloat("MoveY"));
    }

    // Update is called once per frame
    public void Update()//�����ƶ�ʱʹ�õ�֡
    {
        Move(); //�ƶ���غ�������Ҫ�����ƶ�
        Hand(); //�ֲ���غ���������������׼��������
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

            //���ж��Ƿ�������ߣ����ƶ���Ŀ��λ��
            if (IsWalkable(transform.position, targetPos))
            {
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
        }
        else
        {
            //�������õ��ƶ�����ͬ����ȥ
            animator.SetBool("IsMoving", false);
            rigidbody2D.velocity = Vector2.zero;
        }
    }


    private void Hand()
    {
        Vector3 mouseScreenPos = Input.mousePosition; //��ȡ������Ļλ��
        Vector3 HandscreenPos = mainCamera.WorldToScreenPoint(HandAim.position); // ��ȡ�ֲ�����Ļλ��
        Vector3 aimDirection = (mouseScreenPos - HandscreenPos).normalized; // �õ����������ֲ���λ��
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg; // ��ȡŷ����
        if(angle > 90 || angle < -90) //ʹ�ַ�ת
        {
            HandAim.localScale = new Vector3(1, -1, 1);
        }
        else
        {
            HandAim.localScale = new Vector3(1, 1, 1);
        }
        HandAim.eulerAngles = new Vector3(0, 0, angle); //ʹ����ת

        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(Shoot());
            nextFireTime = Time.time + fireRate;
        }
    }

    IEnumerator Shoot()
    {
        // ʵ�����ӵ�
        GameObject bullet = Instantiate(bulletPrefab, muzzle.position, muzzle.rotation);
        bullet.SetActive(true);
        // �����ӵ����򣨸���ǹ�ڳ���
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.velocity = muzzle.right * bulletSpeed;

        // ��ѡ�����������Ч
        // AudioSource.PlayClipAtPoint(shootSound, muzzle.position);
        yield return new WaitForSecondsRealtime(3f);
        Destroy(bullet);
    }



    private bool IsWalkable(Vector3 startPos, Vector3 targetPos)//�ж���һ�������Ƿ����
    {
        if (Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer) != null)//�����ǰһ����ʵ�������NPC�������ƶ�
        {
            return false;
        }
        return true;
    }

}
