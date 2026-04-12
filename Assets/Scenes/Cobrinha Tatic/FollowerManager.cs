using System.Collections.Generic;
using UnityEngine;

public class FollowerManager : MonoBehaviour
{
    public GameObject followerPrefab;
    public Transform player;

    private List<Player2> followers = new List<Player2>();

    public void AddFollower(Vector3 spawnPosition)
    {
        GameObject obj = Instantiate(followerPrefab, spawnPosition, Quaternion.identity);

        Player2 newFollower = obj.GetComponent<Player2>();

        if (followers.Count == 0)
        {
            newFollower.target = player;
        }
        else
        {
            newFollower.target = followers[followers.Count - 1].transform;
        }

        followers.Add(newFollower);
    }
}