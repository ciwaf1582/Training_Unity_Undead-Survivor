using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // .. 프리팹들을 보관할 변수
    public GameObject[] prefabs;

    // .. 퓰 담당을 하는 리스트들
    List<GameObject>[] pools;

    private void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];

        for (int index = 0; index < pools.Length; index++)
        {
            pools[index] = new List<GameObject>();
        }
        Debug.Log(pools.Length);
    }
    public GameObject Get(int index)
    {
        GameObject select = null;

        // ... 선택한 풀의 비활성화된 게임 오브젝트 접근
        foreach (GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                //... 발견하면 select 변수에 할당
                select = item;
                select.SetActive(true);
                break;
            }
        }
        // ...못 찾았다면
        if (!select)
        {
            // ... 새롭게 생성하고 select에 할당
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }
        return select;
    }
}
