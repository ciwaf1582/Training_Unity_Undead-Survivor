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
    
    void Awake() // 초기화
    {
        rigid = GetComponent<Rigidbody2D>(); // 오브젝트에서 컴포넌트를 가져오는 함수
        spriter = GetComponent<SpriteRenderer>();                                    
    }
    void Update()
    {
        // GetAxis : 부드럽게 보정(float), GetAxisRaw : 딱딱하게 보정(int) 
        //inputVec.x = Input.GetAxisRaw("Horizontal");
        //inputVec.y = Input.GetAxisRaw("Vertical");

    }
    void FixedUpdate() // 물리 업데이트
    {
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
        if (inputVec.x != 0)
        {
            spriter.flipX = inputVec.x < 0;
        }
    }
    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>(); // Player Input 컴퍼넌트에서 자체적인 normalized을 받음
    }
}
