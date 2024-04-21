using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private SpriteRenderer spriter;
    public Sprite[] bombSpr;

    private void Awake()
    {
        spriter = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        StartCoroutine(Explode());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
        }
    }

    WaitForSeconds wait = new WaitForSeconds(0.1f);
    IEnumerator Explode()
    {
        yield return wait;
        GetComponent<SpriteRenderer>().sprite = bombSpr[1];
        spriter.color = new Color(1f, 1f, 1f, .8f);
        yield return wait;
        GetComponent<SpriteRenderer>().sprite = bombSpr[2];
        spriter.color = new Color(1f, 1f, 1f, .6f);



        Destroy(gameObject);
    }
}
