using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public enum SelectionState
{
    None,
    SelectingOrigin,
    SelectingDestination,
    Filled
}

public enum SearchType
{
    BFS,
    DFS,
    Dijkstra,
    AStar
}

public class UIController : MonoBehaviour
{
    public SelectionState currentState = SelectionState.None;
    public Node originNode = null;
    public Node destinationNode = null;

    [Header("Referências de cena")]
    public GameManager gameManager;

    [Header("Player")]
    public Player player; // AGORA É REFERÊNCIA DIRETA

    [Header("Controle")]
    public bool isWaitingForPlayer = false;

    [Header("Botões")]
    public Button selectOriginButton;
    public Button selectDestinationButton;
    public Button clearGridButton;
    public Button buscarButton;

    [Header("UI")]
    public TMP_Dropdown searchTypeDropdown;
    public TMP_Text infoText;

    public SearchType searchType = SearchType.BFS;

    void Start()
    {
        PopulateSearchTypeDropdown();

        infoText.text = "Clique em um cubo para começar.";

        clearGridButton.onClick.AddListener(() =>
        {
            originNode = null;
            destinationNode = null;
            currentState = SelectionState.None;
            isWaitingForPlayer = false;
            buscarButton.interactable = true;
            infoText.text = "Clique em um cubo para começar.";
        });
    }

    public void NodeClicked(Node clickedNode)
    {
        if (clickedNode.isObstacle) return;

        //BLOQUEIA clique enquanto player anda
        if (isWaitingForPlayer) return;

        // 1 ignora (primeiro clique da vida)
        if (currentState == SelectionState.None)
        {
            currentState = SelectionState.SelectingOrigin;
            return;
        }

        // 2 START
        if (currentState == SelectionState.SelectingOrigin)
        {
            if (originNode != null && !originNode.isObstacle)
            {
                Renderer prev = originNode.GetComponent<Renderer>();
                if (prev != null) prev.material.color = Color.white;
            }

            originNode = clickedNode;
            clickedNode.GetComponent<Renderer>().material.color = gameManager.startColor;

            currentState = SelectionState.SelectingDestination;
            return;
        }

        // 3 DESTINO + BUSCA
        if (currentState == SelectionState.SelectingDestination)
        {
            if (destinationNode != null && !destinationNode.isObstacle)
            {
                Renderer prev = destinationNode.GetComponent<Renderer>();
                if (prev != null) prev.material.color = Color.white;
            }

            destinationNode = clickedNode;
            clickedNode.GetComponent<Renderer>().material.color = gameManager.endColor;

            // roda busca
            gameManager.RunSearch(originNode, destinationNode, searchType);

            if (player != null && gameManager.lastPath != null)
            {
                isWaitingForPlayer = true;

                player.MoveAlongPath(gameManager.lastPath, () =>
                {
                    HandleAutoReset();
                });

                // chama seguidores
                Player2[] followers = FindObjectsByType<Player2>(FindObjectsSortMode.None);
            }

            currentState = SelectionState.Filled;
            return;
        }
    }

    //RESET AUTOMÁTICO (PASSO 4)
    void HandleAutoReset()
    {
        // limpa visual
        foreach (Node node in gameManager.nodes.Values)
        {
            node.Color = node.baseColor;
        }

        // destino vira novo start
        originNode = destinationNode;
        destinationNode = null;

        if (originNode != null)
        {
            Renderer r = originNode.GetComponent<Renderer>();
            if (r != null) r.material.color = gameManager.startColor;
        }

        gameManager.lastPath = null;

        currentState = SelectionState.SelectingDestination;

        isWaitingForPlayer = false;
    }

    void PopulateSearchTypeDropdown()
    {
        searchTypeDropdown.ClearOptions();
        List<string> options = new List<string>();

        foreach (var value in Enum.GetValues(typeof(SearchType)))
            options.Add(value.ToString());

        searchTypeDropdown.AddOptions(options);
    }
}