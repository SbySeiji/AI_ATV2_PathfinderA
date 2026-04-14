using UnityEngine;

public class Orb : MonoBehaviour
{
    private FollowerPool pool;
    private OrbSpawner spawner;

    void Start()
    {
        pool = FindObjectOfType<FollowerPool>();
        spawner = FindObjectOfType<OrbSpawner>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        var follower = pool.GetAvailableFollower();

        if (follower == null) return;

        follower.transform.position = transform.position;

        pool.RefreshFollowers();

        gameObject.SetActive(false);

        spawner.SpawnRandomOrb();
    }
}