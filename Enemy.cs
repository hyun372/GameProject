using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Bazier Curve")]
    public Route route;
    public Transform startPoint;
    public Transform controlPoint;
    public Transform endPoint;
    public float _speed = .1f;
    private float _tParam = 0f;

    private Vector2 enemyPos;
    private Vector2 prePos;

    [Header("Fire")]
    public GameObject bulletPrefab;
    public float _fireDelay = 1.0f;

    private SpriteRenderer spriter;

    [Header("Health")]
    public float _maxHealth;
    public float _currentHealth;
    private WaitForSeconds _hitDelay = new WaitForSeconds(.1f);
    private bool _isDead = false;

    private void Awake()
    {
        spriter = GetComponentInChildren<SpriteRenderer>();

        route = GameManagerGP.Instance.routes[Random.Range(0, GameManagerGP.Instance.routes.Length)];

        startPoint = route.startPoint;
        controlPoint = route.controlPoint;
        endPoint = route.endPoint;

        prePos = transform.position;

        _currentHealth = _maxHealth;
        _isDead = false;
    }

    private void Start()
    {
        InvokeRepeating("Fire" , _fireDelay, _fireDelay);  
    }

    private void Update()
    {
        if (!GameManagerGP.Instance._isPlaying) { return; }

        if (_tParam < 1)
        {
            _tParam += Time.deltaTime * _speed;
            enemyPos = Mathf.Pow(1 - _tParam, 2) * startPoint.position +
                2 * (1 - _tParam) * _tParam * controlPoint.position +
                Mathf.Pow(_tParam, 2) * endPoint.position;

            RotateMovementDirectoin(enemyPos);

            transform.position = enemyPos;
            prePos = enemyPos;

        }

        if (transform.position.y < Camera.main.ViewportToWorldPoint(Vector2.zero).y - spriter.size.y)
        {
            Destroy(gameObject);
        }
    }


    private void RotateMovementDirectoin(Vector2 position)
    {
        Vector2 direction = position - prePos;
        if(direction != Vector2.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    void Fire()
    {
        if (!GameManagerGP.Instance._isPlaying) { return; }

        Instantiate(bulletPrefab, transform.position, Quaternion.identity);
    }

    public void TakeDamage(float damage)
    {
        StartCoroutine(Flash());
        _currentHealth -= damage;
        if(_currentHealth <= 0 && !_isDead)
        {
            Dead();
        }
    }
    IEnumerator Flash()
    {
        spriter.color = new Color(.8f, .8f, .8f);

        yield return _hitDelay;

        spriter.color = Color.white;
    }

    public void Dead()
    {
        Destroy(gameObject);
        _isDead = true;
        Instantiate(GameManagerGP.Instance.explosionPrefab, transform.position, Quaternion.identity);
        GameManagerGP.Instance._score += 100;
        if(Random.Range(0, 100) < GameManagerGP.Instance._itemDropRate)
        {   
            Instantiate(GameManagerGP.Instance.itemPrefabs[Random.Range(0, GameManagerGP.Instance.itemPrefabs.Length)], transform.position, Quaternion.identity);
        }
    }
}
