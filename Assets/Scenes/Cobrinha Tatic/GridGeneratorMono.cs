using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gera o grid 3D sorteando entre vários tipos de nó (prefabs),
/// cada um com custo e cor próprios configuráveis no Inspector.
/// </summary>
public class GridGeneratorMono : MonoBehaviour
{
    // ── Estrutura que aparece como linha no Inspector ──────────────────────
    [System.Serializable]
    public class NodeType
    {
        public string name;          // só para identificar no Inspector
        public GameObject prefab;    // prefab com Node.cs + BoxCollider
        public Color color;          // cor visual do cubo
        public float cost = 1f;      // custo de travessia (usado por Dijkstra e A*)
        [Range(0, 100)]
        public int spawnWeight = 50; // peso relativo de sorteio (não precisa somar 100)
    }

    [Header("Dimensões do grid")]
    public int cols = 5;
    public int rows = 5;
    public int depth = 5;

    [Header("Tipos de nó")]
    public List<NodeType> nodeTypes = new List<NodeType>();

    [Header("Referências")]
    public GameManager gameManager;

    [Header("Obstáculos")]
    [Range(0, 80)]
    public int obstaclePercentage = 20;

    private List<Node> allNodes = new List<Node>();

    void Start()
    {
        GenerateGrid();
    }

    public void GenerateGrid()
    {
        if (nodeTypes == null || nodeTypes.Count == 0)
        {
            Debug.LogError("[GridGeneratorMono] Nenhum NodeType configurado no Inspector.");
            return;
        }

        allNodes.Clear();

        // Pré-calcula o peso total para o sorteio
        int totalWeight = 0;
        foreach (var nt in nodeTypes)
            totalWeight += Mathf.Max(0, nt.spawnWeight);

        float startX = transform.position.x - (cols / 2f);
        float startY = transform.position.y - (rows / 2f);
        float startZ = transform.position.z - (depth / 2f);

        for (int x = 0; x < cols; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                for (int z = 0; z < depth; z++)
                {
                    Vector3 worldPos = new Vector3(startX + x, startY + y, startZ + z);

                    // Sorteia qual NodeType usar com base no peso
                    NodeType chosen = PickNodeType(totalWeight);

                    GameObject obj = Instantiate(chosen.prefab, worldPos, Quaternion.identity, transform);
                    obj.name = $"Node_{x}_{y}_{z}_{chosen.name}";

                    Node node = obj.GetComponent<Node>();
                    node.gridPosition = new Vector3(x, y, z);
                    node.cost = chosen.cost;

                    Renderer r = obj.GetComponent<Renderer>();

                    // Sorteia se vira obstáculo
                    bool isObstacle = Random.Range(0, 100) < obstaclePercentage;
                    if (isObstacle)
                    {
                        node.isObstacle = true;
                        if (r != null) r.material.color = Color.black;
                    }
                    else
                    {
                        if (r != null) r.material.color = chosen.color;
                    }

                    allNodes.Add(node);
                    gameManager.RegisterNode(node);
                }
            }
        }

        gameManager.BuildGraph();
    }

    private NodeType PickNodeType(int totalWeight)
    {
        int roll = Random.Range(0, totalWeight);
        int accumulated = 0;

        foreach (var nt in nodeTypes)
        {
            accumulated += Mathf.Max(0, nt.spawnWeight);
            if (roll < accumulated)
                return nt;
        }

        return nodeTypes[0]; // fallback
    }

    public void ClearGrid()
    {
        foreach (Node node in allNodes)
        {
            if (node.isObstacle) continue;

            Renderer r = node.GetComponent<Renderer>();
            if (r == null) continue;

            // para restaurar a cor certa de cada tipo
            NodeType match = nodeTypes.Find(nt => Mathf.Approximately(nt.cost, node.cost));
            r.material.color = match != null ? match.color : Color.white;
        }
    }
}
