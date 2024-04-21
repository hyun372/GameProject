using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BossArm : MonoBehaviour
{
    public Boss boss;

    [Header("Arm")]
    public float _currentHealth;
    private SpriteRenderer armSpriter;
    public Sprite brokenArm;
    private BoxCollider2D armCollider;
    public bool _isBroken = false;

    [Header("Rotate")]
    public float _rotateSpeed;
    public float _rotateTimer;
    public float _rotateDelay = 3.0f;

    [Header("Fire")]
    public GameObject bulletPrefab;
    public Transform firePoint;

    private WaitForSeconds _hitDelay = new WaitForSeconds(.1f);

    private void Awake()
    {
        armSpriter = GetComponent<SpriteRenderer>();
        armCollider = GetComponent<BoxCollider2D>();
        if (boss == null)
        {
            boss = GetComponentInParent<Boss>();
        }
    }

    private void Update()
    {
        if (!GameManagerGP.Instance._isPlaying) { return; }

        if (_isBroken) { return; }

        if (_currentHealth <= 0)
        {
            armSpriter.sprite = brokenArm;
            armCollider.enabled = false;
            Instantiate(GameManagerGP.Instance.explosionPrefab, transform.position, Quaternion.identity);
            _isBroken = true;
            boss._armBroken = true;
            GameManagerGP.Instance._score += 500;
        }

        LookAtPlayer();
    }

    public void TakeDamage(float damage)
    {
        StartCoroutine(Flash());
        _currentHealth -= damage;
        boss._currentHealth -= damage * .25f;
    }
    IEnumerator Flash()
    {
        armSpriter.color = new Color(1.0f, .8f, 1.0f);

        yield return _hitDelay;

        armSpriter.color = Color.white;
    }

    

    private void LookAtPlayer()
    {
        _rotateTimer += Time.deltaTime;
        if (_rotateTimer > _rotateDelay)
        {
            Vector2 direction = GameManagerGP.Instance.player.transform.position - transform.position;
            float anlge = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(anlge + 90, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, _rotateSpeed * Time.deltaTime);

            if(_rotateTimer > _rotateDelay + 3.0f)
            {
                StartCoroutine(FireAtPlayer(rotation));
                _rotateTimer = 0;

            }
        }
    }

    WaitForSeconds fireRateDelay = new WaitForSeconds(.15f);
    IEnumerator FireAtPlayer(Quaternion rotation)
    {
        for (int i = 0; i < 3; i++)
        {
            // ÃÑ¾Ë ¹ß»ç
            Instantiate(bulletPrefab, firePoint.position, rotation);
            yield return fireRateDelay;
        }        
    }
}
