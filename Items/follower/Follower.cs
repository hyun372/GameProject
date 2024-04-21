using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public Transform player; // 플레이어의 Transform
    public float followSpeed = 5.0f; // 팔로워의 이동 속도
    public int index; // 이 팔로워의 인덱스
    public float verticalSpacing = 1.0f; // 수직 간격
    public List<Transform> otherFollowers; // 다른 팔로워들의 리스트
    public float minimumDistance = 1.0f; // 팔로워 사이의 최소 거리

    [Header("Attack")]
    public float _attackRate = 2.0f; // 공격 속도
    public float _timer; // 공격 타이머
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
                transform.position += direction * (minimumDistance - distance); // 거리 조정
            }
        }
    }

    private void Fire()
    {
        Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        _timer = 0;
    }
}
