using UnityEngine;

public class CameraManager3D : MonoBehaviour
{
    public Camera mainCamera;

    [Header("Enquadramento")]
    public float padding = 1.2f;  // Margem extra ao redor do grid

    [Header("Ângulos da câmera")]
    public float tiltAngle = 45f;   // Inclinação vertical (eixo X)
    public float rotationY = 45f;   // Rotação horizontal (eixo Y)

    private void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        GameObject grid = GameObject.FindGameObjectWithTag("Grid");
        if (grid != null)
            AdjustCamera(grid);
        else
            Debug.LogWarning("[CameraManager3D] Nenhum objeto com a tag 'Grid' foi encontrado.");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject grid = GameObject.FindGameObjectWithTag("Grid");
            if (grid != null) AdjustCamera(grid);
        }
    }

    public void AdjustCamera(GameObject gridObject)
    {
        if (mainCamera == null || gridObject == null) return;

        Bounds bounds = CalculateGridBounds(gridObject);

        if (bounds.size == Vector3.zero)
        {
            Debug.LogWarning("[CameraManager3D] Bounds do grid é zero — prefabs ainda não instanciados?");
            return;
        }

        Vector3 center = bounds.center;

        // Usa a maior dimensão horizontal (X ou Z) para calcular distância
        float size = Mathf.Max(bounds.size.x, bounds.size.z) * padding;
        float fov = mainCamera.fieldOfView * Mathf.Deg2Rad;
        float distance = size / Mathf.Tan(fov / 2f);

        Quaternion rotation = Quaternion.Euler(tiltAngle, rotationY, 0f);
        Vector3 direction = rotation * Vector3.forward;
        Vector3 position = center - direction * distance;

        mainCamera.transform.position = position;
        mainCamera.transform.rotation = rotation;
    }

    private Bounds CalculateGridBounds(GameObject gridObject)
    {
        Renderer[] renderers = gridObject.GetComponentsInChildren<Renderer>();

        if (renderers.Length == 0)
            return new Bounds(gridObject.transform.position, Vector3.zero);

        Bounds bounds = renderers[0].bounds;
        foreach (Renderer r in renderers)
            bounds.Encapsulate(r.bounds);

        return bounds;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        GameObject grid = GameObject.FindGameObjectWithTag("Grid");
        if (grid == null) return;
        Bounds bounds = CalculateGridBounds(grid);
        Gizmos.DrawWireCube(bounds.center, bounds.size);
        Gizmos.DrawSphere(bounds.center, 0.3f);
    }
}
