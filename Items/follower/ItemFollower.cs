using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFollower : Item
{
    public override void Drop()
    {
        Vector3 spawnPosition = GameManagerGP.Instance.player.transform.position - new Vector3(0, GameManagerGP.Instance.allFollowers.Count * 1.5f, 0); // 적절한 생성 위치
        GameObject newFollowerGO = Instantiate(GameManagerGP.Instance.followerPrefab, spawnPosition, Quaternion.identity);
        Follower newFollower = newFollowerGO.GetComponent<Follower>();
        newFollower.player = GameManagerGP.Instance.player.transform;
        GameManagerGP.Instance.allFollowers.Add(newFollower);
        GameManagerGP.Instance.UpdateFollowerLists();
        GameManagerGP.Instance._score += 300;
    }
}
