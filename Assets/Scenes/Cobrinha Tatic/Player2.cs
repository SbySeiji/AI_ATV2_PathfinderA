using UnityEngine;

public class Player2 : MonoBehaviour
{
    public Transform target; // 👈 quem ele segue
    public float followSpeed = 5f;
    public float distance = 1.0f; // distância da frente

    void Update()
    {
        if (target == null) return;

        Vector3 dir = (transform.position - target.position).normalized;
        Vector3 desiredPosition = target.position + dir * distance;

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            Time.deltaTime * followSpeed
        );
    }
}