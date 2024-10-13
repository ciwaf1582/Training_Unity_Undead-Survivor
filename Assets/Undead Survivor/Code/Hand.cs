using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public bool isLeft;
    public SpriteRenderer spriter;

    SpriteRenderer player;

    Vector3 rifgtPos = new Vector3(0.35f, -0.15f, 0);
    Vector3 rifgtPosReverse = new Vector3(-0.15f, -0.15f, 0);
    Quaternion leftRot = Quaternion.Euler(0, 0, -35);
    Quaternion leftRotReverse = Quaternion.Euler(0, 0, -135);

    private void Awake()
    {
        player = GetComponentsInParent<SpriteRenderer>()[1]; // �θ� ���� �ϱ� ������ [1]�� ����
    }
    private void LateUpdate()
    {
        bool isReverse = player.flipX;
        if (isLeft) // ���� ����
        {
            transform.localRotation = isReverse ? leftRotReverse : leftRot;
            spriter.flipY = isReverse;
            spriter.sortingOrder = isReverse ? 4 : 6;
        }
        else        // ���Ÿ� ����
        {
            transform.localPosition = isReverse ? rifgtPosReverse : rifgtPos;
            spriter.flipX = isReverse;
            spriter.sortingOrder = isReverse ? 6 : 4;
        }
    }
}
