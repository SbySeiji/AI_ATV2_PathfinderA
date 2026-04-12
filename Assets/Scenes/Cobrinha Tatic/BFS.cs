using System.Collections.Generic;
using UnityEngine;
public class BFS
{
    private List<Node> allNodes;
    private Color pathColor;

    public void RunBFS(Node startNode, Node destinationNode, List<Node> nodes, Color color)
    {
        pathColor = color;
        allNodes = nodes;

        BuscaLargura(startNode);
        VisualizarCaminho(destinationNode);
    }

    private void BuscaLargura(Node startNode)
    {
        // GameManager já resetou os campos antes de chamar RunBFS
        startNode.visited = true;
        startNode.distance = 0;

        Queue<Node> queue = new Queue<Node>();
        queue.Enqueue(startNode);

        while (queue.Count > 0)
        {
            Node current = queue.Dequeue();

            foreach (Node neighbour in current.neighbours)
            {
                if (!neighbour.visited && !neighbour.isObstacle)
                {
                    neighbour.visited = true;
                    neighbour.distance = current.distance + 1;
                    neighbour.previousNode = current;
                    queue.Enqueue(neighbour);
                }
            }
        }

        Debug.Log("[BFS] Busca em largura concluída.");
    }

    private void VisualizarCaminho(Node destination)
    {
        Node current = destination;
        while (current != null)
        {
            Renderer r = current.GetComponent<Renderer>();
            if (r != null) r.material.color = pathColor;
            current = current.previousNode;
        }
    }
}
