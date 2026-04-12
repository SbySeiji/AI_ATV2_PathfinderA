using System.Collections.Generic;
using UnityEngine;

public class AStar
{
    public List<Node> path = new List<Node>();

    private class NodeRecord
    {
        public Node node;
        public Node connection;       // De qual nó viemos
        public float costSoFar;       // g(n): custo real acumulado
        public float estimatedTotal;  // f(n) = g(n) + h(n)

        public NodeRecord(Node node)
        {
            this.node = node;
            connection = null;
            costSoFar = 0;
            estimatedTotal = Mathf.Infinity;
        }
    }

    private float Heuristic(Node a, Node b)
    {
        return Vector3.Distance(a.gridPosition, b.gridPosition);
    }

    public void RunAStar(Node startNode, Node destinationNode, List<Node> nodes)
    {
        path.Clear();

        Dictionary<Node, NodeRecord> records = new Dictionary<Node, NodeRecord>();
        List<NodeRecord> open = new List<NodeRecord>();
        List<NodeRecord> closed = new List<NodeRecord>();

        NodeRecord startRecord = new NodeRecord(startNode);
        startRecord.costSoFar = 0;
        startRecord.estimatedTotal = Heuristic(startNode, destinationNode);
        open.Add(startRecord);
        records[startNode] = startRecord;

        while (open.Count > 0)
        {
            // Pega o nó com menor f(n)
            NodeRecord current = open[0];
            foreach (var rec in open)
            {
                if (rec.estimatedTotal < current.estimatedTotal)
                    current = rec;
            }

            if (current.node == destinationNode) break;

            foreach (Node neighbour in current.node.neighbours)
            {
                if (neighbour.isObstacle) continue;

                float newCost = current.costSoFar + neighbour.cost;

                NodeRecord neighbourRecord;
                if (records.ContainsKey(neighbour))
                {
                    neighbourRecord = records[neighbour];
                    // Se já achamos caminho mais barato, ignora
                    if (neighbourRecord.costSoFar <= newCost) continue;
                }
                else
                {
                    neighbourRecord = new NodeRecord(neighbour);
                    records[neighbour] = neighbourRecord;
                }

                neighbourRecord.costSoFar = newCost;
                neighbourRecord.connection = current.node;
                neighbourRecord.estimatedTotal = newCost + Heuristic(neighbour, destinationNode);

                if (!open.Contains(neighbourRecord))
                    open.Add(neighbourRecord);
            }

            open.Remove(current);
            closed.Add(current);
        }

        // Reconstrói o caminho de trás pra frente via connection
        if (records.ContainsKey(destinationNode))
        {
            NodeRecord cur = records[destinationNode];
            while (cur.node != startNode)
            {
                path.Add(cur.node);
                cur = records[cur.connection];
            }
            path.Add(startNode);
            path.Reverse(); // opcional: deixa na ordem origem → destino
        }
        else
        {
            Debug.Log("[A*] Destino não alcançado.");
        }
    }
    public void ColorirCaminho(Color color)
    {
        foreach (Node node in path)
        {
            Renderer r = node.GetComponent<Renderer>();
            if (r != null) r.material.color = color;
        }
    }
}