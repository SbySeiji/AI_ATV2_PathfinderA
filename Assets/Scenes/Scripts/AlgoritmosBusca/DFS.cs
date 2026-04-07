using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DFS (Depth First Search) é um algoritmo de busca em profundidade que percorre todos os nós de um grafo
/// Este exemplo aplica o algoritmo em um grid 2D
/// </summary>
public class DFS
{
    // Referência à lista de nós
    public List<Node> squares;
    Color pathColor;

    // Inicia a busca em profundidade e, após a conclusão, visualiza o caminho
    public void RunDFS(Node startNode, Node destinationNode, List<Node> nodes, Color color)
    {
        pathColor = color;
        squares = nodes;
        foreach (Node node in squares)
        {
            node.visited = false;
            node.previousNode = null;
        }
        BuscaProfundidade(startNode, destinationNode);
        VisualizarCaminho(destinationNode);
    }

    // Método para realizar a busca em profundidade
    private void BuscaProfundidade(Node currentNode, Node destinationNode)
    {
        // Marca o nó atual como visitado
        currentNode.visited = true;
        // Se o nó atual é o destino, encerra a busca
        if (currentNode == destinationNode)
        {
            Debug.Log("Destino alcançado na busca em profundidade");
            return;
        }
        else
        {
            // Para cada vizinho do nó atual
            foreach (var neighbor in currentNode.neighbors)
            {
                // Se o vizinho não foi visitado e não é um obstáculo
                if (!neighbor.visited && !neighbor.isObstacle)
                {
                    // Marca o vizinho como visitado, define o nó atual como anterior e chama a busca recursivamente
                    neighbor.previousNode = currentNode;
                    BuscaProfundidade(neighbor, destinationNode);
                }
            }
        }


    }

    // Método para visualizar o caminho (opcional)
    private void VisualizarCaminho(Node destino)
    {
        Node currentNode = destino;
        while (currentNode != null)
        {
            currentNode.square.GetComponent<SpriteRenderer>().color = pathColor;
            currentNode = currentNode.previousNode;
        }
    }
}
