using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{

    [Header("Body")]
    public float _currentHealth;
    private SpriteRenderer bodySpriter;

    private WaitForSeconds _hitDelay = new WaitForSeconds(.1f);

    [Header("Movement")]
    public float _speed = 1f;
    public float _width = 5.0f;
    public float _height = 3.0f;
    public float _duration = 5.0f;
    private float _moveTimer = 0.0f;
    public bool _isAppeared = true;

    [Header("Fire")]
    public GameObject bulletPrefab;
    public GameObject patternBulletPrefab;
    public Transform firePoint;
    private float _fireTimer = 0.0f;
    private float _patternTimer = 0.0f;
    public int _patternBulletCount = 3;

    public bool _armBroken = false;

    private bool _isDead = false;

    private void Awake()
    {
        bodySpriter = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (!GameManagerGP.Instance._isPlaying) { return; }

        Movement();

        if (_isAppeared) { return; }
        Fire();

        if (_armBroken)
        {
            _patternBulletCount = 5;
        }

        if (_currentHealth <= 0 && !_isDead)
        {
            Dead();
        }
    }

    private void Movement()
    {
        _moveTimer += Time.deltaTime * _speed;

        if (_isAppeared)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(0, 3, 0), _speed * Time.deltaTime);
        }
        else
        {
            float x = Mathf.Cos(_moveTimer / _duration * 2 * Mathf.PI) * _width;
            float y = Mathf.Sin(_moveTimer / _duration * 2 * Mathf.PI) * Mathf.Cos(_moveTimer / _duration * 2 * Mathf.PI) * _height;
            transform.position = new Vector3(x, y + 3, 0);
        }

        if (_moveTimer > _duration)
        {
            if (_isAppeared)
            {
                _isAppeared = false;
                _moveTimer = 1.25f;
            }
            else
            {
                _moveTimer = 0.0f;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        StartCoroutine(Flash());
        _currentHealth -= damage;
    }
    IEnumerator Flash()
    {
        bodySpriter.color = new Color(1.0f, .8f, 1.0f);

        yield return _hitDelay;

        bodySpriter.color = Color.white;
    }

    public void Fire()
    {
        _fireTimer += Time.deltaTime;
        _patternTimer += Time.deltaTime;

        if (_fireTimer > 1)
        {
            Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            _fireTimer = 0;
        }

        if (_patternTimer > 2)
        {
            float angleStep = 60f / _patternBulletCount - 1;

            for (int i = 0; i < _patternBulletCount; i++)
            {
                float currentAngle = -30 + i * angleStep;

                Instantiate(patternBulletPrefab, firePoint.position, Quaternion.Euler(0, 0, currentAngle));
            }

            _patternTimer = 0;
        }
        
    }

    void Dead()
    {
        GameManagerGP.Instance._score += 3000;
        StartCoroutine(DoDead());
    }
    WaitForSeconds explosionDelay = new WaitForSeconds(.5f);
    IEnumerator DoDead()
    {
        _isDead = true;
        yield return explosionDelay;
        for (int i = 0; i < Random.Range(1, 2); i++)
        {
            Instantiate(GameManagerGP.Instance.explosionPrefab, new Vector2(transform.position.x + Random.Range(-2f, 2f), transform.position.y + Random.Range(-1f, 1f)), Quaternion.identity);
        }
        yield return explosionDelay;
        for (int i = 0; i < Random.Range(1, 2); i++)
        {
            Instantiate(GameManagerGP.Instance.explosionPrefab, new Vector2(transform.position.x + Random.Range(-2f, 2f), transform.position.y + Random.Range(-1f, 1f)), Quaternion.identity);
        }
        yield return explosionDelay;
        for (int i = 0; i < Random.Range(1, 2); i++)
        {
            Instantiate(GameManagerGP.Instance.explosionPrefab, new Vector2(transform.position.x + Random.Range(-2f, 2f), transform.position.y + Random.Range(-1f, 1f)), Quaternion.identity);
        }

        yield return new WaitForSeconds(1f);
        GameManagerGP.Instance._canShoot = false;
        GameManagerGP.Instance._isDamageable = false;
        GameManagerGP.Instance._isVictory = true;
        GameManagerGP.Instance.GameOver();
    }
}
