using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed;

    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;
    void Awake() // �ʱ�ȭ
    {
        rigid = GetComponent<Rigidbody2D>(); // ������Ʈ���� ������Ʈ�� �������� �Լ�
        spriter = GetComponent<SpriteRenderer>();    
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        // GetAxis : �ε巴�� ����(float), GetAxisRaw : �����ϰ� ����(int) 
        //inputVec.x = Input.GetAxisRaw("Horizontal");
        //inputVec.y = Input.GetAxisRaw("Vertical");

    }
    void FixedUpdate() // ���� ������Ʈ
    {
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
}
