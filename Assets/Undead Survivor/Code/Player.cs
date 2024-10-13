using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed;
    public Scanner scanner;
    public Hand[] hands;
    public RuntimeAnimatorController[] animCon;

    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;

    void Awake() // �ʱ�ȭ
    {
        rigid = GetComponent<Rigidbody2D>(); // ������Ʈ���� ������Ʈ�� �������� �Լ�
        spriter = GetComponent<SpriteRenderer>();    
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
        hands = GetComponentsInChildren<Hand>(true); //�ڽ��̰� 2���ϱ�...
    }
    private void OnEnable()
    {
        speed *= Character.Speed;
        anim.runtimeAnimatorController = animCon[GameManager.instance.playerId];
    }
    void Update()
    {
        if (!GameManager.instance.isLive) return;

        // GetAxis: �ε巴�� ����(float), GetAxisRaw: �����ϰ� ����(int)
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");

    }
    void FixedUpdate() // ���� ������Ʈ
    {
        if (!GameManager.instance.isLive) return;
        // 1. ���� �ش�
        // rigid.AddForce(inputVec);
        // 2. �ӵ� ����
        // rigid.velocity = inputVec;
        // 3. ��ġ �̵�                                 Time.deltaTime : Updata()���� ���
        // �밢������ �̵� �ÿ��� ���� ��                 Time.fixedDeltaTime : FixedUpdate()���� ���    
        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime; 
        rigid.MovePosition(rigid.position + nextVec); // �� ��ġ + ���ư� ��ġ
    }
    void LateUpdate() //Update()�� ������ ���� ������ ���� ����Ǵ� �Լ�
    {
        if (!GameManager.instance.isLive) return;

        anim.SetFloat("Speed", inputVec.magnitude); // ������ ����

        if (inputVec.x != 0)
        {
            spriter.flipX = inputVec.x < 0;
        }
    }
    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>(); // Player Input ���۳�Ʈ���� ��ü���� normalized�� ����
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!GameManager.instance.isLive) return;

        GameManager.instance.health -= Time.deltaTime * 10;

        if (GameManager.instance.health < 0)
        {
            for (int i = 2; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            anim.SetTrigger("Dead");
            GameManager.instance.GameOver();
        }
    }
}
