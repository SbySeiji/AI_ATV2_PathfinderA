using UnityEngine;

public class CameraManager3D : MonoBehaviour
{
    public Camera mainCamera;
    public float padding = 1.2f;
    public float tiltAngle = 45f; // inclinação (X)
    public float rotationY = 45f; // rotação lateral (Y)

    private void Start()
    {
        // Garante que temos uma câmera
        if (mainCamera == null)
            mainCamera = Camera.main;

        // Procura o grid pela TAG
        GameObject grid = GameObject.FindGameObjectWithTag("Grid");

        if (grid != null)
        {
            AdjustCamera(grid);
        }
        else
        {
            Debug.LogWarning("Nenhum objeto com a tag 'Grid' foi encontrado.");
        }
    }

    private void Update()
    {
        // Pressione ESPAÇO pra reajustar manualmente (debug)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject grid = GameObject.FindGameObjectWithTag("Grid");

            if (grid != null)
                AdjustCamera(grid);
        }
    }

    public void AdjustCamera(GameObject gridObject)
    {
        if (mainCamera == null || gridObject == null) return;

        Bounds bounds = CalculateGridBounds(gridObject);

        // Segurança: evita divisão estranha
        if (bounds.size == Vector3.zero)
        {
            Debug.LogWarning("Bounds do grid é zero.");
            return;
        }

        Vector3 center = bounds.center;

        // Considera largura (X) e profundidade (Z)
        float size = Mathf.Max(bounds.size.x, bounds.size.z) * padding;

        // FOV em radianos
        float fov = mainCamera.fieldOfView * Mathf.Deg2Rad;

        // Distância necessária pra caber tudo na tela
        float distance = size / Mathf.Tan(fov / 2f);

        // Rotação da câmera (inclinada)
        Quaternion rotation = Quaternion.Euler(tiltAngle, rotationY, 0f);

        // Direção da câmera
        Vector3 direction = rotation * Vector3.forward;

        // Calcula posição final
        Vector3 position = center - direction * distance;

        // Aplica
        mainCamera.transform.position = position;
        mainCamera.transform.rotation = rotation;
    }

    private Bounds CalculateGridBounds(GameObject gridObject)
    {
        Renderer[] renderers = gridObject.GetComponentsInChildren<Renderer>();

        if (renderers.Length == 0)
        {
            return new Bounds(gridObject.transform.position, Vector3.zero);
        }

        Bounds bounds = renderers[0].bounds;

        foreach (Renderer r in renderers)
        {
            bounds.Encapsulate(r.bounds);
        }

        return bounds;
    }

    // Debug visual (mostra o centro no editor)
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        GameObject grid = GameObject.FindGameObjectWithTag("Grid");
        if (grid == null) return;

        Bounds bounds = CalculateGridBounds(grid);
        Gizmos.DrawSphere(bounds.center, 0.3f);
    }
}