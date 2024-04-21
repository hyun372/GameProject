using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float _damage;
    public float _speed;



    private void Awake()
    {
        _damage = GameManagerGP.Instance._attackDamage;
    }

    private void Update()
    {
        if (!GameManagerGP.Instance._isPlaying) { return; }

        transform.Translate(Vector2.up * _speed * Time.deltaTime);

        if (transform.position.y > Camera.main.ViewportToWorldPoint(new Vector2(1, 1)).y)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!GameManagerGP.Instance._isAttackable) { return; }

        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().TakeDamage(_damage);
            Destroy(gameObject);
            Instantiate(GameManagerGP.Instance.bulletExplosionPrefab, transform.position, Quaternion.identity);
        }

        if(collision.CompareTag("Boss"))
        {
            if(collision.GetComponent<BossArm>() != null)
            {
                collision.GetComponent<BossArm>().TakeDamage(_damage);
            }
            if (collision.GetComponent<Boss>() != null)
            {
                collision.GetComponent<Boss>().TakeDamage(_damage);
            }
            Destroy(gameObject);
            Instantiate(GameManagerGP.Instance.bulletExplosionPrefab, transform.position, Quaternion.identity);
        }
    }
}
