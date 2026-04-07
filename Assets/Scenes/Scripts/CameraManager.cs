using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera mainCamera;
    public float padding = 1f; // Espaço extra para garantir que o grid caiba inteiramente na visão
    private void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }
    public void AdjustCamera(GameObject gridObject)
    {
        if (mainCamera == null || gridObject == null) return;

        // Calcula os limites do grid considerando todos os filhos
        Bounds gridBounds = CalculateGridBounds(gridObject);

        // Ajusta o size baseado na altura e largura do grid e na relação de aspecto da câmera
        float screenRatio = Screen.width / (float)Screen.height;
        float targetRatio = gridBounds.size.x / gridBounds.size.y;

        if (screenRatio >= targetRatio)
        {
            mainCamera.orthographicSize = gridBounds.size.y / 2 + padding;
        }
        else
        {
            float differenceInSize = targetRatio / screenRatio;
            mainCamera.orthographicSize = (gridBounds.size.y / 2 + padding) * differenceInSize;
        }

        // Posiciona a câmera no centro do grid
        mainCamera.transform.position = new Vector3(gridBounds.center.x, gridBounds.center.y, mainCamera.transform.position.z);
    }

    private Bounds CalculateGridBounds(GameObject gridObject)
    {
        Bounds bounds = new Bounds(gridObject.transform.position, Vector3.zero);
        foreach (Renderer r in gridObject.GetComponentsInChildren<Renderer>())
        {
            bounds.Encapsulate(r.bounds);
        }
        return bounds;
    }
}
