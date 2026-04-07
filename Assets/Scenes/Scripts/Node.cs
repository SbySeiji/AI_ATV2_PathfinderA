using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public GameObject square;
    public Vector2 gridPosition;
    public float cost = 1f;
    public float distance = Mathf.Infinity;
    public bool visited = false;
    public Node previousNode = null;
    public bool isObstacle = false;
    public List<Node> neighbors = new List<Node>();

    public Node(GameObject square, Vector2 gridPosition)
    {
        this.square = square;
        this.gridPosition = gridPosition;
    }

    public void AddNeighbor(Node neighbor)
    {
        neighbors.Add(neighbor);
        
        // -----------------------------------------------------------
        // ativar a linha abaixo para setar o cost randomicamente.
        // Só vai funcionar no Dijkstra e no A*
        // assim o Dijkistra encontra o menor caminho baseado no custo
        // Se não tiver essa linha, o Dijkistra vai se comportar como o BFS
        
        cost = Random.Range(0.0f, 3.0f);
        
        // -----------------------------------------------------------
    }
}
