using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBomb : Item
{
    ItemType itemType = ItemType.Bomb;

    public override void Drop()
    {
        if (!GameManagerGP.Instance._hasBomb)
        {
            GameManagerGP.Instance._hasBomb = true;
            GameManagerGP.Instance._score += 300;
        }
        else
        {
            GameManagerGP.Instance._score += 500;
        }
    }
}
