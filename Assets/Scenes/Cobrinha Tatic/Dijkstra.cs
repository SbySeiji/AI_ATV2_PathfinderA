using System.Collections.Generic;
using UnityEngine;
public class Dijkstra
{
    private Color pathColor;
    private List<Node> openList = new List<Node>();

    public void RunDijkstra(Node startNode, Node destinationNode, List<Node> nodes, Color color)
    {
        pathColor = color;
        openList.Clear();

        // GameManager já resetou os campos (distance = Infinity, visited = false)
        startNode.distance = 0;
        openList.Add(startNode);

        while (openList.Count > 0)
        {
            // Pega o nó com menor distância acumulada
            Node current = null;
            float smallest = Mathf.Infinity;
            foreach (Node node in openList)
            {
                if (node.distance < smallest)
                {
                    smallest = node.distance;
                    current = node;
                }
            }

            openList.Remove(current);

            if (current == destinationNode)
                break;

            foreach (Node neighbour in current.neighbours)
            {
                if (neighbour.isObstacle || neighbour.visited) continue;

                float tentative = current.distance + neighbour.cost;
                if (tentative < neighbour.distance)
                {
                    neighbour.distance = tentative;
                    neighbour.previousNode = current;

                    if (!openList.Contains(neighbour))
                        openList.Add(neighbour);
                }
            }

            current.visited = true;
        }

        VisualizarCaminho(destinationNode);
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
