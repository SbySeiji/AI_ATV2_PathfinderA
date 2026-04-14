using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager3D : MonoBehaviour
{
    public Camera mainCamera;

    [Header("Enquadramento")]
    public float padding = 1.2f;

    [Header("Ângulos da câmera")]
    public float tiltAngle = 45f;
    public float rotationY = 45f;

    [Header("Zoom")]
    public float zoomSpeed = 10f;
    public float minDistance = 5f;
    public float maxDistance = 50f;

    [Header("Suavização")]
    public float smoothSpeed = 5f; // quanto maior, mais rápido chega

    private float targetDistance;
    private float currentDistance;

    private Vector3 currentCenter;

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        StartCoroutine(InitCamera());
    }

    IEnumerator InitCamera()
    {
        yield return null; // espera 1 frame

        GameObject grid = GameObject.FindGameObjectWithTag("Grid");

        if (grid != null)
            AdjustCamera(grid);
    }

    private void Update()
    {
        GameObject grid = GameObject.FindGameObjectWithTag("Grid");
        if (grid == null) return;

        // SCROLL
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            targetDistance -= scroll * zoomSpeed;
            targetDistance = Mathf.Clamp(targetDistance, minDistance, maxDistance);
        }

        // SUAVIZAÇÃO (ESSA É A MÁGICA)
        currentDistance = Mathf.Lerp(currentDistance, targetDistance, Time.deltaTime * smoothSpeed);

        UpdateCameraPosition();

        // RESET
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AdjustCamera(grid);
        }
    }

    public void AdjustCamera(GameObject gridObject)
    {
        if (mainCamera == null || gridObject == null) return;

        Bounds bounds = CalculateGridBounds(gridObject);

        if (bounds.size == Vector3.zero)
            return;

        currentCenter = bounds.center;

        float size = Mathf.Max(bounds.size.x, bounds.size.z) * padding;
        float fov = mainCamera.fieldOfView * Mathf.Deg2Rad;

        float idealDistance = size / Mathf.Tan(fov / 2f);

        targetDistance = Mathf.Clamp(idealDistance, minDistance, maxDistance);
        currentDistance = targetDistance;

        UpdateCameraPosition();
    }

    private void UpdateCameraPosition()
    {
        Quaternion rotation = Quaternion.Euler(tiltAngle, rotationY, 0f);
        Vector3 direction = rotation * Vector3.forward;

        Vector3 position = currentCenter - direction * currentDistance;

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
}