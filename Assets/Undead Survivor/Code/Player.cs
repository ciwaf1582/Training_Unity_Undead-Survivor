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

    void Awake() // 초기화
    {
        rigid = GetComponent<Rigidbody2D>(); // 오브젝트에서 컴포넌트를 가져오는 함수
        spriter = GetComponent<SpriteRenderer>();    
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
        hands = GetComponentsInChildren<Hand>(true); //자식이고 2개니까...
    }
    private void OnEnable()
    {
        speed *= Character.Speed;
        anim.runtimeAnimatorController = animCon[GameManager.instance.playerId];
    }
    void Update()
    {
        if (!GameManager.instance.isLive) return;

        // GetAxis: 부드럽게 보정(float), GetAxisRaw: 딱딱하게 보정(int)
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");

    }
    void FixedUpdate() // 물리 업데이트
    {
        if (!GameManager.instance.isLive) return;
        // 1. 힘을 준다
        // rigid.AddForce(inputVec);
        // 2. 속도 제어
        // rigid.velocity = inputVec;
        // 3. 위치 이동                                 Time.deltaTime : Updata()에서 사용
        // 대각선으로 이동 시에도 같은 값                 Time.fixedDeltaTime : FixedUpdate()에서 사용    
        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime; 
        rigid.MovePosition(rigid.position + nextVec); // 현 위치 + 나아갈 위치
    }
    void LateUpdate() //Update()가 끝나고 다음 프레임 전에 실행되는 함수
    {
        if (!GameManager.instance.isLive) return;

        anim.SetFloat("Speed", inputVec.magnitude); // 벡터의 길이

        if (inputVec.x != 0)
        {
            spriter.flipX = inputVec.x < 0;
        }
    }
    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>(); // Player Input 컴퍼넌트에서 자체적인 normalized을 받음
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
