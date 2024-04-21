using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float _speed;

    private void Update()
    {
        if (!GameManagerGP.Instance._isPlaying) { return; }

        transform.Translate(Vector2.down * _speed * Time.deltaTime);

        if (transform.position.y < Camera.main.ViewportToWorldPoint(new Vector2(0, 0)).y)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!GameManagerGP.Instance._isPlaying) { return; }

        if (collision.gameObject.CompareTag("Player") && GameManagerGP.Instance._isDamageable)
        {
            GameManagerGP.Instance._currentHP--;
            Destroy(gameObject);
            Instantiate(GameManagerGP.Instance.bulletExplosionPrefab, transform.position, Quaternion.identity);

        }
    }
}
