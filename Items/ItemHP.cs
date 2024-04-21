using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHP : Item
{
    ItemType itemType = ItemType.Health;

    public override void Drop()
    {
        if(GameManagerGP.Instance._currentHP < 3)
        {
            GameManagerGP.Instance._currentHP++;
            GameManagerGP.Instance._score += 300;
        }
        else
        {
            GameManagerGP.Instance._score += 1000;
        }
    }
}
