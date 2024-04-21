using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform firePos;
    
    private SpriteRenderer sprite;

    private Vector2 _inputVec;
    public float _speed = 5f;

    public GameObject bulletPrefab;
    public float _attackRate = .5f;
    public float _shotAngleRange = 30f;
    public float _timer;

    private Animator anim;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    float H => Input.GetAxis("Horizontal");
    float V => Input.GetAxis("Vertical");

    float x, y;

    void Update()
    {
        if (!GameManagerGP.Instance._isPlaying) { return; }

        _inputVec = new Vector2(H, V);

        _timer += Time.deltaTime;

        anim.SetFloat("Horizontal", x);
    }
    void FixedUpdate()
    {
        //Vector2 nextVec = _inputVec.normalized * _speed * Time.deltaTime;
        //rigid.MovePosition(rigid.position + nextVec);

        Move();

        if (_timer >= _attackRate && GameManagerGP.Instance._canShoot)
        {
            //if (Input.GetKey(KeyCode.Space))
            //{
            //    Fire();
            //}
            Fire();
        }
    }

    void Move()
    {
        _inputVec = new Vector3(x, y, 0);
        _inputVec.Normalize();


        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        max.x = max.x - sprite.size.x / 2f;
        min.x = min.x + sprite.size.x / 2f;

        max.y = max.y - sprite.size.y / 2f;
        min.y = min.y + sprite.size.y / 2f;

        Vector2 pos = transform.position;
        pos += _inputVec * _speed * Time.deltaTime;

        pos.x = Mathf.Clamp(pos.x, min.x, max.x);
        pos.y = Mathf.Clamp(pos.y, min.y, max.y);

        transform.position = pos;
    }

    public void OnStickChanged(Vector2 stickPos)
    {
        x = stickPos.x;
        y = stickPos.y;
    }

    void Fire()
    {
        if (GameManagerGP.Instance._bulletCount > 1)
        {
            if (GameManagerGP.Instance._bulletCount== 2)
            {
                Instantiate(bulletPrefab, firePos.position - new Vector3(.2f, 0, 0), Quaternion.Euler(0, 0, 0));
                Instantiate(bulletPrefab, firePos.position - new Vector3(-.2f, 0, 0), Quaternion.Euler(0, 0, 0));
            }
            else
            {
                float angleStep = _shotAngleRange / (GameManagerGP.Instance._bulletCount - 1);

                for (int i = 0; i < GameManagerGP.Instance._bulletCount; i++)
                {
                    float currentAngle = -_shotAngleRange / 2 + angleStep * i;


                    Instantiate(bulletPrefab, firePos.position, Quaternion.Euler(0, 0, currentAngle));
                }
            }
        }
        else
        {
            Instantiate(bulletPrefab, firePos.position, Quaternion.Euler(0, 0, 0));
        }
        

        _timer = 0;
    }

    public void UnBreakable()
    {
        StartCoroutine(DoUnBreakable());
    }
    private Color fadeColor = new Color(1, 1, 1, .3f);
    private WaitForSeconds fadeTime = new WaitForSeconds(.25f);
    IEnumerator DoUnBreakable()
    {
        GameManagerGP.Instance._isDamageable = false;
        for (int i = 0; i < 4; i++)
        {
            sprite.color = fadeColor;
            yield return fadeTime;
            sprite.color = Color.white;
            yield return fadeTime;
        }

        GameManagerGP.Instance._isDamageable = true;
    }

    public void Init()
    {
        sprite.color = Color.white;
        transform.position = Vector3.up * -3;
        _timer = 0;
    }
}
