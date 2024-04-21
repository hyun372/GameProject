using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public Transform player; // �÷��̾��� Transform
    public float followSpeed = 5.0f; // �ȷο��� �̵� �ӵ�
    public int index; // �� �ȷο��� �ε���
    public float verticalSpacing = 1.0f; // ���� ����
    public List<Transform> otherFollowers; // �ٸ� �ȷο����� ����Ʈ
    public float minimumDistance = 1.0f; // �ȷο� ������ �ּ� �Ÿ�

    [Header("Attack")]
    public float _attackRate = 2.0f; // ���� �ӵ�
    public float _timer; // ���� Ÿ�̸�
    public GameObject bulletPrefab;

    void Update()
    {
        if(!GameManagerGP.Instance._isPlaying) { return; }

        FollowPlayer();
        MaintainDistance();
        _timer += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (_timer >= _attackRate)
        {
            Fire();
        }
    }

    private void FollowPlayer()
    {
        Vector3 targetPosition = new Vector3(player.position.x, player.position.y - index * verticalSpacing, player.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }

    private void MaintainDistance()
    {
        foreach (Transform other in otherFollowers)
        {
            float distance = Vector3.Distance(transform.position, other.position);
            if (distance < minimumDistance)
            {
                Vector3 direction = transform.position - other.position;
                direction.Normalize();
                transform.position += direction * (minimumDistance - distance); // �Ÿ� ����
            }
        }
    }

    private void Fire()
    {
        Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        _timer = 0;
    }
}
