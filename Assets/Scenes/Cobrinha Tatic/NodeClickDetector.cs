using UnityEngine;
using UnityEngine.EventSystems;

public class NodeClickDetector : MonoBehaviour
{
    private Camera mainCamera;
    private UIController uiController;

    void Start()
    {
        mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("Camera principal não encontrada! Use a tag 'MainCamera'.");
        }

        uiController = FindFirstObjectByType<UIController>();

        if (uiController == null)
        {
            Debug.LogError("UIController não encontrado na cena!");
        }
    }

    void Update()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        if (!Input.GetMouseButtonDown(0)) return;

        if (mainCamera == null || uiController == null) return;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 1f);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Node clickedNode = hit.collider.GetComponent<Node>();

            if (clickedNode != null)
            {
                uiController.NodeClicked(clickedNode);
            }
            else
            {
                Debug.Log("Clicou em objeto sem Node: " + hit.collider.name);
            }
        }
    }
}