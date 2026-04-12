using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Vector3 gridPosition;
    public Color baseColor;
    public float cost = 1f;
    public float distance = Mathf.Infinity;
    public Node previousNode = null;

    public List<Node> neighbours = new List<Node>();

    public bool visited = false;
    public bool isObstacle = false;
    public bool isStart = false;

    private GameManager gm;

    private Renderer r;

    public Color Color
    {
        get => r != null ? r.material.color : Color.white;
        set
        {
            if (r != null)
                r.material.color = value;
        }
    }
    void Awake()
    {
        gm = FindFirstObjectByType<GameManager>();
        r = GetComponent<Renderer>();
    }

    public void AddNeighbours(Dictionary<Vector3, Node> grid)
    {
        neighbours.Clear();

        Vector3[] directions = new Vector3[]
        {
            Vector3.right,   // +X
            Vector3.left,    // -X
            Vector3.forward, // +Z
            Vector3.back,    // -Z
            Vector3.up,      // +Y
            Vector3.down     // -Y
        };

        foreach (var dir in directions)
        {
            Vector3 neighbourPos = gridPosition + dir;

            if (grid.ContainsKey(neighbourPos))
            {
                Node neighbour = grid[neighbourPos];
                if (!neighbour.isObstacle)
                    neighbours.Add(neighbour);
            }
        }
    }

    public void ResetSearchData()
    {
        visited = false;
        distance = Mathf.Infinity;
        previousNode = null;
    }
}
