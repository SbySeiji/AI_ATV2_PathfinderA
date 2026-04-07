using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// BFS (Breadth First Search) é um algoritmo de busca em largura que percorre todos os nós de um grafo
/// Este exemplo aplica o algoritmo em um grid 2D
/// </summary>
public class BFS
{
    // Referência à lista de nós
    public List<Node> squares;
    Color pathColor;

    // Inicia a busca em largura e, após a conclusão, visualiza o caminho
    public void RunBFS(Node startNode, Node destinationNode, List<Node> nodes, Color color)
    {
        pathColor = color;
        squares = nodes;
        BuscaLargura(startNode);
        VisualizarCaminho(destinationNode);
    }

    // Método para realizar a busca em largura
    public void BuscaLargura(Node startNode)
    {
        // Reseta os valores dos nós
        foreach (var node in squares)
        {
            node.visited = false;
            node.distance = Mathf.Infinity;
            node.previousNode = null;
        }

        // A raiz é o primeiro nó a ser visitado
        startNode.visited = true;
        // A distância da raiz para ela mesma é 0
        startNode.distance = 0;

        // Cria uma fila para gerenciar os nós a serem visitados
        Queue<Node> queue = new Queue<Node>();
        // Adiciona o nó inicial à fila
        queue.Enqueue(startNode);

        // Enquanto a fila não estiver vazia, continua a busca
        while (queue.Count > 0)
        {
            // Remove o nó da fila e associa a uma variável temporária do nó atual
            Node currentNode = queue.Dequeue();
            // Para cada vizinho do nó atual
            foreach (var neighbor in currentNode.neighbors)
            {
                // Se o vizinho não foi visitado e não é um obstáculo
                // marca como visitado, calcula a distância e o enfileira
                if (!neighbor.visited && !neighbor.isObstacle)
                {
                    neighbor.visited = true;
                    neighbor.distance = currentNode.distance + 1;
                    neighbor.previousNode = currentNode;
                    queue.Enqueue(neighbor);
                }
            }
        }

        Debug.Log("Busca em largura realizada com sucesso");
    }

    // Método para visualizar o caminho (opcional)
    public void VisualizarCaminho(Node destino)
    {
        Node currentNode = destino;
        while (currentNode != null)
        {
            // Altera a cor do quadrado para indicar o caminho
            currentNode.square.GetComponent<SpriteRenderer>().color = pathColor;
            currentNode = currentNode.previousNode;
        }
    }
}
