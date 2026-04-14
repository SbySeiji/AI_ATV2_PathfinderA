using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform target;

    [Header("Movimento")]
    public float speed = 5f;

    [Header("Distâncias")]
    public float minDistance = 2f;
    public float maxDistance = 6f;

    void Update()
    {
        if (target == null) return;

        float distance = Vector3.Distance(transform.position, target.position);

        if (distance > minDistance && distance <= maxDistance)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }

        if (distance > maxDistance)
        {
            Vector3 pullPosition = target.position - (target.forward * minDistance);
            transform.position = Vector3.Lerp(transform.position, pullPosition, Time.deltaTime * 10f);
        }
    }
}