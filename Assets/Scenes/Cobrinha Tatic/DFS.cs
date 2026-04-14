using System.Collections.Generic;
using UnityEngine;
public class DFS
{
    private Color pathColor;
    private bool found;

    public void RunDFS(Node startNode, Node destinationNode, List<Node> nodes, Color color)
    {
        pathColor = color;
        found = false;

        BuscaProfundidade(startNode, destinationNode);
        VisualizarCaminho(destinationNode);
    }

    private void BuscaProfundidade(Node startNode, Node destinationNode)
    {
        // Pilha iterativa — evita stack overflow em grids 3D grandes
        Stack<Node> stack = new Stack<Node>();
        stack.Push(startNode);
        startNode.visited = true;

        while (stack.Count > 0 && !found)
        {
            Node current = stack.Pop();

            if (current == destinationNode)
            {
                found = true;
                Debug.Log("[DFS] Destino alcançado.");
                return;
            }

            foreach (Node neighbour in current.neighbours)
            {
                if (!neighbour.visited && !neighbour.isObstacle)
                {
                    neighbour.visited = true;
                    neighbour.previousNode = current;
                    stack.Push(neighbour);
                }
            }
        }

        if (!found)
            Debug.Log("[DFS] Destino não alcançado.");
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
