using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GridGenerator gridGenerator;
    List<Node> squares;
    public GameObject squarePrefab;
    public GameObject grid;
    [Header("Grid Settings - Cuidado ao colocar valores muito altos!")]
    [Range(10, 200)]
    public int rows;
    [Range(10, 200)]
    public int cols;
    [Range(0, 50)]
    public int obstaclesPercentage;
    public Color startColor;
    public Color endColor;
    public Color pathColor;
    public CameraManager cameraManager;
    BFS bfs;
    DFS dfs;
    Dijkstra dijkistra;
    AStar aStar;

    void Start()
    {
        gridGenerator = new GridGenerator(rows, cols, squarePrefab, obstaclesPercentage, grid);
        squares = gridGenerator.GenerateGrid();
        cameraManager.AdjustCamera(grid);
        bfs = new BFS();
        dfs = new DFS();
        dijkistra = new Dijkstra();
        aStar = new AStar();
    }

    public void RunSearch(Node startNode, Node destinationNode, SearchType searchType)
    {
        switch(searchType)
        {
            case SearchType.BFS:
                bfs.RunBFS(startNode, destinationNode, squares, pathColor);
                break;
            case SearchType.DFS:
                dfs.RunDFS(startNode, destinationNode, squares, pathColor);
                break;
            case SearchType.Dijkstra:
                dijkistra.RunDijkstra(startNode, destinationNode, squares, pathColor);
                break;
            case SearchType.AStar:
                aStar.RunAStar(startNode, destinationNode, squares, pathColor);
                break;
        }        
    }
}
