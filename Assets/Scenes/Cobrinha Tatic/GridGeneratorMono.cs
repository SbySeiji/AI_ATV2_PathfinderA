using System.Collections.Generic;
using UnityEngine;

public class GridGeneratorMono : MonoBehaviour
{
    [System.Serializable]
    public class NodeType
    {
        public string name;
        public GameObject prefab;
        public Color color;
        public float cost = 1f;
        [Range(0, 100)]
        public int spawnWeight = 50;
    }

    [Header("Dimensões do grid")]
    public int cols = 5;
    public int rows = 5;
    public int depth = 5;

    [Header("Tipos de nó")]
    public List<NodeType> nodeTypes = new List<NodeType>();

    [Header("Referências")]
    public GameManager gameManager;

    private List<Node> allNodes = new List<Node>();

    void Start()
    {
        GenerateGrid();
    }

    public void GenerateGrid()
    {
        if (nodeTypes == null || nodeTypes.Count == 0)
        {
            Debug.LogError("[GridGeneratorMono] Nenhum NodeType configurado.");
            return;
        }

        // Limpa grid antigo (evita duplicação)
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        allNodes.Clear();

        // Soma dos pesos
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

                    NodeType chosen = PickNodeType(totalWeight);

                    GameObject obj = Instantiate(chosen.prefab, worldPos, Quaternion.identity, transform);
                    obj.name = $"Node_{x}_{y}_{z}_{chosen.name}";

                    Node node = obj.GetComponent<Node>();
                    node.gridPosition = new Vector3(x, y, z);
                    node.cost = chosen.cost;

                    Renderer r = obj.GetComponent<Renderer>();

                    if (r != null)
                    {
                        node.baseColor = chosen.color;
                        node.Color = node.baseColor;
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

        return nodeTypes[0];
    }
}