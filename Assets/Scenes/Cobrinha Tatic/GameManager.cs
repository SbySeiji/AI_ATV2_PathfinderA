using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    [Header("Referências")]
    public GridGeneratorMono gridGenerator;

    [Header("Cores")]
    public Color startColor = Color.green;
    public Color endColor = Color.red;
    public Color pathColor = Color.yellow;
    // Dicionário posição → nó, usado pelo AddNeighbours
    public Dictionary<Vector3, Node> nodes;
    public List<Node> lastPath;
    void Awake()
    {
        nodes = new Dictionary<Vector3, Node>();
    }

    public void RegisterNode(Node node)
    {
        nodes[node.gridPosition] = node;
    }

    public void BuildGraph()
    {
        foreach (Node node in nodes.Values)
            node.AddNeighbours(nodes);
    }

    public void RunSearch(Node origin, Node destination, SearchType type)
    {
        foreach (Node node in nodes.Values)
            node.ResetSearchData();

        List<Node> allNodes = nodes.Values.ToList();

        switch (type)
        {
            case SearchType.BFS:
                BFS bfs = new BFS();
                bfs.RunBFS(origin, destination, allNodes, pathColor);
                break;

            case SearchType.DFS:
                DFS dfs = new DFS();
                dfs.RunDFS(origin, destination, allNodes, pathColor);
                break;

            case SearchType.Dijkstra:
                Dijkstra dijkstra = new Dijkstra();
                dijkstra.RunDijkstra(origin, destination, allNodes, pathColor);
                break;

            case SearchType.AStar:
                AStar astar = new AStar();
                astar.RunAStar(origin, destination, allNodes);
                lastPath = astar.path;

                foreach (Node n in lastPath)
                {
                    Renderer r = n.GetComponent<Renderer>();
                    if (r != null) r.material.color = pathColor;
                }
                break;
        }
    }
}
