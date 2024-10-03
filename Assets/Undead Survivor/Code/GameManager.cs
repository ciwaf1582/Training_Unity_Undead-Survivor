using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Player player;

    void Awake()
    {
            Instance = this; // 자기 자신으로 초기화
    }


}
