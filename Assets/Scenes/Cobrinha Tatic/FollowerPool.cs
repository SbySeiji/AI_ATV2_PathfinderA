using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FollowerPool : MonoBehaviour
{
    [Header("Config")]
    public string followerTag = "Follower";

    [Header("Referências")]
    public Transform player;

    [Header("Auto Refresh")]
    public float refreshInterval = 0.5f;

    private List<Player2> pool = new List<Player2>();

    void Start()
    {
        RefreshPool();
        StartCoroutine(AutoRefresh());
    }

    IEnumerator AutoRefresh()
    {
        while (true)
        {
            yield return new WaitForSeconds(refreshInterval);
            RefreshFollowers();
        }
    }

    public void RefreshPool()
    {
        pool = FindObjectsOfType<Player2>(true)
            .Where(f => f.CompareTag(followerTag))
            .ToList();

        RefreshFollowers();
    }

    public void RefreshFollowers()
    {
        List<Player2> active = pool
            .Where(f => f.gameObject.activeSelf)
            .ToList();

        active = active.OrderBy(f => f.transform.GetSiblingIndex()).ToList();

        for (int i = 0; i < active.Count; i++)
        {
            if (i == 0)
            {
                active[i].target = player;
            }
            else
            {
                active[i].target = active[i - 1].transform;
            }
        }
    }

    public Player2 GetAvailableFollower()
    {
        var f = pool.FirstOrDefault(x => !x.gameObject.activeSelf);

        if (f == null) return null;

        f.gameObject.SetActive(true);

        RefreshFollowers();

        return f;
    }

    public void ReturnFollower(Player2 follower)
    {
        follower.gameObject.SetActive(false);
        RefreshFollowers();
    }
}