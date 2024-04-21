using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBullet : Item
{
    ItemType itemType = ItemType.Bullet;

    public override void Drop()
    {
        GameManagerGP.Instance._bulletCount++;
        GameManagerGP.Instance._score += 300;
    }
}
