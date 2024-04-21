using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public enum ItemType
    {
        Health = 0,
        Bullet = 1,
        Bomb = 2,
        Follower = 3
    }

    private Animator anim;
    public ItemType itemId;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        anim.SetInteger("ItemID", (int)itemId);
    }

    public abstract void Drop();

    private void Update()
    {
        if (!GameManagerGP.Instance._isPlaying) { return; }

        transform.position += Vector3.down * 5f * Time.deltaTime;

        if(transform.position.y<Camera.main.ViewportToWorldPoint(Vector3.zero).y)
        {
            Destroy(gameObject);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Drop();
            Destroy(gameObject);
        }
    }

     
}
