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
    public void Leve1Up(float damage, int count) // ���� ��
    {
        this.damage = damage * Character.Damage;
        this.count += count;

        if (id == 0)
        {
            Batch();
        }
        // .......... ����Ǵ� Ÿ�̹� ..........
        // ... 1. weapon�� Init �Ǿ��� ��    ...
        // ... 2. weapon�� LevelUp �Ǿ��� �� ...
        // ... 3. gear�� Init �Ǿ��� ��      ...
        // ... 4. gear�� LevelUp �Ǿ��� ��   ...
        // ...... �߰��� ���� ���⿡ ���� .....
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver); // "ApplyGear"�̶�� �޼��� �̸��� ������Ʈ�� ȣ�� �ϴ� ���
    }
    public void Init(ItemData data) // �ʱ�ȭ
    {
        // Basic Set
        name = "Weapon " + data.itemId;
        transform.parent = player.transform; // �θ� ����
        transform.localPosition = Vector3.zero; // �θ� �������� (0, 0, 0)���� ����

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
        Hand hand = player.hands[(int)data.itemType]; // �޼�(0), ������(1)
        hand.spriter.sprite = data.hand;
        hand.gameObject.SetActive(true);

        // .......... ����Ǵ� Ÿ�̹� ..........
        // ... 1. weapon�� Init �Ǿ��� ��    ...
        // ... 2. weapon�� LevelUp �Ǿ��� �� ...
        // ... 3. gear�� Init �Ǿ��� ��      ...
        // ... 4. gear�� LevelUp �Ǿ��� ��   ...
        // ...... �߰��� ���� ���⿡ ���� .....
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver); // "ApplyGear"�̶�� �޼��� �̸��� ������Ʈ�� ȣ�� �ϴ� ���
    }
    void Batch()
    {
        for(int i = 0; i < count; i++)
        {
            Transform bullet;
            if (i < transform.childCount) // �θ� ������Ʈ�� �ڽ� ������ ������ üũ
            {
                bullet = transform.GetChild(i); // �۴ٸ� �ڽ����� ����
            }
            else
            {
                // ���ο� �Ѿ��� �����´�
                bullet = GameManager.instance.pool.Get(prefabID).transform; // �θ� �ٲٱ� ���� Transform
                bullet.parent = transform; // �� �Ѿ��� �θ�� �� ������Ʈ�� ����
            }
            
            bullet.localPosition = Vector3.zero; // �θ�� �Ѿ��� ��ġ�� ��ġ
            bullet.localRotation = Quaternion.identity; // ȸ�� �ʱ�ȭ

            Vector3 rotVec = Vector3.forward * 360 * i / count; // ȸ�� ���� ���(i�� ������ ���Ⱚ ����)
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);

            bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero); // -1 is Infinity per.

        }
    }
    void Fire()
    {
        if (!player.scanner.nearestTarget)
            return;
        // �Ѿ��� ������ ���� ���
        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        Transform bullet = GameManager.instance.pool.Get(prefabID).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir); // �ٸ� ���� ���ͷ� ȸ��
        bullet.GetComponent<Bullet>().Init(damage, count, dir);
    }
}
