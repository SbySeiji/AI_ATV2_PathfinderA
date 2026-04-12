using UnityEngine;

public class Orb : MonoBehaviour
{
    private FollowerManager followerManager;
    private OrbSpawner spawner;

    void Start()
    {
        followerManager = FindObjectOfType<FollowerManager>();
        spawner = FindObjectOfType<OrbSpawner>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 spawnPos = transform.position;

            followerManager.AddFollower(spawnPos);

            // desativa orbe
            gameObject.SetActive(false);

            // spawn próximo
            spawner.SpawnRandomOrb();
        }
    }
}