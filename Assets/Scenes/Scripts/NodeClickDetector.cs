using UnityEngine;

public class NodeClickDetector : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main; // Assume que existe apenas uma câmera principal
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 0 é o botão esquerdo do mouse
        {
            RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject.GetComponent<Node>() != null)
            {
                // Obtém o componente Node do objeto clicado
                Node clickedNode = hit.collider.gameObject.GetComponent<Node>();
                // Chama o método que lida com o clique no Node
                FindObjectOfType<UIController>().NodeClicked(clickedNode);
            }
        }
    }
}
