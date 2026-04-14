using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OrbSpawner : MonoBehaviour
{
    [Header("Grid (objeto pai dos nodes)")]
    public Transform gridRoot;

    [Header("Delay de inicialização")]
    public float startDelay = 0.2f;

    private List<GameObject> orbPool = new List<GameObject>();
    private GameObject currentOrb;

    void Start()
    {
        StartCoroutine(InitAfterGrid());
    }

    IEnumerator InitAfterGrid()
    {
        // espera um tempinho inicial
        yield return new WaitForSeconds(startDelay);

        // espera o grid ter filhos (nodes criados)
        while (gridRoot == null || gridRoot.childCount == 0)
            yield return null;

        SetupPool();
    }

    void SetupPool()
    {
        orbPool.Clear();

        // pega todos orbes dentro do grid (inclusive inativos)
        var foundOrbs = gridRoot
            .GetComponentsInChildren<Transform>(true)
            .Where(t => t.CompareTag("Orbe"))
            .Select(t => t.gameObject)
            .ToList();

        foreach (var orb in foundOrbs)
        {
            // move pro spawner mantendo posição global
            orb.transform.SetParent(transform, true);

            orb.SetActive(false);
            orbPool.Add(orb);
        }

        Debug.Log("Orbs encontrados: " + orbPool.Count);

        SpawnRandomOrb();
    }

    public void SpawnRandomOrb()
    {
        if (orbPool.Count == 0)
        {
            Debug.LogWarning("Nenhum orbe disponível!");
            return;
        }

        if (currentOrb != null)
            currentOrb.SetActive(false);

        GameObject next;

        do
        {
            next = orbPool[Random.Range(0, orbPool.Count)];
        }
        while (next == currentOrb && orbPool.Count > 1);

        currentOrb = next;
        currentOrb.SetActive(true);
    }
}