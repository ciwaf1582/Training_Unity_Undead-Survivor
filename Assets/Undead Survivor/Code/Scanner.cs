using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    public float sancRange;
    public LayerMask targetLayer;
    public RaycastHit2D[] targets;
    public Transform nearestTarget;

    private void FixedUpdate()
    {
        // 1. 캐스팅 시작 위치  2. 원의 반지름  3. 캐스팅 방향, 4. 캐스팅 길이,  5. 대상 레이어
        targets = Physics2D.CircleCastAll(transform.position, sancRange, Vector2.zero, 0, targetLayer);
        nearestTarget = GetNearest();
    }
    Transform GetNearest() // 가까운 값 반환 함수
    {
        Transform result = null;
        float diff = 100;

        foreach (RaycastHit2D target in targets)
        {
            Vector3 myPos = transform.position;
            Vector3 targetPos = target.transform.position;
            float curDiff = Vector3.Distance(myPos, targetPos); // Distance : 벡터 A와 B의 거리를 계산해주는 함수.

            if (curDiff < diff)
            {
                diff = curDiff;
                result = target.transform;
            }
        }
        return result;
    }
}
