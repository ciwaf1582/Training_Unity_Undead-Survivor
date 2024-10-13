using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabID;
    public float damage;
    public int count;
    public float speed;

    float timer;
    Player player;

    private void Awake()
    {
        player = GameManager.instance.player;
    }
    void Update()
    {
        if (!GameManager.instance.isLive) return;

        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;
            default:
                timer += Time.deltaTime;
                if (timer > speed)
                {
                    timer = 0f;
                    Fire();
                }
                break;
        }
        if (Input.GetButtonDown("Jump"))
        {
            Leve1Up(10, 1);
        }
    }
    public void Leve1Up(float damage, int count) // 레벨 업
    {
        this.damage = damage * Character.Damage;
        this.count += count;

        if (id == 0)
        {
            Batch();
        }
        // .......... 실행되는 타이밍 ..........
        // ... 1. weapon이 Init 되었을 때    ...
        // ... 2. weapon이 LevelUp 되었을 때 ...
        // ... 3. gear가 Init 되었을 때      ...
        // ... 4. gear가 LevelUp 되었을 때   ...
        // ...... 추가로 들어올 무기에 적용 .....
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver); // "ApplyGear"이라는 메서드 이름의 오브젝트를 호출 하는 방법
    }
    public void Init(ItemData data) // 초기화
    {
        // Basic Set
        name = "Weapon " + data.itemId;
        transform.parent = player.transform; // 부모 설정
        transform.localPosition = Vector3.zero; // 부모 기준으로 (0, 0, 0)으로 설정

        // Property Set
        id = data.itemId;
        damage = data.baseDamage * Character.Damage;
        count = data.baseCount + Character.Count;

        for (int i = 0; i < GameManager.instance.pool.prefabs.Length; i++)
        {
            if (data.progectile == GameManager.instance.pool.prefabs[i])
            {
                prefabID = i;
                break;
            }
        }

        switch (id)
        {
            case 0:
                speed = 150 * Character.WeaponSpeed;
                Batch();
                break;
            default:
                speed = 0.5f * Character.WeaponRate;
                break;
        }
        // Hand Set
        Hand hand = player.hands[(int)data.itemType]; // 왼손(0), 오른손(1)
        hand.spriter.sprite = data.hand;
        hand.gameObject.SetActive(true);

        // .......... 실행되는 타이밍 ..........
        // ... 1. weapon이 Init 되었을 때    ...
        // ... 2. weapon이 LevelUp 되었을 때 ...
        // ... 3. gear가 Init 되었을 때      ...
        // ... 4. gear가 LevelUp 되었을 때   ...
        // ...... 추가로 들어올 무기에 적용 .....
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver); // "ApplyGear"이라는 메서드 이름의 오브젝트를 호출 하는 방법
    }
    void Batch()
    {
        for(int i = 0; i < count; i++)
        {
            Transform bullet;
            if (i < transform.childCount) // 부모 오브젝트의 자식 수보다 작은지 체크
            {
                bullet = transform.GetChild(i); // 작다면 자식으로 지정
            }
            else
            {
                // 새로운 총알을 가져온다
                bullet = GameManager.instance.pool.Get(prefabID).transform; // 부모를 바꾸기 위해 Transform
                bullet.parent = transform; // 새 총알의 부모는 현 오브젝트로 지정
            }
            
            bullet.localPosition = Vector3.zero; // 부모와 총알의 위치를 일치
            bullet.localRotation = Quaternion.identity; // 회전 초기화

            Vector3 rotVec = Vector3.forward * 360 * i / count; // 회전 벡터 계산(i의 값마다 방향값 저장)
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);

            bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero); // -1 is Infinity per.

        }
    }
    void Fire()
    {
        if (!player.scanner.nearestTarget)
            return;
        // 총알이 나가는 방향 계산
        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        Transform bullet = GameManager.instance.pool.Get(prefabID).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir); // 다른 방향 벡터로 회전
        bullet.GetComponent<Bullet>().Init(damage, count, dir);
    }
}
